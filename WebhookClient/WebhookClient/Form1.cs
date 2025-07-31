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

        public Form1()
        {
            _ = InitializeComponent();
            SetupNotifyIcon();
        }

        private void SetupNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = SystemIcons.Information;
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Google Sheet Notifier";

            _notifyIcon.BalloonTipClicked += (s, e) =>
            {
                AppendLog("INFO", "🔔 BalloonTipClicked event triggered.");
                if (!string.IsNullOrEmpty(_latestUrl))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "chrome",
                            Arguments = _latestUrl,
                            UseShellExecute = true,
                            WindowStyle = ProcessWindowStyle.Normal // Ensure the window is not minimized
                        });
                    }
                    catch
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = _latestUrl,
                            UseShellExecute = true,
                            WindowStyle = ProcessWindowStyle.Normal // Ensure the window is not minimized
                        });
                    }
                }
            };

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Mở Google Sheet", null, (s, e) =>
            {
                if (!string.IsNullOrEmpty(_latestUrl))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "chrome",
                            Arguments = _latestUrl,
                            UseShellExecute = true,
                            WindowStyle = ProcessWindowStyle.Normal // Ensure the window is not minimized
                        });
                    }
                    catch
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = _latestUrl,
                            UseShellExecute = true,
                            WindowStyle = ProcessWindowStyle.Normal // Ensure the window is not minimized
                        });
                    }
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
                        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
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
            ConnectToSignalR(SheetId);
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
