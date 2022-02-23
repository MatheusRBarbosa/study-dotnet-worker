namespace QueueSimulator.Domain.Models
{
    public class Message
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}