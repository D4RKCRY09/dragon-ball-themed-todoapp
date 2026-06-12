using Microsoft.AspNetCore.Identity;

namespace Backend.Models
{
    public class AppUser : IdentityUser
    {
        public long PowerLevel { get; set; }
        public int CurrentForm {  get; set; }
        public ICollection<TodoTask> Tasks { get; set; }
    }
}
