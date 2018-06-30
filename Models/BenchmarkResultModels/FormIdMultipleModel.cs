using System.ComponentModel.DataAnnotations;

namespace Forms.Models.BenchmarkResultModels
{
    public class FormIdMultipleModel
    {
        [Required]
        public string[] formId { get; set; }
    }
}
