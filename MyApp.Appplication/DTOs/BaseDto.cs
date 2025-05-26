
namespace MyApp.Application.DTOs
{
    public abstract class BaseDto
    {
        public int? Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public string CreateUser { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string? ModifyUser { get; set; }
        public bool IsActive { get; set; }
    }
}
