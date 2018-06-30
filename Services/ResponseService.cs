using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Forms.Models.DBModels;
using Forms.Models.NewModels;
using Forms.Models.ResponseModels;
using Forms.ServiceInterfaces;
using Forms.Utils;

namespace Forms.Services
{
    public class ResponseService : IResponseService
    {
        private IMongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<FormViewModel> formCollection;
        private IMongoCollection<FieldViewModel> fieldCollection;
        private IMongoCollection<ResponseViewModel> responseCollection;

        public ResponseService() => ConnectToDatabase();

        private void ConnectToDatabase()
        {
            MongoUrl mongoUrl = new MongoUrl($"{Config.Config.DBUri}/{Config.Config.DBName}");
            client = new MongoClient(mongoUrl);
            database = client.GetDatabase(Config.Config.DBName);

            bool mongoLive = database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);
            if (mongoLive)
                Console.WriteLine("Connected to MongoDB");
            else
                throw new Exception("Unable to connect to MongoDB. Aborting...");

            responseCollection = database.GetCollection<ResponseViewModel>(Config.Config.ResponseCollectionName);
            formCollection = database.GetCollection<FormViewModel>(Config.Config.FormCollectionName);
            fieldCollection = database.GetCollection<FieldViewModel>(Config.Config.FieldCollectionName);
        }

        public async Task<FormObjectViewModel> GetForm(ObjectId formObjectId)
        {
            var formTask = await formCollection.FindAsync(_ => _.Id == formObjectId);
            var fieldTask = await fieldCollection.FindAsync(_ => _.formId == formObjectId);

            FormViewModel form = await formTask.SingleOrDefaultAsync();
            List<FieldViewModel> fields = await fieldTask.ToListAsync();
            return FormUtils.CombineFormAndFields(form, fields);
        }


        public async Task<ResponseViewModel> GetResponse(ObjectId responseId)
        {
            var responseTask = await responseCollection.FindAsync(_ => _.Id == responseId);
            return await responseTask.SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ResponseViewModel>> GetResponsesForForm(ObjectId formId)
        {
            var responseTask = await responseCollection.FindAsync(_ => _.formId == formId);
            return await responseTask.ToListAsync();
        }

        public async Task<IEnumerable<ResponseViewModel>> GetResponsesCreatedBy(string createdBy)
        {
            var responseTask = await responseCollection.FindAsync(_ => _.createdBy == createdBy);
            return await responseTask.ToListAsync();
        }

        public async Task<ResponseViewModel> CreateResponse(NewResponseViewModel response, ObjectId formId)
        {
            FormObjectViewModel form = await GetForm(formId);
            List<FieldViewModel> fields = form.fields;

            if (fields.Count != response.responseValues.Count)
                throw new Exception("Responses do not match fields");

            bool fieldsValid = ResponseUtils.ResponseValidator(new List<FieldViewModel>(fields),
                new List<NewResponseValuesViewModel>(response.responseValues));
            if (!fieldsValid)
                throw new Exception("Responses do not match fields");

            List<ResponseValueViewModel> responseValues = new List<ResponseValueViewModel>();
            foreach (var responseValue in response.responseValues)
            {
                if (!TypeConstants.isValidFieldType(responseValue.responseType))
                    throw new Exception("Invalid Response Type");

                ResponseValueViewModel responseValueViewModel = new ResponseValueViewModel
                {
                    Id = ObjectId.GenerateNewId(),
                    responseType = responseValue.responseType,
                    value = responseValue.value,
                    index = responseValue.index,
                    fieldId = ObjectId.Parse(responseValue.fieldId)
                };
                responseValues.Add(responseValueViewModel);
            }

            ResponseViewModel responseViewModel = new ResponseViewModel
            {
                Id = ObjectId.GenerateNewId(),
                createdBy = response.createdBy,
                createdAt = DateTime.UtcNow,
                formId = formId,
                responseValues = responseValues
            };

            await responseCollection.InsertOneAsync(responseViewModel);
            return responseViewModel;
        }

        public async Task<ResponseViewModel> UpdateResponse(NewResponseViewModel response,
            ObjectId formId,
            ObjectId responseId)
        {
            DeleteResult responseDeleteResult = await responseCollection.DeleteOneAsync(_ => _.Id == responseId);
            if (!responseDeleteResult.IsAcknowledged)
                throw new Exception("Unable to update previous response");

            FormObjectViewModel form = await GetForm(formId);
            List<FieldViewModel> fields = form.fields;

            if (fields.Count != response.responseValues.Count)
                throw new Exception("Responses do not match fields");

            bool fieldsValid = ResponseUtils.ResponseValidator(new List<FieldViewModel>(fields),
                new List<NewResponseValuesViewModel>(response.responseValues));
            if (!fieldsValid)
                throw new Exception("Responses do not match fields");

            return await CreateResponse(response, formId);
        }

        public async Task<bool> DeleteResponse(ObjectId responseId)
        {
            DeleteResult responseDeleteResult = await responseCollection.DeleteOneAsync(_ => _.Id == responseId);
            if (!responseDeleteResult.IsAcknowledged)
                throw new Exception("Unable to delete response");

            return true;
        }

        public async Task<long> DeleteResponsesForForm(ObjectId formId)
        {
            DeleteResult responseDeleteResult = await responseCollection.DeleteManyAsync(_ => _.formId == formId);
            if (!responseDeleteResult.IsAcknowledged)
                throw new Exception("Unable to responses");

            return responseDeleteResult.DeletedCount;
        }

        public async Task<long> DeleteResponsesCreatedBy(string createdBy)
        {
            DeleteResult responseDeleteResult = await responseCollection.DeleteManyAsync(_ => _.createdBy == createdBy);
            if (!responseDeleteResult.IsAcknowledged)
                throw new Exception("Unable to responses");

            return responseDeleteResult.DeletedCount;
        }
    }
}