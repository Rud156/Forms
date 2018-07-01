using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forms.Models.RequestModels
{
    public class FormIdMultipleModel
    {
        [Required]
        public List<string> formId { get; set; }
    }
}
