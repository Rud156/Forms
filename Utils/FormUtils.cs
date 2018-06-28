using System;
using System.Collections.Generic;
using Forms.Models.DBModels;
using Forms.Models.ResponseModels;

namespace Forms.Utils
{
    public static class FormUtils
    {
        public static FormObjectViewModel CombineFormAndFields(FormViewModel form, List<FieldViewModel> fields)
        {
            FormObjectViewModel formObjectViewModel = new FormObjectViewModel();
            formObjectViewModel.Id = form.Id;
            formObjectViewModel.formTitle = form.formTitle;
            formObjectViewModel.createdAt = form.createdAt;
            formObjectViewModel.createdBy = form.createdBy;
            formObjectViewModel.fields = fields.ToArray();

            return formObjectViewModel;
        }
    }
}