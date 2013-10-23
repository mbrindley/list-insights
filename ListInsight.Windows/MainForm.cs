using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ListInsight.Common;

namespace ListInsight.Windows
{
    public partial class MainForm : Form
    {
        private long gmailAddresesCount;
        private long googleMxAddresesCount;
        private long totalAddressesCount;
        private readonly EmailMxRecordFinder emailMxRecordFinder;
        private readonly GmailIdentifier gmailIdentifier;
        private SegmentCsvWriter writer;

        public MainForm()
        {
            InitializeComponent();
            MoveToState(State.Initial);
            // Expand the threadpool for this light (but high quantity) DNS bg work
            int workThreads, ioThreads;
            ThreadPool.GetMaxThreads(out workThreads, out ioThreads);
            ThreadPool.SetMaxThreads(workThreads > 10000 ? workThreads : 10000, ioThreads);
            // DI
            emailMxRecordFinder = new EmailMxRecordFinder(new WindowsDnsMx());
            gmailIdentifier = new GmailIdentifier(emailMxRecordFinder);
        }

        /// <summary>
        /// Change the UI into the desired state
        /// </summary>
        /// <param name="state"></param>
        private void MoveToState(State state)
        {
            switch (state)
            {
                case State.Initial:
                    infoLabel.Text = Properties.Resources.InitalStateText;
                    scanProgressBar.Visible = false;
                    openButton.Visible = true;
                    saveButton.Visible = false;
                    break;
                case State.PickSaveLocation:
                    infoLabel.Text = Properties.Resources.PickSaveLocationText;
                    scanProgressBar.Visible = false;
                    openButton.Visible = false;
                    saveButton.Visible = true;
                    break;
                case State.InProgress:
                    infoLabel.Text = Properties.Resources.ScanInProgressText;
                    scanProgressBar.Visible = true;
                    openButton.Visible = false;
                    saveButton.Visible = false;
                    break;
                case State.Complete:
                    infoLabel.Text = string.Format(Properties.Resources.CompleteText, Path.GetFileName(saveFileDialog.FileName));
                    scanProgressBar.Visible = true;
                    openButton.Visible = false;
                    saveButton.Visible = false;
                    break;
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();

            if (string.IsNullOrWhiteSpace(openFileDialog.FileName))
                return;

            MoveToState(State.PickSaveLocation);   
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(openFileDialog.FileName);
            saveFileDialog.ShowDialog();

            if (string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                return;

            MoveToState(State.InProgress);

            scanProgressBar.Maximum = File.ReadLines(openFileDialog.FileName).Count();

            new Thread(FindGmailAddresses).Start();
        }

        private void FindGmailAddresses()
        {
            try
            {
                writer = new SegmentCsvWriter(File.Create(saveFileDialog.FileName));
            }
            catch (IOException)
            {
                MessageBox.Show(string.Format("{0} is currently in use. Please close any applications currently using it and try again.", Path.GetFileName(saveFileDialog.FileName)), "Unable to access file");
                Invoke((MethodInvoker)(() => MoveToState(State.PickSaveLocation)));
                return;
            }

            try
            {
                using (var reader = new MailingListCsvParser(openFileDialog.OpenFile()))
                {
                    bool batchContainedItems;
                    do
                    {
                        var tasks = new List<Task>();

                        batchContainedItems = false;
                        foreach (var emailAddress in reader.GetEmailAddressesBatch(10000))
                        {
                            batchContainedItems = true;
                            string address = emailAddress;
                            tasks.Add(Task.Factory.StartNew(() => IsAddressGmail(address)));
                        }

                        Task.WaitAll(tasks.ToArray());
                        tasks.Clear();
                    } while (batchContainedItems);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(string.Format("{0} is currently in use. Please close any applications currently using it and try again.", Path.GetFileName(openFileDialog.FileName)), "Unable to access file");
                Invoke((MethodInvoker)(() => MoveToState(State.Initial)));
                return;
            }

            Invoke((MethodInvoker)Complete);
        }

        private string IsAddressGmail(string address)
        {
            var addressType = gmailIdentifier.Identify(address);

            // If it's anything to do with Google, inc the counter and write it out to the csv
            if (addressType == AddressType.GoogleMx || addressType == AddressType.GmailAddress)
            {
                Interlocked.Increment(ref googleMxAddresesCount);
                writer.WriteEmailAddress(address);
            }

            // Specifically count @gmail addresses as it's interesting
            if (addressType == AddressType.GmailAddress)
                Interlocked.Increment(ref gmailAddresesCount);
            
            Interlocked.Increment(ref totalAddressesCount);
            Invoke((MethodInvoker) delegate { scanProgressBar.Value++; });
            return address;
        }

        /// <summary>
        /// The work has completed. Finish up!
        /// </summary>
        private void Complete()
        {
            var total = Interlocked.Read(ref totalAddressesCount);
            var googleMx = Interlocked.Read(ref googleMxAddresesCount);
            var gmailAddress = Interlocked.Read(ref gmailAddresesCount);
            var percentage = googleMx*100 / total;
            var additional = googleMx - gmailAddress;
            MessageBox.Show(string.Format(Properties.Resources.CompletedAlertText, total, gmailAddress, additional, googleMx, percentage));

            MoveToState(State.Complete);

            // Close the file
            writer.Dispose();

            // Open Explorer and show their file
            Process.Start(Path.GetDirectoryName(saveFileDialog.FileName) ?? Directory.GetCurrentDirectory());
        }
    }
}
