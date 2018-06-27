using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Forms.Models.DBModels
{
  public class ResponseViewModel
  {
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonRequired]
    public ObjectId formId { get; set; }

    [BsonRequired]
    public string createdBy { get; set; }

    [BsonRequired]
    public DateTime createdAt { get; set; }

    [BsonRequired]
    public ResponseValueViewModel[] responseValues { get; set; }
  }
}