using System.ComponentModel.DataAnnotations;

namespace Forms.Models.NewModels
{
    public class NewResponseValuesViewModel
    {
        [Required]
        public string fieldId { get; set; }

        [Required]
        public string responseType { get; set; }

        [Required]
        public int index { get; set; }

        [Required]
        public object value { get; set; }
    }
}