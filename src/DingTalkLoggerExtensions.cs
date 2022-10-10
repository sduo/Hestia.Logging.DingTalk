using Hestia.Logging.DingTalk.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System;

namespace Hestia.Logging.DingTalk
{
    public static class DingTalkLoggerExtensions
    {
        public static ILoggingBuilder AddDingTalk(this ILoggingBuilder builder,Func<IConfiguration, IFormatter> formatter = null)
        {
            builder.AddConfiguration();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DingTalkLoggerProvider>((services)=> new DingTalkLoggerProvider(services,formatter)));
            return builder;
        }
    }
}
