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

namespace Forms.Services
{
    public class FormService : IFormService
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<FormViewModel> _formCollection;
        private IMongoCollection<FieldViewModel> _fieldCollection;
        private IMongoCollection<ResponseViewModel> _responseCollection;

        public FormService() => ConnectToDatabase();

        public void ConnectToDatabase()
        {
            MongoUrl mongoUrl = new MongoUrl($"{Config.Config.DBUri}/{Config.Config.DBName}");
            _client = new MongoClient(mongoUrl);
            _database = _client.GetDatabase(Config.Config.DBName);

            bool mongoLive = _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);
            if (mongoLive)
                Console.WriteLine("Connected to MongoDB");
            else
                throw new Exception("Unable to connect to MongoDB. Aborting...");


            _formCollection = _database.GetCollection<FormViewModel>(Config.Config.FormCollectionName);
            _fieldCollection = _database.GetCollection<FieldViewModel>(Config.Config.FieldCollectionName);
            _responseCollection = _database.GetCollection<ResponseViewModel>(Config.Config.ResponseCollectionName);
        }

        public async Task<FormObjectViewModel> GetForm(string formId)
        {
            ObjectId formObjectId = ObjectId.Parse(formId);
            var task = await _formCollection.FindAsync(_ => _.Id == formObjectId);
            

            throw new NotImplementedException();
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