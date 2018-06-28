using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Forms.Models.DBModels
{
    public class FieldViewModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonRequired]
        public ObjectId formId { get; set; }

        [BsonRequired]
        public string fieldType { get; set; }

        [BsonRequired]
        public int index { get; set; }

        [BsonRequired]
        public string title { get; set; }

        [BsonRequired]
        public DateTime createdAt { get; set; }

        public object value { get; set; }
    }
}