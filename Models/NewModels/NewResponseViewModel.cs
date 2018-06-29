using System.ComponentModel.DataAnnotations;

namespace Forms.Models.NewModels
{
    public class NewResponseViewModel
    {
        [Required]
        public string formId { get; set; }

        [Required]
        public string createdBy { get; set; }

        public NewResponseValuesViewModel[] responseValues { get; set; }
    }
}