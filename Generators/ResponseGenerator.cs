using System;
using System.Collections.Generic;
using Bogus;
using MongoDB.Bson;
using Forms.Models.NewModels;
using Forms.Utils;
using System.Linq;

namespace Forms.Generators
{
    public class ResponseGenerator
    {
        Faker faker;
        Random random;

        public ResponseGenerator()
        {
            this.faker = new Faker();
            this.random = new Random();
        }

        public NewResponseValuesViewModel GenerateResponseValue(string fieldId, string responseType,
            int index, object value)
        {
            object resultValue;
            if (responseType == TypeConstants.SINGLE_LINE_INPUT)
                resultValue = faker.Lorem.Words(5);
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
                List<string> arrayValues = value as List<string>;
                float randomOptionValue = (float)random.NextDouble();
                int randomNumber = (int)Math.Floor(randomOptionValue * arrayValues.Count);
                List<string> results = new List<string>();

                for (int i = 0; i < randomNumber; i++)
                    results.Add(faker.Random.ListItem(arrayValues));

                resultValue = results;
            }
            else
            {
                string[] arrayValues = value as string[];
                resultValue = faker.Random.ArrayElement(arrayValues);
            }


            return new NewResponseValuesViewModel
            {
                fieldId = fieldId,
                responseType = responseType,
                index = index,
                value = value
            };
        }
    }
}