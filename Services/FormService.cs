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
using Forms.ServiceInterfaces;

namespace Forms.Services
{
    public class FormService : IFormService
    {
        private IMongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<FormViewModel> formCollection;
        private IMongoCollection<FieldViewModel> fieldCollection;

        public FormService() => ConnectToDatabase();

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

        public async Task<FieldViewModel> GetField(ObjectId formId, ObjectId fieldId)
        {
            var fieldTask = await fieldCollection.FindAsync(_ => _.Id == fieldId && _.formId == formId);
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
                string fieldType = field.fieldType;
                if (!TypeConstants.isValidFieldType(fieldType))
                    throw new Exception("Invalid Field Type");

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
                formTitle = form.title,
            };

            await formCollection.InsertOneAsync(formViewModel);
            return FormUtils.CombineFormAndFields(formViewModel, fields);
        }

        public async Task<FieldViewModel> AddNewFieldToForm(NewFieldViewModel field, ObjectId formId)
        {
            ObjectId fieldObjectId = ObjectId.GenerateNewId();

            if (!TypeConstants.isValidFieldType(field.fieldType))
                throw new Exception("Invalid Field Type");

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

        public async Task<FormObjectViewModel> UpdateFormTitle(ObjectId formId, string newFormTitle)
        {
            UpdateResult formUpdateResult = await formCollection.UpdateOneAsync(_ => _.Id == formId,
                Builders<FormViewModel>.Update.Set(_ => _.formTitle, newFormTitle));

            if (formUpdateResult.IsAcknowledged)
            {
                return await GetForm(formId);
            }
            else
                throw new Exception("Unable to update form title");
        }

        public async Task<FieldViewModel> UpdateField(FieldViewModel field, ObjectId formId, ObjectId fieldId)
        {
            if (!TypeConstants.isValidFieldType(field.fieldType))
                throw new Exception("Invalid Field Type");

            UpdateResult fieldUpdateResult = await fieldCollection.UpdateOneAsync(
                _ => _.Id == fieldId && _.formId == formId,
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
                throw new Exception("Unable to update field content");
        }

        public async Task<bool> DeleteField(ObjectId formId, ObjectId fieldId)
        {
            UpdateResult formUpdateResult = await formCollection.UpdateOneAsync(_ => _.Id == formId,
                Builders<FormViewModel>.Update
                .Pull(_ => _.fields, fieldId));
            if (!formUpdateResult.IsAcknowledged)
                throw new Exception("Unable to delete field");

            DeleteResult fieldDeleteResult = await fieldCollection.DeleteOneAsync(_ => _.Id == fieldId);
            return true;
        }

        public async Task<bool> DeleteForm(ObjectId formId)
        {
            DeleteResult formDeleteResult = await formCollection.DeleteOneAsync(_ => _.Id == formId);
            if (!formDeleteResult.IsAcknowledged)
                throw new Exception("Unable to delete form");

            await fieldCollection.DeleteManyAsync(_ => _.formId == formId);

            return true;
        }

        public async Task<long> DeleteFormsCreatedBy(string createdBy)
        {
            var formTask = await formCollection.FindAsync(_ => _.createdBy == createdBy);
            List<FormViewModel> forms = await formTask.ToListAsync();
            HashSet<ObjectId> formObjectIds = new HashSet<ObjectId>(forms.Select(_ => _.Id));


            DeleteResult formDeleteResult = await formCollection.DeleteManyAsync(_ => _.createdBy == createdBy);
            if (!formDeleteResult.IsAcknowledged)
                throw new Exception($"Unable to delete forms created by {createdBy}");

            await fieldCollection.DeleteManyAsync(_ => formObjectIds.Contains(_.formId));

            return formDeleteResult.DeletedCount;
        }
    }
}