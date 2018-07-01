using Forms.Models.APIResponseModels;

namespace Forms.Models.ResponseModels
{
    public class ResponseModel
    {
        public bool success { get; set; }
        public ResponseAPIResponseViewModel response { get; set; }
        public long timeElapsed { get; set; }
    }
}