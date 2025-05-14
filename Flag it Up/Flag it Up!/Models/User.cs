using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlagItUpApp.Models
{
    [Table("Users")]
    public class User
    {
        [Key] // Вказуємо, що Username є основним ключем
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int Id { get; set; }

    }
}
