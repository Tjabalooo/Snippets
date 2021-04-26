using Prometheus;

namespace ExampleApp.PrometheusNet
{
    public partial class PrometheusMetrics
    {
        private class GaugeMetrics : IGauges
        {
            public GaugeMetrics()
            {
            }
        }
    }
}
