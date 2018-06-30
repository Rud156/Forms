using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forms.Models.NewModels
{
    public class NewFormViewModel
    {
        [Required]
        public string createdBy { get; set; }

        [Required]
        public string title { get; set; }

        public List<NewFieldViewModel> fields { get; set; }
    }
}