using System.ComponentModel.DataAnnotations;

namespace Forms.Models.NewModels
{
    public class NewFieldViewModel
    {
        [Required]
        public string fieldType { get; set; }

        [Required]
        public int index { get; set; }

        [Required]
        public string title { get; set; }

        public object value { get; set; }
    }
}