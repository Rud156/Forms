using Forms.Models.DBModels;
using Forms.Models.NewModels;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forms.ServiceInterfaces
{
    public interface IResponseService
    {
        Task<ResponseViewModel> GetResponse(ObjectId responseId);

        Task<IEnumerable<ResponseViewModel>> GetResponsesCreatedBy(string createdBy);

        Task<IEnumerable<ResponseViewModel>> GetResponsesForForm(ObjectId formId);

        Task<ResponseViewModel> CreateResponse(NewResponseViewModel response, ObjectId formId);

        Task<ResponseViewModel> UpdateResponse(NewResponseViewModel response, ObjectId formId, ObjectId responseId);

        Task<bool> DeleteResponse(ObjectId responseId);

        Task<long> DeleteResponsesForForm(ObjectId formId);

        Task<long> DeleteResponsesCreatedBy(string createdBy);
    }
}