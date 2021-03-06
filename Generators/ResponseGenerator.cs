using Bogus;
using Forms.Models.APIResponseModels;
using Forms.Models.NewModels;
using Forms.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Forms.Generators
{
    public class ResponseGenerator
    {
        private Faker faker;
        private Random random;

        public ResponseGenerator()
        {
            this.faker = new Faker();
            this.random = new Random();
        }

        public NewResponseValuesViewModel GenerateCorrectResponseValue(string fieldId, string responseType,
            int index, object value)
        {
            object resultValue;
            if (responseType == TypeConstants.SINGLE_LINE_INPUT)
                resultValue = faker.Random.Words(5);
            else if (responseType == TypeConstants.PARAGRAPH_TEXT_INPUT)
                resultValue = faker.Lorem.Sentences(3);
            else if (responseType == TypeConstants.DATE_INPUT)
                resultValue = faker.Date.Future();
            else if (responseType == TypeConstants.TIME_INPUT)
                resultValue = faker.Date.Recent();
            else if (responseType == TypeConstants.FILE_UPLOAD)
                resultValue = faker.Internet.Url();
            else if (responseType == TypeConstants.CHECKBOX_INPUT)
            {
                List<string> arrayValues = Constants.ConvertJsonObject(value);
                float randomOptionValue = (float)random.NextDouble();
                int randomNumber = (int)Math.Floor(randomOptionValue * arrayValues.Count);
                List<string> results = new List<string>();

                for (int i = 0; i < randomNumber; i++)
                    results.Add(faker.Random.ListItem(arrayValues));

                resultValue = results;
            }
            else
            {
                var values = Constants.ConvertJsonObject(value);
                resultValue = faker.Random.ListItem<string>(values);
            }

            return new NewResponseValuesViewModel
            {
                fieldId = fieldId,
                responseType = responseType,
                index = index,
                value = resultValue
            };
        }

        public NewResponseValuesViewModel GenerateInCorrectResponseValue(string fieldId, string responseType, int index)
        {
            List<string> values = new List<string>();
            values.Add(faker.Random.Words(20));

            object valueObject;
            if (responseType != TypeConstants.CHECKBOX_INPUT)
                valueObject = faker.Random.Words(20);
            else
                valueObject = values;

            return new NewResponseValuesViewModel
            {
                fieldId = fieldId,
                responseType = responseType,
                index = index,
                value = valueObject
            };
        }

        public NewResponseViewModel GenerateRandomResponse(FormObjectViewModelResponse form, float incorrectRatio)
        {
            string formId = form.Id;
            string createdBy = faker.Name.FullName();

            List<NewResponseValuesViewModel> responseValues = new List<NewResponseValuesViewModel>();
            foreach (var field in form.fields)
            {
                float randomValue = (float)random.NextDouble();
                if (randomValue < incorrectRatio)
                    responseValues.Add(
                            GenerateInCorrectResponseValue(
                                field.Id,
                                field.fieldType,
                                field.index
                            )
                        );
                else
                    responseValues.Add(
                        GenerateCorrectResponseValue(
                            field.Id,
                            field.fieldType,
                            field.index,
                            field.value
                        )
                    );
            }

            return new NewResponseViewModel
            {
                formId = formId,
                createdBy = createdBy,
                responseValues = responseValues
            };
        }
    }
}