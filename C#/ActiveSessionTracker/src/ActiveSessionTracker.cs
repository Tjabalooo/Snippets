using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TJL.ActiveSessionTracker
{
    public class ActiveSessionTracker : IActiveSessionTracker
    {
        private class ActiveSession
        {
            private readonly long _lifetime;
            private long _timeLeft;

            public ActiveSession(long lifetime)
            {
                _lifetime = lifetime;
                _timeLeft = lifetime;
            }

            public bool IsDead() { return _timeLeft <= 0; }
            public void ResetTimeLeft() { _timeLeft = _lifetime; }
            public void Update(long millisecondsSinceLastCall) { _timeLeft -= millisecondsSinceLastCall; }
        }

        private readonly object _writeLock;

        private readonly Dictionary<string, ActiveSession> _activeSessions;
        private readonly IServiceProvider _services;
        private readonly long _millisecondsBeforeSessionIsConsideredCold;
        private int _lastKnownCount;
        private Stopwatch _stopwatch;

        public ActiveSessionTracker(IServiceProvider services, long millisecondsBeforeSessionIsConsideredCold)
        {
            _writeLock = new object();
            _activeSessions = new Dictionary<string, ActiveSession>();
            _millisecondsBeforeSessionIsConsideredCold = millisecondsBeforeSessionIsConsideredCold;
            _services = services;
            _lastKnownCount = 0;
            _stopwatch = new Stopwatch();
        }

        private void writeToActiveSessionsCollection(Action<Dictionary<string, ActiveSession>> writeAction)
        {
            lock (_writeLock)
            {
                writeAction(_activeSessions);
                _lastKnownCount = _activeSessions.Count;
            }
        }

        private void updateActiveSessions()
        {
            writeToActiveSessionsCollection((activeSessions) =>
            {
                if (!_stopwatch.IsRunning)
                    _stopwatch.Start();

                var time = _stopwatch.ElapsedMilliseconds;
                if (time > 1000)
                {
                    var deadSessions = new List<string>();
                    foreach (var pair in activeSessions)
                    {
                        pair.Value.Update(time);
                        if (pair.Value.IsDead())
                            deadSessions.Add(pair.Key);
                    }
                    deadSessions.ForEach(key => activeSessions.Remove(key));
                    _stopwatch.Restart();
                }
            });
        }

        public int GetNumberOfActiveSessions()
        {
            updateActiveSessions();

            return _lastKnownCount;
        }

        public void UpdateCurrentSession()
        {
            var httpContextAccessor = (IHttpContextAccessor)_services.GetService(typeof(IHttpContextAccessor));
            var session = httpContextAccessor.HttpContext.Session;

            string sessionId = session.GetString(SessionKeys.SessionId);
            if (sessionId == null)
            {
                sessionId = Guid.NewGuid().ToString();
                session.SetString(SessionKeys.SessionId, sessionId);
            }

            writeToActiveSessionsCollection((activeSessions) =>
            {
                if (activeSessions.ContainsKey(sessionId))
                    activeSessions[sessionId].ResetTimeLeft();
                else
                    activeSessions.Add(sessionId, new ActiveSession(_millisecondsBeforeSessionIsConsideredCold));
            });

            updateActiveSessions();
        }
    }
}
