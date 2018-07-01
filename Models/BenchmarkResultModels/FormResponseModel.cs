using Forms.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forms.Models.BenchmarkResultModels
{
    public class FormResponseModel
    {
        public bool success { get; set; }

        public FormObjectViewModelResponse form { get; set; }
    }
}
