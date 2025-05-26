
namespace MyApp.Domain.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public string CreateUser { get; set; } = "system";
        public DateTime? ModifyDate { get; set; }
        public string? ModifyUser { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
