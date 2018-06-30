using Forms.Models.DBModels;
using Forms.Models.ResponseModels;
using System.Collections.Generic;

namespace Forms.Utils
{
    public static class FormUtils
    {
        public static FormObjectViewModel CombineFormAndFields(FormViewModel form, List<FieldViewModel> fields)
        {
            if (form == null)
                return null;

            FormObjectViewModel formObjectViewModel = new FormObjectViewModel
            {
                Id = form.Id,
                formTitle = form.formTitle,
                createdAt = form.createdAt,
                createdBy = form.createdBy,
                fields = fields
            };

            return formObjectViewModel;
        }
    }
}