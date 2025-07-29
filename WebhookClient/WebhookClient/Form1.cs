using Microsoft.AspNetCore.SignalR.Client;

namespace WebhookClient
{
    public partial class Form1 : Form
    {
        private HubConnection _hubConnection;
        public Form1()
        {
            InitializeComponent();
            ConnectToSignalR();
        }

        private async void ConnectToSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://sheet-api-c921.onrender.com/hubs/sheetChange") // Thay URL nếu khác
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<object>("ReceiveSheetChange", (data) =>
            {
                Invoke(() =>
                {
                    txtSignalR.AppendText($"📥 Dữ liệu mới nhận:\n{data}\n\n");
                });
            });

            try
            {
                await _hubConnection.StartAsync();
                txtSignalR.AppendText("✅ Đã kết nối tới server.\n");

                // Nếu có nhiều Sheet và muốn chia group theo FileId:
                await _hubConnection.InvokeAsync("JoinFileGroup", "1oywoq5nIQkOEA0Z1gj2LrvSz4zXwJgX8DMZNOfEzQV4");
            }
            catch (Exception ex)
            {
                txtSignalR.AppendText($"❌ Không kết nối được: {ex.Message}\n");
            }
        }
    }
}
