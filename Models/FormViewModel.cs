using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Forms.Models
{
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
    public ObjectId[] fields { get; set; }
  }
}