using Hestia.Logging.Abstractions;
using System.Net.Http;

namespace Hestia.Logging.DingTalk.Formatters
{
    public interface IFormatter
    {
        public HttpContent Format(Log log);
    }
}
