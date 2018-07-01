using Bogus;
using Forms.Models.NewModels;
using Forms.Utils;
using System;
using System.Collections.Generic;

namespace Forms.Generators
{
    public class FormGenerator
    {
        private Faker faker;
        private Random random;

        public FormGenerator()
        {
            this.faker = new Faker();
            this.random = new Random();
        }

        public NewFieldViewModel GenerateRandomField(int index)
        {
            float randomFieldValue = (float)random.NextDouble();

            string fieldTitle = faker.Lorem.Sentence(20);
            string fieldType = Constants.FieldTypes[(int)MathF.Floor(randomFieldValue * Constants.FieldTypes.Count)];

            object value;
            if (
                fieldType == TypeConstants.SINGLE_LINE_INPUT ||
                fieldType == TypeConstants.PARAGRAPH_TEXT_INPUT ||
                fieldType == TypeConstants.DATE_INPUT ||
                fieldType == TypeConstants.TIME_INPUT ||
                fieldType == TypeConstants.FILE_UPLOAD
            )
                value = "";
            else
            {
                float randomOptionValue = (float)random.NextDouble();
                int randomOptionsCount = (int)MathF.Floor(randomOptionValue * 100);
                randomOptionsCount = randomOptionsCount <= 0 ? 1 : randomOptionsCount;
                List<string> optionsArray = new List<string>();

                for (int i = 0; i < randomOptionsCount; i++)
                    optionsArray.Add(faker.Random.Words(5));
                value = optionsArray;
            }

            return new NewFieldViewModel
            {
                fieldType = fieldType,
                title = fieldTitle,
                index = index,
                value = value,
            };
        }

        public NewFormViewModel GenerateRandomForm(int fieldCount)
        {
            string formTitle = faker.Lorem.Sentence(20);
            string createdBy = faker.Name.FullName();
            List<NewFieldViewModel> fields = new List<NewFieldViewModel>();

            for (int i = 0; i < fieldCount; i++)
                fields.Add(GenerateRandomField(i));

            return new NewFormViewModel
            {
                title = formTitle,
                createdBy = createdBy,
                fields = fields
            };
        }
    }
}