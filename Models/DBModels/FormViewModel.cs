using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Forms.Models.DBModels
{
    [BsonIgnoreExtraElements]
    public class FormViewModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonRequired]
        public string createdBy { get; set; }

        [BsonRequired]
        public string formTitle { get; set; }

        [BsonRequired]
        public DateTime createdAt { get; set; }

        public List<ObjectId> fields { get; set; }
    }
}