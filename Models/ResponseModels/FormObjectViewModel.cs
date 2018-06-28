using System;
using Forms.Models.DBModels;

namespace Forms.Models.ResponseModels
{
    public class FormObjectViewModel
    {
        public string createdBy { get; set; }

        public string formTitle { get; set; }

        public DateTime createdAt { get; set; }

        public FieldViewModel[] fields { get; set; }
    }
}