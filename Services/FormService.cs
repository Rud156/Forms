using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Forms.ServiceInterfaces;
using Forms.Models.DBModels;
using Forms.Models.NewModels;
using Forms.Models.ResponseModels;
using Forms.Utils;

namespace Forms.Services
{
    public class FormService : IFormService
    {
        private IMongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<FormViewModel> formCollection;
        private IMongoCollection<FieldViewModel> fieldCollection;
        private IMongoCollection<ResponseViewModel> responseCollection;

        public FormService() => ConnectToDatabase();

        public void ConnectToDatabase()
        {
            MongoUrl mongoUrl = new MongoUrl($"{Config.Config.DBUri}/{Config.Config.DBName}");
            client = new MongoClient(mongoUrl);
            database = client.GetDatabase(Config.Config.DBName);

            bool mongoLive = database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);
            if (mongoLive)
                Console.WriteLine("Connected to MongoDB");
            else
                throw new Exception("Unable to connect to MongoDB. Aborting...");


            formCollection = database.GetCollection<FormViewModel>(Config.Config.FormCollectionName);
            fieldCollection = database.GetCollection<FieldViewModel>(Config.Config.FieldCollectionName);
            responseCollection = database.GetCollection<ResponseViewModel>(Config.Config.ResponseCollectionName);
        }

        public async Task<FormObjectViewModel> GetForm(string formId)
        {
            ObjectId formObjectId = ObjectId.Parse(formId);
            var formTask = await formCollection.FindAsync(_ => _.Id == formObjectId);
            var fieldTask = await fieldCollection.FindAsync(_ => _.formId == formObjectId);

            FormViewModel form = await formTask.SingleOrDefaultAsync();
            List<FieldViewModel> fields = await fieldTask.ToListAsync();
            return FormUtils.CombineFormAndFields(form, fields);
        }

        public Task<FieldViewModel> GetField(string fieldId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FormObjectViewModel>> GetFormsCreatedBy(string createdBy)
        {
            throw new NotImplementedException();
        }

        public Task<FormObjectViewModel> CreateForm(NewFormViewModel form)
        {
            throw new NotImplementedException();
        }

        public Task<FieldViewModel> AddNewFieldToForm(NewFieldViewModel field, string formId)
        {
            throw new NotImplementedException();
        }

        public Task<FieldViewModel> UpdateField(FieldViewModel field, string formId)
        {
            throw new NotImplementedException();
        }

        public Task<FormObjectViewModel> UpdateFormTitle(string formId, string newFormTitle)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteField(string fieldId, string formId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteForm(string formId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteFormsCreatedBy(string createdBy)
        {
            throw new NotImplementedException();
        }
    }
}