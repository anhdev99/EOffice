using System;
using MongoDB.Bson.Serialization.Attributes;

namespace EOffice.WebAPI.Models
{
    public class HistoryQuestion: Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string QuestionId { get; set; }
        /// <summary>
        /// Action as [Question, Answer]
        /// </summary>
        public string TypeHistory { get; set; }
        public string Action { get; set; }
        public HistoryAction HistoryAction { get; set; }
        public StatusQuestion StatusQuestion { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Hash { get; set; }
        public string ReferenceHash { get; set; }
        public dynamic OldValue { get; set; }
    }
}