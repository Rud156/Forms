using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Forms.Models.DBModels
{
  public class ResponseValueViewModel
  {
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonRequired]
    public ObjectId fieldId { get; set; }

    [BsonRequired]
    public string responseType { get; set; }

    [BsonRequired]
    public int index { get; set; }

    [BsonRequired]
    public object value { get; set; }
  }
}