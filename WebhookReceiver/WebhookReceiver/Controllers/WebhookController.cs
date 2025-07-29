using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebhookReceiver.Hubs;
using WebhookReceiver.Models;

namespace WebhookReceiver.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : Controller
    {
        private readonly IHubContext<SheetChangeEventHub> _hubContext;

        public WebhookController(IHubContext<SheetChangeEventHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> Receive([FromBody] SheetChangeEvent data)
        {
            if (string.IsNullOrEmpty(data?.FileId))
            {
                return BadRequest(new { error = "FileId is required." });
            }
            // Gửi đến tất cả client đã "join" group fileId
            await _hubContext.Clients.Group(data.FileId).SendAsync("ReceiveSheetChange", data);
            return Ok(new { status = "sent" });
        }
    }
}
