namespace ExampleApp.PrometheusNet
{
    public interface IPrometheusMetrics
    {
        public ICounters Counters { get; }
        public IGauges Gauges{ get; }
        public IHistograms Histograms{ get; }
        public ISummaries Summaries { get; }
    }
}