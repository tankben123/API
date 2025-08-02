using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Text.Json;

namespace WebhookClient
{
    public partial class Form1 : Form
    {
        private HubConnection _hubConnection;
        private NotifyIcon _notifyIcon;
        private string _latestUrl = "";
        private System.Windows.Forms.Timer _keepAliveTimer;

        public Form1()
        {
            _ = InitializeComponent();
            SetupNotifyIcon();
            KeyPreview = true;
            _keepAliveTimer = new System.Windows.Forms.Timer();
            _keepAliveTimer.Interval = 300000; // 5 phút = 300,000 ms
            _keepAliveTimer.Tick += async (s, e) =>
            {
                try
                {
                    using var client = new HttpClient();
                    var response = await client.GetAsync("https://sheet-api-mega.onrender.com/ping");
                    AppendLog("INFO", $"Ping server: {response.StatusCode}");
                }
                catch (Exception ex)
                {
                    AppendLog("ERROR", $"Ping lỗi: {ex.Message}");
                }
            };
            _keepAliveTimer.Start();


        }

        private void SetupNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = SystemIcons.Information;
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Google Sheet Notifier";

            _notifyIcon.BalloonTipClicked += async (s, e) =>
            {
                AppendLog("INFO", "🔔 BalloonTipClicked event triggered.");
                if (!string.IsNullOrEmpty(_latestUrl))
                {
                    await OpenUrlInBrowser(_latestUrl);
                }
            };

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Mở Google Sheet", null, async (s, e) =>
            {
                if (!string.IsNullOrEmpty(_latestUrl))
                {
                    await OpenUrlInBrowser(_latestUrl);
                }
            });
            contextMenu.Items.Add("Mở màn hình chính", null, (s, e) =>
            {
                this.Show(); // 👈 BẮT BUỘC để hiện Form lại
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            });
            contextMenu.Items.Add("Thoát", null, (s, e) => Application.Exit());
            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        private void AppendLog(string logType, string message)
        {
            string timestamp = DateTime.Now.ToString("MMM dd hh:mm:ss tt"); // Format: Jul 29 01:42:48 PM
            string formattedLog = $"{timestamp} [{logType}] {message}{Environment.NewLine}";
            textBoxLogs.AppendText(formattedLog);
        }

        private async Task ConnectToSignalR(string sheetId)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://sheet-api-mega.onrender.com/hubs/sheetChange")
                .WithAutomaticReconnect(new ImmediateRetryPolicy()) // Custom reconnection policy
                .Build();

            _hubConnection.On<object>("ReceiveSheetChange", (data) =>
            {
                Invoke(() =>
                {
                    try
                    {
                        var options = new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        };

                        var json = JsonSerializer.Serialize(data, options);
                        using var doc = JsonDocument.Parse(json);
                        var root = doc.RootElement;
                        string sheetName = root.GetProperty("sheetName").GetString() ?? "";
                        string range = root.GetProperty("range").GetString() ?? "";
                        string newValue = root.GetProperty("newValue").GetString() ?? "";

                        AppendLog("INFO", $"📥 Thay đổi tại Sheet: {sheetName}, Ô: {range}, Giá trị mới: {newValue}");

                        string message = $"Thay đổi tại Sheet: {sheetName} | Ô: {range}";
                        _latestUrl = root.GetProperty("url").GetString() ?? "";

                        ShowNotification("📌 Google Sheet thay đổi", message);
                    }
                    catch (Exception ex)
                    {
                        AppendLog("ERROR", $"❌ Lỗi khi xử lý dữ liệu: {ex.Message}");
                    }
                });
            });

            _hubConnection.Reconnecting += async (error) =>
            {
                Invoke(() =>
                {
                    AppendLog("WARNING", "⚠️ Đang cố gắng kết nối lại...");
                });
            };

            _hubConnection.Reconnected += async (connectionId) =>
            {
                Invoke(() =>
                {
                    AppendLog("SUCCESS", "✅ Đã kết nối lại thành công.");
                });

                try
                {
                    await _hubConnection.InvokeAsync("JoinFileGroup", sheetId);
                    AppendLog("INFO", "🔄 Đã gán lại Group ID sau khi kết nối lại.");
                }
                catch (Exception ex)
                {
                    AppendLog("ERROR", $"❌ Lỗi khi gán lại Group ID: {ex.Message}");
                }
            };

            _hubConnection.Closed += async (error) =>
            {
                Invoke(() =>
                {
                    AppendLog("ERROR", "❌ Kết nối đã bị đóng. Đang cố gắng kết nối lại...");
                });

                await ConnectToSignalR(sheetId);
            };

            try
            {
                await _hubConnection.StartAsync();
                AppendLog("SUCCESS", "✅ Đã kết nối tới server.");

                await _hubConnection.InvokeAsync("JoinFileGroup", sheetId);
            }
            catch (Exception ex)
            {
                AppendLog("ERROR", $"❌ Không kết nối được: {ex.Message}");
            }
        }

        private void ShowNotification(string title, string message)
        {
            _notifyIcon.BalloonTipTitle = title;
            _notifyIcon.BalloonTipText = message;
            _notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            _notifyIcon.ShowBalloonTip(5000);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string SheetId = textBoxSheetId.Text.Trim();
            if (string.IsNullOrEmpty(SheetId))
            {
                AppendLog("ERROR", "❌ Vui lòng nhập ID Sheet.");
                return;
            }
            _ = ConnectToSignalR(SheetId);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized; // Minimize the form
            this.ShowInTaskbar = false; // Remove the form from the taskbar
            this.Visible = false; // Hide the form
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Ẩn form thay vì đóng app
                e.Cancel = true;
                this.Hide(); // Hoặc Minimize
            }
            base.OnFormClosing(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            // Check for the specific key combination (e.g., Ctrl + Shift + L)
            if (e.Control && e.Shift && e.KeyCode == Keys.L)
            {
                textBoxLogs.Clear(); // Clear the logs
                AppendLog("INFO", "🧹 Logs đã được xóa.");
            }
        }

        private async Task OpenUrlInBrowser(string url)
        {
            try
            {
                await Task.Run(() =>
                {
                    var process = Process.Start(new ProcessStartInfo
                    {
                        FileName = "chrome",
                        Arguments = url,
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Normal
                    });

                    if (process == null)
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true,
                            WindowStyle = ProcessWindowStyle.Normal
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                AppendLog("ERROR", $"❌ Error opening browser: {ex.Message}");
            }
        }
    }

    public class ImmediateRetryPolicy : IRetryPolicy
    {
        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            // Always return zero delay for immediate reconnection
            return TimeSpan.Zero;
        }
    }
}
