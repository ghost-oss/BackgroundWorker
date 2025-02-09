using BackgroundWorker;
using BackgroundWorker.Abstractions;
using BackgroundWorker.Options;
using BackgroundWorker.Services;
using AWSSDK;
using Amazon.SQS;
using BackgroundWorker.AWSExtentions;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configurationBuilder =>
    {
        configurationBuilder.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<SQSBackgroundWorker>();

        services.AddSingleton<ISQSService, SQSService>();

        //services.Configure<SQSOptions>(context.Configuration.GetSection("SqsOptions"));

        services.ConfigureAWSServices(context.Configuration);
    })
    .Build();

host.Run();

