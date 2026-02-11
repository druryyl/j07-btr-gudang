using BtrGudang.Helper.Common;
using BtrGudang.Winform.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackingOrderDownloaderApp
{
    public partial class DownloadPackingOrderForm : Form
    {
        private readonly PackingOrderDownloaderSvc _downloader;
        private readonly Timer _timer;
        private bool _isExecuting = false;
        private readonly object _executionLock = new object();

        public DownloadPackingOrderForm()
        {
            InitializeComponent();

            // Initialize service
            _downloader = new PackingOrderDownloaderSvc();

            // Setup timer
            _timer = new Timer
            {
                Interval = 300000 // 5 minutes in milliseconds
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();

            // Start the clock
            UpdateClock();
            var clockTimer = new Timer { Interval = 1000 };
            clockTimer.Tick += (s, e) => UpdateClock();
            clockTimer.Start();
        }

        private void UpdateClock()
        {
            lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            await ExecuteDownloadAsync();
        }

        private async void btnDownloadNow_Click(object sender, EventArgs e)
        {
            await ExecuteDownloadAsync();
        }

        private async Task ExecuteDownloadAsync()
        {
            // Prevent overlapping executions
            lock (_executionLock)
            {
                if (_isExecuting)
                    return;
                _isExecuting = true;
            }

            try
            {
                var currentTime = DateTime.Now;
                AppendLog($"[{currentTime:HH:mm:ss}] Executing download...");

                var periode = new Periode(DateTime.Today); // Current date as required

                var result = await _downloader.Execute(periode);

                var success = result.Item1;
                var message = result.Item2;
                var orders = result.Item3;

                if (success)
                {
                    AppendLog("[SUCCESS]");

                    if (orders != null)
                    {
                        foreach (var order in orders)
                        {
                            AppendLog(order.ToString()); // Assuming meaningful ToString() override
                        }
                    }

                    if (!string.IsNullOrEmpty(message))
                    {
                        var lines = message.Split(new[] { '\r', '\n' },
                                                  StringSplitOptions.RemoveEmptyEntries);
                        foreach (var line in lines)
                        {
                            AppendLog(line);
                        }
                    }
                }
                else
                {
                    AppendLog("[FAILED]");
                    AppendLog(message ?? "Unknown error occurred");
                }

                var nextExecution = DateTime.Now.AddMinutes(5);
                AppendLog($"Next execution at: {nextExecution:HH:mm:ss}");
                AppendLog(new string('-', 50));
            }
            catch (Exception ex)
            {
                AppendLog($"[ERROR] {ex.Message}");
            }
            finally
            {
                lock (_executionLock)
                {
                    _isExecuting = false;
                }
            }
        }

        private void AppendLog(string text)
        {
            if (rtbLogs.InvokeRequired)
            {
                rtbLogs.Invoke(new Action<string>(AppendLog), text);
                return;
            }

            rtbLogs.AppendText($"[{DateTime.Now:HH:mm:ss}] {text}\n");
            rtbLogs.ScrollToCaret();
        }

        // Replace the explicit Dispose(bool) override (which caused a duplicate with the designer partial)
        // by disposing timers when the form is closed. This avoids CS0111 while ensuring resources are released.
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                _timer?.Stop();
                _timer?.Dispose();
            }
            catch
            {
                // Swallow exceptions during shutdown to avoid interfering with form closing.
            }

            base.OnFormClosed(e);
        }
    }

    // Supporting classes
    //public class PackingOrderDownloaderSvc
    //{
    //    public async Task<(bool, string, List<PackingOrderModel>)> Execute(Periode periode)
    //    {
    //        // Simulate async work
    //        await Task.Delay(1000);

    //        // For demo purposes - replace with actual implementation
    //        return (true,
    //               "Download completed successfully",
    //               new List<PackingOrderModel>
    //               {
    //                   new PackingOrderModel { DocumentId = "PO001", CustomerName = "Customer A" },
    //                   new PackingOrderModel { DocumentId = "PO002", CustomerName = "Customer B" }
    //               });
    //    }
    //}

    //public class Periode
    //{
    //    public Periode(DateTime tgl)
    //    {
    //        Tgl1 = tgl.Date;
    //        Tgl2 = tgl.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
    //    }

    //    public Periode(DateTime tgl1, DateTime tgl2)
    //    {
    //        Tgl1 = tgl1.Date;
    //        Tgl2 = tgl2.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
    //    }

    //    public DateTime Tgl1 { get; }
    //    public DateTime Tgl2 { get; }
    //}

    //public class PackingOrderModel
    //{
    //    public string DocumentId { get; set; }
    //    public string CustomerName { get; set; }

    //    public override string ToString()
    //    {
    //        return $"{DocumentId} - {CustomerName}";
    //    }
    //}
}