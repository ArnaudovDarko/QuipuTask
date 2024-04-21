using System.ComponentModel.DataAnnotations;

namespace Quipu_Task.Models
{
    public class ClientInfo
    {
        [Key]
        public int ClientId { get; set; }
        [StringLength(100, MinimumLength = 1)]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; } = string.Empty;
        [StringLength(100, MinimumLength = 1)]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; } = string.Empty;
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Home Address is required")]
        public string HomeAddress { get; set; } = string.Empty;
        public string? HomeAddress2 { get; set; }
        [Required]
        public DateOnly DateBirth { get; set; }

    }
}
