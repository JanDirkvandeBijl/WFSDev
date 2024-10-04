using System.ComponentModel.DataAnnotations;

namespace WFSDev.Models
{
    public class LocalizedResourceDetails
    {
        [Required]
        public string? Key { get; set; }
        [Required]
        public List<Translation>? Translations { get; set; }
    }

    public class Translation
    {
        public int? Id { get; set; } // Translation ID
        public int CultureId { get; set; } // ID of the culture
        public string? Value { get; set; } // The actual translation text
    }
}
