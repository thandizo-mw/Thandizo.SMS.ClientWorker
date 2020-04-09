﻿using Autofac;
using GreenPipes;
using MassTransit;
using MassTransit.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using Thandizo.SMS.ClientWorker.Consumers;
using Thandizo.SMS.ClientWorker.Modules;

namespace Thandizo.SMS.ClientWorker
{
    class Program
    {
        private static IConfigurationRoot _configuration;
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration = configBuilder.Build();

            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.ColoredConsole()
                .CreateLogger();
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddSerilog(logger);
            LogContext.ConfigureCurrentLogContext(loggerFactory);
            var smsBaseUrl = _configuration["BaseUrl"];
            var smsSender = _configuration["SmsSender"];

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ServiceModule(smsBaseUrl, smsSender));
            builder.RegisterModule<ConsumersModule>();
            builder.Register(context =>
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.ConfigureJsonSerializer(options =>
                    {
                        options.DefaultValueHandling = DefaultValueHandling.Include;
                        return options;
                    });
                    var host = config.Host(new Uri(_configuration.GetValue<string>("RabbitMQHost")), h =>
                    {
                        h.Username(_configuration.GetValue<string>("RabbitMQUsername"));
                        h.Password(_configuration.GetValue<string>("RabbitMQPassword"));
                    });
                    config.PrefetchCount = 32;
                    config.UseRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(6)));
                    config.ReceiveEndpoint(_configuration["SmsQueue"], e =>
                    {
                        e.Consumer<SmsConsumer>(context);
                    });
                });
                return busControl;
            })
            .SingleInstance()
            .As<IBusControl>()
            .As<IBus>();
            LogContext.Info?.Log("Creating Service bus..");
            var container = builder.Build();
            var bus = container.Resolve<IBusControl>();
            bus.Start();
            LogContext.Info?.Log("Bus started");
            LogContext.Info?.Log("Waiting for request");
            Console.ReadLine();
            bus.Stop();
        }
    }
}
