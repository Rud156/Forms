namespace Forms.Models.BenchmarkResultModels
{
    public class FieldResponseModel
    {
        public string Id { get; set; }

        public string formId { get; set; }

        public string fieldType { get; set; }

        public int index { get; set; }

        public string title { get; set; }

        public string createdAt { get; set; }

        public object value { get; set; }
    }
}