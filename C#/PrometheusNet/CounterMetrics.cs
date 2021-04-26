using Prometheus;

namespace ExampleApp.PrometheusNet
{
    public partial class PrometheusMetrics
    {
        private class CounterMetrics : ICounters
        {
            public Counter DummyCounter { get; }

            public CounterMetrics()
            {
                DummyCounter = Metrics.CreateCounter(
                    GenerateMetricName(metricNamespace, "dummy counter"),
                    "dummy counter");
            }
        }
    }
}
