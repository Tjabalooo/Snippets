# ActiveSessionTracker

## Target
ASP.Net Core projects that use Razor-pages.

## What
Code that keeps track of how many uniqe HttpContext-sessions are being used to fetch views from a site.

## How
##### startup.cs
Add *IActiveSessionTracker* by using the extension method provided. The timeout given will be used when deciding if sessions are active or not.
```C#
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddActiveSessionTracker(5000);
    ...
}
```
##### _Layout.cshtml
Inject *IActiveSessionTracker* and call *UpdateCurrentSession()* at the top of the shared layout.
```cshtml
@inject TJL.ActiveSessionTracker.IActiveSessionTracker sessionTracker
@{sessionTracker.UpdateCurrentSession();}
```
##### [someFile.cs]
Inject *IActiveSessionTracker* and call *GetNumberOfActiveSessions()* to get the active session count.
```C#
...
_activeSessionTracker = (IActiveSessionTracker)[DependencyInjectedOrResolvedValue];
var activeSessions = _activeSessionTracker.GetNumberOfActiveSessions();
...
```