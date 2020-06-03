using System;
using System.ComponentModel.DataAnnotations;

namespace MVC_REST_API.Models
{
    public class Command
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string HowTo { get; set; }
        
        [Required]
        public string Line { get; set; }
        
        [Required]
        public string Platform { get; set; }
    }
}
