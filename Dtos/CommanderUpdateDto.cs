using System.ComponentModel.DataAnnotations;

namespace MVC_REST_API.Dtos
{
    public class CommanderUpdateDto
    {
        [Required]
        [MaxLength(250)]
        public string HowTo { get; set; }

        [Required]
        public string Line { get; set; }

        [Required]
        public string Platform { get; set; }
    }
}
