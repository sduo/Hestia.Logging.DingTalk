using Hestia.Logging.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using Hestia.Logging.DingTalk.Formatters;
using System.Text;
using System.Web;
using System.Diagnostics;

namespace Hestia.Logging.DingTalk
{
    public sealed class DingTalkLoggerProvider : BatchingLoggerProvider
    {
        private readonly IFormatter formatter;

        private static Func<IConfiguration, IFormatter> CreateDefaultFormatter = config => new MarkdownFormatter(config);

        public DingTalkLoggerProvider(IServiceProvider services,Func<IConfiguration, IFormatter> formatter = null) : base(services)
        {
            this.formatter = (formatter ?? CreateDefaultFormatter).Invoke(configuration.GetSection($"{Prefix}:{Name}"));
        }
        public override string Name  => "DingTalk";

        protected override async Task WriteMessagesAsync(IEnumerable<Log> logs, CancellationToken token)
        {
            string url = configuration.GetValue<string>($"{Prefix}:{Name}:Url", null);
            if (string.IsNullOrEmpty(url)) { return; }

            foreach (Log log in logs)
            {
                if (token.IsCancellationRequested) { break; }

                var content = formatter?.Format(log);

                if (content is null) { continue; }                

                string secret = configuration.GetValue<string>($"{Prefix}:{Name}:Secret", null);
                if (!string.IsNullOrEmpty(secret))
                {
                    long ts = log.Timestamp.ToUnixTimeMilliseconds();
                    string source = string.Join('\n', ts, secret);
                    string signature = HttpUtility.UrlEncode(Convert.ToBase64String(Security.MAC.HMAC_SHA256(Encoding.UTF8.GetBytes(secret), Encoding.UTF8.GetBytes(source))));
                    url += $"&timestamp={ts}&sign={signature}";
                }

                HttpRequestMessage request = new(HttpMethod.Post, url) { Content = content };

                using HttpClient rpc = new();
                var response = await rpc.SendAsync(request, token);
                Trace.WriteLine($"[{nameof(DingTalkLoggerProvider)}]{url}:{response.StatusCode}");
            }
        }
    }
}