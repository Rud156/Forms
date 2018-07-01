using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forms.Models.APIResponseModels
{
    public class FormResponseModel
    {
        public bool success { get; set; }

        public FormObjectViewModelResponse form { get; set; }
    }
}
