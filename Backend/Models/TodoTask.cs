namespace Backend.Models
{
    public class TodoTask
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public int TaskTier { get; set; }
        public int RewardPL { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
