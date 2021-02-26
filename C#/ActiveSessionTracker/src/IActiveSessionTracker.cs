namespace TJL.ActiveSessionTracker
{
    public interface IActiveSessionTracker
    {
        int GetNumberOfActiveSessions();

        void UpdateCurrentSession();
    }
}
