using System;
using Chaldea.Core.Repositories;
using Chaldea.Core.Sms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chaldea.Infrastructure.Sms.Aliyun
{
    public static class Extensions
    {
        public static IServiceCollection AddSms(
            this IServiceCollection services,
            Action<SmsOptions> action)
        {
            var option = new SmsOptions();
            action?.Invoke(option);
            if (option.Config != null) services.Configure<DataProviderSettings>(option.Config);
            services.AddTransient<ISmsService, SmsService>();
            return services;
        }
    }

    public class SmsOptions
    {
        public IConfigurationSection Config { get; set; }
    }
}