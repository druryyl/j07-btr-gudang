using BtrGudang.Helper.Common;
using BtrGudang.Winform.Services;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackingOrderDownloader
{
    /// <summary>
    /// Main form for the Packing Order Downloader application
    /// </summary>
    public partial class DownloadPackingOrder2Form : Form
    {
        // Timers
        private System.Windows.Forms.Timer _downloadTimer;
        private System.Windows.Forms.Timer _clockTimer;

        // Service and state management
        private readonly PackingOrderDownloaderSvc _downloadService;
        private DateTime _nextScheduledExecution;
        private int _isExecuting = 0; // Using int for Interlocked operations
        private const int DOWNLOAD_INTERVAL_MILLISECONDS = 5 * 60 * 1000; // 5 minutes

        public DownloadPackingOrder2Form()
        {
            _downloadService = new PackingOrderDownloaderSvc();
            InitializeComponent();
            InitializeTimers();
            ScheduleNextExecution();
            
            // Log initial message
            LogMessage("Application started successfully", LogLevel.Info);
            LogMessage($"Automatic download interval: {DOWNLOAD_INTERVAL_MILLISECONDS / 1000 / 60} minutes", LogLevel.Info);
        }

        /// <summary>
        /// Initialize and start timers
        /// </summary>
        private void InitializeTimers()
        {
            // Clock timer - updates every second
            _clockTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000 // 1 second
            };
            _clockTimer.Tick += ClockTimer_Tick;
            _clockTimer.Start();

            // Download timer - triggers every 5 minutes
            _downloadTimer = new System.Windows.Forms.Timer
            {
                Interval = DOWNLOAD_INTERVAL_MILLISECONDS
            };
            _downloadTimer.Tick += DownloadTimer_Tick;
            _downloadTimer.Start();
        }

        /// <summary>
        /// Clock timer tick event - updates the clock display
        /// </summary>
        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            _clockLabel.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// Download timer tick event - triggers automatic download
        /// </summary>
        private async void DownloadTimer_Tick(object sender, EventArgs e)
        {
            await ExecuteDownloadAsync(isManual: false);
        }

        /// <summary>
        /// Manual download button click event
        /// </summary>
        private async void ManualDownloadButton_Click(object sender, EventArgs e)
        {
            await ExecuteDownloadAsync(isManual: true);
        }

        /// <summary>
        /// Execute the download operation asynchronously
        /// </summary>
        /// <param name="isManual">Indicates if this is a manual execution</param>
        private async Task ExecuteDownloadAsync(bool isManual)
        {
            // Prevent overlapping executions using thread-safe approach
            if (Interlocked.CompareExchange(ref _isExecuting, 1, 0) == 1)
            {
                LogMessage("Download already in progress, skipping execution", LogLevel.Warning);
                return;
            }

            try
            {
                // Update UI state
                UpdateUIState(isExecuting: true);

                string executionType = isManual ? "Manual" : "Scheduled";
                LogMessage($"========== {executionType} Download Started ==========", LogLevel.Info);
                LogMessage($"Execution Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}", LogLevel.Info);

                // Create period for today
                var periode = new Periode(DateTime.Now);
                LogMessage($"Period: {periode.Tgl1:yyyy-MM-dd HH:mm:ss} to {periode.Tgl2:yyyy-MM-dd HH:mm:ss}", LogLevel.Info);

                // Execute the service call
                var (success, message, orders) = await _downloadService.Execute(periode);

                // Log results
                if (success)
                {
                    LogMessage($"[SUCCESS] {message}", LogLevel.Success);
                    LogMessage($"Records retrieved: {orders.Count}", LogLevel.Info);
                }
                else
                {
                    LogMessage($"[ERROR] {message}", LogLevel.Error);
                }

                // Schedule next execution (only for non-manual)
                if (!isManual)
                {
                    ScheduleNextExecution();
                }
                
                LogMessage($"Next scheduled execution: {_nextScheduledExecution:HH:mm:ss}", LogLevel.Info);
                LogMessage($"========== {executionType} Download Completed ==========\n", LogLevel.Info);
            }
            catch (Exception ex)
            {
                LogMessage($"[EXCEPTION] Unhandled error: {ex.Message}", LogLevel.Error);
                LogMessage($"Stack Trace: {ex.StackTrace}", LogLevel.Error);
            }
            finally
            {
                // Release execution lock
                Interlocked.Exchange(ref _isExecuting, 0);
                UpdateUIState(isExecuting: false);
            }
        }

        /// <summary>
        /// Schedule the next execution time
        /// </summary>
        private void ScheduleNextExecution()
        {
            _nextScheduledExecution = DateTime.Now.AddMilliseconds(DOWNLOAD_INTERVAL_MILLISECONDS);
        }

        /// <summary>
        /// Update UI elements based on execution state
        /// </summary>
        /// <param name="isExecuting">Current execution state</param>
        private void UpdateUIState(bool isExecuting)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<bool>(UpdateUIState), isExecuting);
                return;
            }

            _manualDownloadButton.Enabled = !isExecuting;
            _manualDownloadButton.Text = isExecuting ? "Downloading..." : "Download Now";
            _statusLabel.Text = isExecuting ? "Downloading data..." : "Ready";
        }

        /// <summary>
        /// Log levels for message categorization
        /// </summary>
        private enum LogLevel
        {
            Info,
            Success,
            Warning,
            Error
        }

        /// <summary>
        /// Log a message to the RichTextBox with color coding
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="level">Log level</param>
        private void LogMessage(string message, LogLevel level)
        {
            if (_logTextBox.InvokeRequired)
            {
                _logTextBox.Invoke(new Action<string, LogLevel>(LogMessage), message, level);
                return;
            }

            // Determine color based on log level
            Color color;
            switch (level)
            {
                case LogLevel.Success:
                    color = Color.LimeGreen;
                    break;
                case LogLevel.Warning:
                    color = Color.Orange;
                    break;
                case LogLevel.Error:
                    color = Color.Red;
                    break;
                case LogLevel.Info:
                default:
                    color = Color.FromArgb(220, 220, 220);
                    break;
            }

            // Append message with color
            _logTextBox.SelectionStart = _logTextBox.TextLength;
            _logTextBox.SelectionLength = 0;
            _logTextBox.SelectionColor = color;
            _logTextBox.AppendText(message + Environment.NewLine);
            _logTextBox.SelectionColor = _logTextBox.ForeColor;

            // Auto-scroll to bottom
            _logTextBox.SelectionStart = _logTextBox.TextLength;
            _logTextBox.ScrollToCaret();
        }

        /// <summary>
        /// Clean up resources when form is closing
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _downloadTimer?.Stop();
            _downloadTimer?.Dispose();
            _clockTimer?.Stop();
            _clockTimer?.Dispose();

            LogMessage("Application shutting down...", LogLevel.Info);
            
            base.OnFormClosing(e);
        }
    }
}
