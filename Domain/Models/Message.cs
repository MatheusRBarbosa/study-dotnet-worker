namespace QueueSimulator.Domain.Models
{
    public class Message
    {
        public string Name { get; set; } = null!;
        public int Position { get; set; }
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}