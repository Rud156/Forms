using Forms.Models.BenchmarkResultModels;
using Forms.Models.DBModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Forms.Models.ResponseModels
{
    public class FormObjectViewModelResponse
    {
        public string Id { get; set; }

        public string createdBy { get; set; }

        public string formTitle { get; set; }

        public string createdAt { get; set; }

        public List<FieldResponseModel> fields { get; set; }
    }
}