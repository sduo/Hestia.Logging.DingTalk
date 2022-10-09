using System;
using System.Collections.Generic;
using Hestia.Logging.Abstractions;
using System.Net.Http.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text;

namespace Hestia.Logging.DingTalk.Formatters
{
    public sealed class MarkdownFormatter : IFormatter
    {
        private readonly IConfiguration configuration;

        public const string Separator = "----";

        public MarkdownFormatter(IConfiguration configuration)
        {
            this.configuration = configuration.GetSection("Markdown");
        }

        public HttpContent Format(Log log)
        {
            var messages = new List<string>(){
                configuration.GetValue($"Image:{log.Level}", $"# {log.Level}"),
                Separator,
                $"机器：{Environment.MachineName}",
                $"路径：{Environment.ProcessPath}",
                $"进程：{Environment.ProcessId}",                
                $"模块：{log.Category}"                
            };

            StringBuilder @event = new StringBuilder($"事件：{log.Event.Id}");
            if (!string.IsNullOrEmpty(log.Event.Name))
            {
                @event.Append($"({log.Event.Name})");
            }
            messages.Add(@event.ToString());

            if (log.Scopes.Count > 0)
            {
                messages.Add(Separator);
                messages.AddRange(log.Scopes.Select(x => x.ToString()));
            }
            messages.Add(Separator);
            messages.Add(log.Message);

            if(log.Exception is not null)
            {
                messages.Add(Separator);
                messages.Add($"**{log.Exception.Message}**");
                if(log.Exception.InnerException is not null)
                {
                    messages.Add($"*{log.Exception.GetBaseException().Message}*");
                }                
                if (configuration.GetValue($"Stack", true) && !string.IsNullOrEmpty(log.Exception.StackTrace))
                {
                    messages.Add($"> {log.Exception.StackTrace}");
                }
            }


            return JsonContent.Create(new
            {
                msgtype = "markdown",
                markdown = new
                {
                    title = string.Concat(configuration.GetValue($"Icon:{log.Level}",$"[{log.Level}]"),log.Category),
                    text = string.Join("\n\n", messages)
                }
            });
        }
    }
}
