using System.Collections.Generic;

namespace Forms.Models.APIResponseModels
{
    public class FormObjectViewModelResponse
    {
        public string Id { get; set; }

        public string createdBy { get; set; }

        public string formTitle { get; set; }

        public string createdAt { get; set; }

        public List<FieldViewResponseModel> fields { get; set; }
    }
}