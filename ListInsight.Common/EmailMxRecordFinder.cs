using System;
using System.ComponentModel;
using System.Linq;

namespace ListInsight.Common
{
    public class EmailMxRecordFinder
    {
        private readonly IDnsMx dnsMx;

        public EmailMxRecordFinder(IDnsMx dnsMx)
        {
            this.dnsMx = dnsMx;
        }

        public string[] Find(string emailAddress)
        {
            if (!emailAddress.Contains("@"))
                throw new ArgumentException(string.Format("{0} is not a valid email address", emailAddress));

            var parts = emailAddress.Split('@');
            var domain = parts.Last();

            return LookupMxRecords(domain);
        }

        private string[] LookupMxRecords(string domain)
        {
            try
            {
                return dnsMx.GetMxRecords(domain);
            }
            catch (Win32Exception)
            {
                return new string[0];
            }
        }
    }
}