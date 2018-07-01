using Forms.Models.DBModels;

namespace Forms.Models.BenchmarkResultModels
{
    public class ResponseModel
    {
        public bool success { get; set; }
        public ResponseViewModel response { get; set; }
        public long timeElapsed { get; set; }

    }
}