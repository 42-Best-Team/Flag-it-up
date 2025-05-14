using System.ComponentModel.DataAnnotations;

namespace FlagItUpApp.Models
{
    public class Flag
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Country { get; set; }


        [Required]
        public string ImagePath { get; set; }

        public Flag()
        {
            Country = string.Empty;
            ImagePath = string.Empty;
        }
    }
}
