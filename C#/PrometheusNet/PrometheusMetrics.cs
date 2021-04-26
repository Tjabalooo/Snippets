using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApp.PrometheusNet
{
    public partial class PrometheusMetrics : IPrometheusMetrics
    {
        private const string metricNamespace = "exampleapp";

        private static string GenerateMetricName(string metricNamespace, string metricName)
        {
            return $"{metricNamespace.ToLower()}_{string.Join('_', metricName.ToLower().Split(' '))}";
        }

        public ICounters Counters { get; }
        public IGauges Gauges { get; }
        public IHistograms Histograms { get; }
        public ISummaries Summaries { get; }

        public PrometheusMetrics()
        {
            Counters = new CounterMetrics();
            Gauges = new GaugeMetrics();
            Histograms = new HistogramMetrics();
            Summaries = new SummaryMetrics();
        }
    }
}
