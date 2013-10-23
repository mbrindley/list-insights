using System;
using System.Diagnostics;
using System.Linq;

namespace ListInsight.Common
{
    public class GmailIdentifier
    {
        private readonly EmailMxRecordFinder emailMxRecordFinder;

        public GmailIdentifier(EmailMxRecordFinder emailMxRecordFinder)
        {
            this.emailMxRecordFinder = emailMxRecordFinder;
        }

        /// <summary>
        /// Identifies if this is a Google email address and indicates if it mentions Gmail in the address specifically
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public AddressType Identify(string address)
        {
            // If the address is a regular @gmail. or @googlemail. address
            if (FastContains(address, "@gmail.") || FastContains(address, "@googlemail."))
                return AddressType.GmailAddress;    // Don't even bother checking MX records.

            // Check MX records for mentions Google's mail servers
            var mxRecords = emailMxRecordFinder.Find(address);
            if (mxRecords.Any(m => FastContains(m, ".google.com") || FastContains(m, ".googlemail.com")))
                return AddressType.GoogleMx;

            // It's nothing Google related
            return AddressType.NonGoogle;
        }

        private bool FastContains(string text, string search)
        {
            return text.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}