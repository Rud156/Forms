using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
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

        public async Task<FormObjectViewModel> GetForm(ObjectId formObjectId)
        {
            var formTask = await formCollection.FindAsync(_ => _.Id == formObjectId);
            var fieldTask = await fieldCollection.FindAsync(_ => _.formId == formObjectId);

            FormViewModel form = await formTask.SingleOrDefaultAsync();
            List<FieldViewModel> fields = await fieldTask.ToListAsync();
            return FormUtils.CombineFormAndFields(form, fields);
        }

        public async Task<FieldViewModel> GetField(ObjectId fieldId)
        {
            var fieldTask = await fieldCollection.FindAsync(_ => _.Id == fieldId);
            return await fieldTask.SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<FormViewModel>> GetFormsCreatedBy(string createdBy)
        {
            var formTask = await formCollection.FindAsync(_ => _.createdBy == createdBy);
            return await formTask.ToListAsync();
        }

        public async Task<FormObjectViewModel> CreateForm(NewFormViewModel form)
        {
            ObjectId formId = ObjectId.GenerateNewId();
            List<FieldViewModel> fields = new List<FieldViewModel>();

            foreach (var field in form.fields)
            {
                FieldViewModel fieldViewModel = new FieldViewModel
                {
                    Id = ObjectId.GenerateNewId(),
                    formId = formId,
                    fieldType = field.fieldType,
                    index = field.index,
                    title = field.title,
                    createdAt = DateTime.UtcNow,
                    value = field.value
                };
                fields.Add(fieldViewModel);
            }
            await fieldCollection.InsertManyAsync(fields);

            FormViewModel formViewModel = new FormViewModel
            {
                Id = formId,
                createdAt = DateTime.UtcNow,
                createdBy = form.createdBy,
                formTitle = form.formTitle,
            };

            await formCollection.InsertOneAsync(formViewModel);
            return FormUtils.CombineFormAndFields(formViewModel, fields);
        }

        public async Task<FieldViewModel> AddNewFieldToForm(NewFieldViewModel field, ObjectId formId)
        {
            ObjectId fieldObjectId = ObjectId.GenerateNewId();

            FieldViewModel fieldViewModel = new FieldViewModel
            {
                Id = fieldObjectId,
                formId = formId,
                createdAt = DateTime.UtcNow,
                index = field.index,
                fieldType = field.fieldType,
                value = field.value,
            };

            await formCollection.UpdateOneAsync(_ => _.Id == formId,
                Builders<FormViewModel>.Update.Push<ObjectId>(_ => _.fields, fieldObjectId));
            await fieldCollection.InsertOneAsync(fieldViewModel);

            return fieldViewModel;
        }

        public async Task<FormViewModel> UpdateFormTitle(ObjectId formId, string newFormTitle)
        {
            UpdateResult formUpdateResult = await formCollection.UpdateOneAsync(_ => _.Id == formId,
                Builders<FormViewModel>.Update.Set(_ => _.formTitle, newFormTitle));

            if (formUpdateResult.IsAcknowledged)
            {
                var formTask = await formCollection.FindAsync(_ => _.Id == formId);
                return await formTask.SingleOrDefaultAsync();
            }
            else
                return null;
        }

        public async Task<FieldViewModel> UpdateField(FieldViewModel field, ObjectId fieldId)
        {
            UpdateResult fieldUpdateResult = await fieldCollection.UpdateOneAsync(_ => _.Id == fieldId,
                Builders<FieldViewModel>.Update
                .Set(_ => _.title, field.title)
                .Set(_ => _.fieldType, field.fieldType)
                .Set(_ => _.index, field.index)
                .Set(_ => _.value, field.value));

            if (fieldUpdateResult.IsAcknowledged)
            {
                var fieldTask = await fieldCollection.FindAsync(_ => _.Id == fieldId);
                return await fieldTask.SingleOrDefaultAsync();
            }
            else
                return null;
        }

        public async Task<bool> DeleteField(ObjectId fieldId, ObjectId formId)
        {
            UpdateResult formUpdateResult = await formCollection.UpdateOneAsync(_ => _.Id == formId,
                Builders<FormViewModel>.Update
                .Pull(_ => _.fields, fieldId));
            if (!formUpdateResult.IsAcknowledged)
                return false;

            DeleteResult fieldDeleteResult = await fieldCollection.DeleteOneAsync(_ => _.Id == fieldId);
            return fieldDeleteResult.IsAcknowledged;
        }

        public async Task<bool> DeleteForm(ObjectId formId)
        {
            DeleteResult formDeleteResult = await formCollection.DeleteOneAsync(_ => _.Id == formId);
            if (!formDeleteResult.IsAcknowledged)
                return false;

            await fieldCollection.DeleteManyAsync(_ => _.formId == formId);

            return true;
        }

        public async Task<bool> DeleteFormsCreatedBy(string createdBy)
        {
            var formTask = await formCollection.FindAsync(_ => _.createdBy == createdBy);
            List<FormViewModel> forms = await formTask.ToListAsync();
            HashSet<ObjectId> formObjectIds = new HashSet<ObjectId>(forms.Select(_ => _.Id));


            DeleteResult formDeleteResult = await formCollection.DeleteManyAsync(_ => _.createdBy == createdBy);
            if (!formDeleteResult.IsAcknowledged)
                return false;

            await fieldCollection.DeleteManyAsync(_ => formObjectIds.Contains(_.formId));

            return true;
        }
    }
}