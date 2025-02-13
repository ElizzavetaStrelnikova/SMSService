namespace SMSService.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Text { get; set; }
    }
}
