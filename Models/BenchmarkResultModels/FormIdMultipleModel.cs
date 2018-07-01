using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forms.Models.BenchmarkResultModels
{
    public class FormIdMultipleModel
    {
        [Required]
        public List<string> formId { get; set; }
    }
}
