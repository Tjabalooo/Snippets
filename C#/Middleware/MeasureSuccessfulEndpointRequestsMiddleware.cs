using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApp.PrometheusNet.Middleware
{
    public class MeasureSuccessfulEndpointRequestsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IPrometheusMetrics _metrics;

        public MeasureSuccessfulEndpointRequestsMiddleware(RequestDelegate next, IPrometheusMetrics metrics)
        {
            _next = next;
            _metrics = metrics;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);

            try
            {
                if (context.Response.StatusCode == 200 && context.Request.Method == "GET")
                {
                    var endpointPath = string.Concat(context.Request.Path, context.Request.QueryString);
                    _metrics.Counters.CallsToEndpointsCounter.WithLabels(endpointPath).Inc();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in '{nameof(MeasureSuccessfulEndpointRequestsMiddleware)}'");
                Console.WriteLine($"Message: {ex.Message}");
            }
        }
    }
}
