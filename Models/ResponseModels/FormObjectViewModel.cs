using Forms.Models.DBModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Forms.Models.ResponseModels
{
    public class FormObjectViewModel
    {
        public ObjectId Id { get; set; }

        public string createdBy { get; set; }

        public string formTitle { get; set; }

        public DateTime createdAt { get; set; }

        public List<FieldViewModel> fields { get; set; }
    }
}