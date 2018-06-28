using System;
using System.Collections.Generic;
using System.Linq;
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

        Task<Boolean> DeleteResponse(string responseId);

        Task<Boolean> DeleteResponsesForForm(string formId);

        Task<Boolean> DeleteResponsesCreatedBy(string createdBy);
    }
}
