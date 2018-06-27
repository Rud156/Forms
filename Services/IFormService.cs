using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forms.Models.DBModels;
using Forms.Models.NewModels;
using Forms.Models.ResponseModels;

namespace Forms.Services
{
  public interface IFormService
  {
    Task<FormObjectViewModel> GetForm(string formId);

    Task<FieldViewModel> GetField(string fieldId);

    Task<IEnumerable<FormObjectViewModel>> GetFormsCreatedBy(string createdBy);

    Task<FormObjectViewModel> CreateForm(NewFormViewModel form);

    Task<FieldViewModel> AddNewFieldToForm(NewFieldViewModel field, string formId);

    Task<FormObjectViewModel> UpdateFormTitle(string formId, string newFormTitle);

    Task<FieldViewModel> UpdateField(FieldViewModel field, string formId);

    Task<Boolean> DeleteField(string fieldId, string formId);

    Task<Boolean> DeleteForm(string formId);
  }
}