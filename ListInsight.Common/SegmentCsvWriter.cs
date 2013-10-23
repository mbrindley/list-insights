using System;
using System.Collections.Generic;
using System.IO;

namespace ListInsight.Common
{
    public class SegmentCsvWriter : IDisposable
    {
        private readonly Stream stream;
        private readonly StreamWriter streamWriter;

        private readonly object writeLineSync = new object();

        public SegmentCsvWriter(Stream stream)
        {
            this.stream = stream;
            streamWriter = new StreamWriter(stream);
        }

        public void WriteEmailAddress(string email)
        {
            lock (writeLineSync)
                streamWriter.WriteLine(email.Trim());
        }

        public void Dispose()
        {
            streamWriter.Dispose();
            stream.Dispose();
        }
    }
}