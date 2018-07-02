using Forms.Models.DBModels;
using Forms.Models.NewModels;
using Forms.Models.ResponseModels;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forms.ServiceInterfaces
{
    public interface IFormService
    {
        Task<FormObjectViewModel> GetForm(ObjectId formId);

        Task<FieldViewModel> GetField(ObjectId formId, ObjectId fieldId);

        Task<IEnumerable<FormViewModel>> GetFormsCreatedBy(string createdBy);

        Task<FormObjectViewModel> CreateForm(NewFormViewModel form);

        Task<FieldViewModel> AddNewFieldToForm(NewFieldViewModel field, ObjectId formId);

        Task<FormObjectViewModel> UpdateFormTitle(ObjectId formId, string newFormTitle);

        Task<FieldViewModel> UpdateField(NewFieldViewModel field, ObjectId formId, ObjectId fieldId);

        Task<bool> DeleteField(ObjectId formId, ObjectId fieldId);

        Task<bool> DeleteForm(ObjectId formId);

        Task<long> DeleteFormsCreatedBy(string createdBy);
    }
}