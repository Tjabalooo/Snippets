# PrometheusNet

## Target

Any .Net setup capable of adding prometheus-net as NuGet package.

## Prerequisites

Knowledge of [prometheus-net](https://github.com/prometheus-net/prometheus-net). This snippet is a way of using the ***prometheus-net*** library in a dependency injectable way.

## What

A design that places all prometheus-net metrics, grouped on metric type, in one injectable interface. The design uses ***partial*** to place the implementaion of each group in its own file while still being able to make the implementation private.

Keeping the implementations private while exposing them only through their interfaces creates a factory-like pattern, making it harder to accidentally create a metric outside of their intended scope. Prometheus-net metrics must be static and creating them by mistake as anything else then that would give an undefined behaviour.

## How

Add the folder to your project and update the namespace (which most likely doesn't fit your project).

Add the interface as a ***singleton*** to your ***IoC-container***. The example below is from ***Startup.cs*** in an ASP.Net Core application, but the same behaviour could be achieved through any IoC-tool or, if all else fails, by rewriting ***PrometheusNet.cs*** to turn it into a singleton on its own.

```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IPrometheusMetrics, PrometheusMetrics>();
}
```

All metrics added (follow the pattern used by ***DummyCounter***) will be reachable through ***IPrometheusNet***. This means that all you need to do is inject this interface anywhere you need to update a metric.

```C#
public class HomeController : Controller
{
    private readonly IPrometheusMetrics _metrics;
    
    public HomeController(IPrometheusMetrics metrics)
    {
        _metrics = metrics;
    }
    
    public IActionResult Index()
    {
        _metrics.Counters.DummyCounter.Inc();
        return View();
    }
}
```