using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Text.Json;
using System.Runtime.InteropServices;

namespace WebhookClient
{
    public partial class Form1 : Form
    {
        private HubConnection _hubConnection;
        private NotifyIcon _notifyIcon;
        private string _latestUrl = "";
        private System.Windows.Forms.Timer _keepAliveTimer;


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (this.ClientRectangle.Width > 0 && this.ClientRectangle.Height > 0)
            {
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(this.ClientRectangle,
                    Color.White, Color.FromArgb(230, 240, 250), 90F))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            }
            else
            {
                base.OnPaintBackground(e); // Fallback to default behavior if dimensions are invalid
            }
        }

        public Form1()
        {
            _ = InitializeComponent();

            buttonJoin.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, buttonJoin.Width, buttonJoin.Height, 8, 8));
            buttonExit.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, buttonExit.Width, buttonExit.Height, 8, 8));
            //textBoxSheetId.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, textBoxSheetId.Width, textBoxSheetId.Height, 6, 6));
            //textBoxLogs.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, textBoxLogs.Width, textBoxLogs.Height, 6, 6));


            buttonJoin.BackColor = Color.FromArgb(46, 204, 113); // xanh lá tươi
            buttonJoin.ForeColor = Color.White;
            buttonJoin.FlatStyle = FlatStyle.Flat;
            buttonJoin.FlatAppearance.BorderSize = 0;

            buttonExit.BackColor = Color.FromArgb(231, 76, 60); // đỏ
            buttonExit.ForeColor = Color.White;
            buttonExit.FlatStyle = FlatStyle.Flat;
            buttonExit.FlatAppearance.BorderSize = 0;


            textBoxSheetId.TextChanged += (s, e) =>
            {
                Properties.Settings.Default.SheetId = textBoxSheetId.Text; // Save value on change
                Properties.Settings.Default.Save();
            };

            SetupNotifyIcon();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();
            using var stream = assembly.GetManifestResourceStream("WebhookClient.Icon.favicon.ico");
            if (stream != null)
            {
                this.Icon = new Icon(stream);
            }
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

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("WebhookClient.Icon.favicon.ico");
            if (stream != null)
            {
                _notifyIcon.Icon = new Icon(stream);
            }
            

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
                else
                {
                    // Use a default Google Sheet URL with a predefined sheet ID
                    string defaultSheetId = textBoxSheetId.Text.Trim(); // Replace with your actual sheet ID
                    string defaultUrl = $"https://docs.google.com/spreadsheets/d/{defaultSheetId}/edit";
                    await OpenUrlInBrowser(defaultUrl);
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
            Color color = Color.Black;

            switch (logType.ToUpper())
            {
                case "SUCCESS":
                    color = Color.FromArgb(76, 175, 80); // xanh lá
                    break;
                case "INFO":
                    color = Color.FromArgb(33, 150, 243); // xanh dương
                    break;
                case "ERROR":
                    color = Color.FromArgb(244, 67, 54); // đỏ
                    break;
            }

            textBoxLogs.SelectionStart = textBoxLogs.TextLength;
            textBoxLogs.SelectionLength = 0;
            textBoxLogs.SelectionColor = color;

            string timestamp = DateTime.Now.ToString("MMM dd hh:mm:ss tt"); // Format: Jul 29 01:42:48 PM
            string formattedLog = $"{timestamp} [{logType}] {message}{Environment.NewLine}";
            textBoxLogs.AppendText(formattedLog);
            textBoxLogs.SelectionColor = textBoxLogs.ForeColor;
            textBoxLogs.ScrollToCaret();
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
                        string user = root.GetProperty("user").GetString() ?? "";

                        AppendLog("INFO", $"📥 Thay đổi tại Sheet: {sheetName} - {user.Split('@')[0]}, Ô: {range}, Giá trị mới: {newValue}");

                        string message = $"Thay đổi tại Sheet: {sheetName} - {user.Split('@')[0]} | Ô: {range}";
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
                    UpdateConnectionStatus(false);
                    AppendLog("WARNING", "⚠️ Đang cố gắng kết nối lại...");
                });
            };

            _hubConnection.Reconnected += async (connectionId) =>
            {
                Invoke(() =>
                {
                    AppendLog("SUCCESS", "✅ Đã kết nối lại thành công.");
                    UpdateConnectionStatus(true);
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
                    UpdateConnectionStatus(true);
                    AppendLog("ERROR", "❌ Kết nối đã bị đóng. Đang cố gắng kết nối lại...");
                });

                await ConnectToSignalR(sheetId);
            };

            try
            {

                await _hubConnection.StartAsync();
                //AppendLog("SUCCESS", "✅ Đã kết nối tới server.");

                await _hubConnection.InvokeAsync("JoinFileGroup", sheetId);
                UpdateConnectionStatus(true);
            }
            catch (Exception ex)
            {
                AppendLog("ERROR", $"❌ Không kết nối được: {ex.Message}");
                UpdateConnectionStatus(false);
            }
        }

        private void ShowNotification(string title, string message)
        {
            _notifyIcon.BalloonTipTitle = title;
            _notifyIcon.BalloonTipText = message;
            _notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            _notifyIcon.ShowBalloonTip(5000);
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
                    // Path to Chrome's user data directory
                    string chromeUserDataPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Google",
                        "Chrome",
                        "User Data"
                    );

                    // Path to the "Local State" file
                    string localStateFilePath = Path.Combine(chromeUserDataPath, "Local State");

                    bool profileUsed = false;

                    if (File.Exists(localStateFilePath))
                    {
                        try
                        {
                            // Read and parse the "Local State" file
                            string localStateJson = File.ReadAllText(localStateFilePath);
                            using var doc = JsonDocument.Parse(localStateJson);
                            var root = doc.RootElement;

                            // Extract profile information
                            var profileInfo = root.GetProperty("profile").GetProperty("info_cache");
                            foreach (var profile in profileInfo.EnumerateObject())
                            {
                                string profileName = profile.Name; // Profile directory name (e.g., "Default", "Profile 1")

                                try
                                {
                                    // Attempt to open the URL with the first valid profile
                                    var process = Process.Start(new ProcessStartInfo
                                    {
                                        FileName = "chrome",
                                        Arguments = $"--profile-directory=\"{profileName}\" {url}",
                                        UseShellExecute = true,
                                        WindowStyle = ProcessWindowStyle.Normal
                                    });

                                    if (process != null)
                                    {
                                        profileUsed = true;
                                        return; // Stop if the URL is successfully opened
                                    }
                                }
                                catch
                                {
                                    // Ignore errors and try the next profile
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AppendLog("ERROR", $"❌ Error reading Chrome profiles: {ex.Message}");
                        }
                    }

                    if (!profileUsed)
                    {
                        // Fallback to default behavior if no profile could open the link
                        var fallbackProcess = Process.Start(new ProcessStartInfo
                        {
                            FileName = "chrome",
                            Arguments = url,
                            UseShellExecute = true,
                            WindowStyle = ProcessWindowStyle.Normal
                        });

                        if (fallbackProcess == null)
                        {
                            // Fallback to the system's default browser
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = url,
                                UseShellExecute = true,
                                WindowStyle = ProcessWindowStyle.Normal
                            });
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                AppendLog("ERROR", $"❌ Error opening browser: {ex.Message}");
            }
        }
        private void UpdateConnectionStatus(bool isConnected)
        {
            if (isConnected)
            {
                statusLabel.Text = "Kết nối: Đã kết nối";
                statusLabel.ForeColor = Color.Green;
            }
            else
            {
                statusLabel.Text = "Kết nối: Chưa kết nối";
                statusLabel.ForeColor = Color.Red;
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
