using Hestia.Logging.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace Hestia.Logging.DingTalk.Formatters
{
    public interface IFormatter
    {
        public IConfiguration Configuration { get; init; }

        public string Name { get; }

        public HttpContent Format(Log log);
    }
}
