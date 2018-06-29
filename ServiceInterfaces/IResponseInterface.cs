using System.Collections.Generic;
using System.Threading.Tasks;
using Forms.Models.DBModels;
using Forms.Models.NewModels;

namespace Forms.ServiceInterfaces
{
    interface IResponseInterface
    {
        Task<ResponseViewModel> GetResponse(string responseId);

        Task<IEnumerable<ResponseViewModel>> GetResponsesCreatedBy(string createdBy);

        Task<IEnumerable<ResponseViewModel>> GetResponsesForForm(string formId);

        Task<ResponseViewModel> CreateResponse(NewResponseViewModel response);

        Task<ResponseViewModel> UpdateResponse(ResponseViewModel response);

        Task<bool> DeleteResponse(string responseId);

        Task<bool> DeleteResponsesForForm(string formId);

        Task<bool> DeleteResponsesCreatedBy(string createdBy);
    }
}
