using System.Collections.Generic;

namespace Forms.Models.APIResponseModels
{
    public class ResponseAPIResponseViewModel
    {
        public string Id { get; set; }

        public string formId { get; set; }

        public string createdBy { get; set; }

        public string createdAt { get; set; }

        public List<ResponseValueResponseModel> responseValues { get; set; }
    }
}