using System.Linq;
using System.Net;
using System.Net.Sockets;
using DnDns.Enums;
using DnDns.Query;
using DnDns.Records;

namespace ListInsight.Common
{
    public class MacDnsMx : IDnsMx
    {
        public string[] GetMxRecords(string domain)
        {
            var request = new DnsQueryRequest();
            var response = request.Resolve("8.8.8.8", domain, NsType.MX, NsClass.INET, ProtocolType.Udp);
            return response.Answers.Where(a => a is MxRecord).OrderBy(a => (a as MxRecord).Preference).Select(a => (a as MxRecord).MailExchange).ToArray();
        }
    }
}