using Hestia.Logging.DingTalk.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System;
using System.Collections.Generic;

namespace Hestia.Logging.DingTalk
{
    public static class DingTalkLoggerExtensions
    {
        public static ILoggingBuilder AddDingTalkMarkdown(this ILoggingBuilder builder, Action<List<string>> with = null) => AddDingTalk(builder,(configuration)=> { return new MarkdownFormatter(configuration, with); });

        public static ILoggingBuilder AddDingTalk(this ILoggingBuilder builder,Func<IConfiguration, IFormatter> formatter = null)
        {
            builder.AddConfiguration();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DingTalkLoggerProvider>((services)=> new DingTalkLoggerProvider(services,formatter)));
            return builder;
        }
    }
}
