using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Forms.Models.DBModels;
using Forms.Models.NewModels;
using Forms.Models.ResponseModels;

namespace Forms.ServiceInterfaces
{
    public interface IFormService
    {
        Task<FormObjectViewModel> GetForm(ObjectId formId);

        Task<FieldViewModel> GetField(ObjectId fieldId);

        Task<IEnumerable<FormViewModel>> GetFormsCreatedBy(string createdBy);

        Task<FormObjectViewModel> CreateForm(NewFormViewModel form);

        Task<FieldViewModel> AddNewFieldToForm(NewFieldViewModel field, ObjectId formId);

        Task<FormViewModel> UpdateFormTitle(ObjectId formId, string newFormTitle);

        Task<FieldViewModel> UpdateField(FieldViewModel field, ObjectId fieldId);

        Task<bool> DeleteField(ObjectId fieldId, ObjectId formId);

        Task<bool> DeleteForm(ObjectId formId);

        Task<bool> DeleteFormsCreatedBy(string createdBy);
    }
}