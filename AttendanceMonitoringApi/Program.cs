using AttendanceMonitoring;
using AttendanceMonitoringApi.Services;
using Microsoft.EntityFrameworkCore;

namespace AttendanceMonitoringApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenAnyIP(5000);
                //serverOptions.ListenAnyIP(5001, listenOptions =>
                //{
                //    listenOptions.UseHttps();
                //});
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            builder.Services.AddHostedService<QueuedProcessorBackgroundService>();
            builder.Services.AddScoped<IAttendanceService, AttendanceService>();
            builder.Services.AddLogging();

            // Connection to database
            builder.Services.AddDbContext<AttendanceMonitoringContext>(option =>
                option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
