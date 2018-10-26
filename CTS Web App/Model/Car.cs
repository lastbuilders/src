using System.ComponentModel.DataAnnotations;

namespace CTS_Web_App
{
    public class Car
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int Id { get; set; }
        [Required]
        public string  Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string  Year { get; set; }

    }
}