namespace AttendanceMonitoringApi.Services
{
    public class QueuedProcessorBackgroundService : BackgroundService
    {
        //private readonly IServiceScopeFactory _serviceProvider;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger _logger;
        public QueuedProcessorBackgroundService(
            IBackgroundTaskQueue taskQueue,
            ILoggerFactory loggerFactory)
        {
            _taskQueue = taskQueue;
            //_serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<QueuedProcessorBackgroundService>();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Processor Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await _taskQueue.DequeueAsync(stoppingToken);
                try
                {
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing background work item.");
                }
            }
            _logger.LogInformation("Queued Processor Background Service is stopping.");
        }
    }
}
