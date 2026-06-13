namespace Backend.DTOs
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        // 0 for Daily, 1 for Weekly
        public int TaskTier { get; set; }
    }
}
