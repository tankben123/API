namespace WebhookReceiver.Models
{
    public class SheetChangeEvent
    {
        public string? FileId { get; set; }
        public string? FileName { get; set; }
        public string? SheetName { get; set; }
        public string? Range { get; set; }
        public string? NewValue { get; set; }
        public string? User { get; set; }
        public string? Url { get; set; }
        public DateTime EditedAt { get; set; }
    }
}
