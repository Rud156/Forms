using System.ComponentModel.DataAnnotations;

namespace Forms.Models.NewModels
{
    public class NewFormViewModel
    {
        [Required]
        public string createdBy { get; set; }

        [Required]
        public string title { get; set; }

        public NewFieldViewModel[] fields { get; set; }
    }
}