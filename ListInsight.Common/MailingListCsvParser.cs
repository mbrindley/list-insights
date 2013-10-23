using System;
using System.Collections.Generic;
using System.IO;

namespace ListInsight.Common
{
    public class MailingListCsvParser : IDisposable
    {
        private readonly Stream fileStream;
        private readonly StreamReader streamReader;

        private static readonly char[] TrimChars = new[]{'\'', '"'};

        public MailingListCsvParser(Stream fileStream)
        {
            this.fileStream = fileStream;
            streamReader = new StreamReader(fileStream);
        }

        public IEnumerable<string> GetEmailAddressesBatch(int batchSize)
        {
            // Read the next batchSize lines out
            for (int i = 0; i < batchSize; i++)
            {
                string address;
                try
                {
                    var line = streamReader.ReadLine();
                    if (line == null)
                        break;
                    address = GetEmailAddressFromLine(line);
                }
                catch (ArgumentException)
                {
                    continue;
                }
                yield return address;
            }
        }

        public void Dispose()
        {
            streamReader.Dispose();
            fileStream.Dispose();
        }

        private string GetEmailAddressFromLine(string line)
        {
            var splitChar = line.Contains("\t") ? '\t' : ',';
            var parts = line.Split(splitChar);
            if (parts.Length > 1)
            {
                // Treat as tab separated
                foreach (var part in parts)
                {
                    if (part.Contains("@"))
                        return CleanEmailAddress(part);
                }
                // If we get here, there's no valid email address in any parts of the line
                throw new ArgumentException("Unable to find an email address on this line");
            }
            return CleanEmailAddress(line);
        }

        private string CleanEmailAddress(string line)
        {
            return line.TrimEnd(TrimChars).TrimStart(TrimChars);
        }
    }
}