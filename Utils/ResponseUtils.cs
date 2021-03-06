using Forms.Models.DBModels;
using Forms.Models.NewModels;
using System;
using System.Collections.Generic;

namespace Forms.Utils
{
    public static class ResponseUtils
    {
        public static bool ResponseValidator(List<FieldViewModel> fields,
            List<NewResponseValuesViewModel> responseValues)
        {
            fields.Sort((a, b) => a.index.CompareTo(b.index));
            responseValues.Sort((a, b) => a.index.CompareTo(b.index));

            bool formValid = true;

            for (int i = 0; i < fields.Count; i++)
            {
                string responseType = responseValues[i].responseType;

                if (
                    responseType != TypeConstants.SINGLE_LINE_INPUT &&
                    responseType != TypeConstants.PARAGRAPH_TEXT_INPUT &&
                    responseType != TypeConstants.DATE_INPUT &&
                    responseType != TypeConstants.TIME_INPUT &&
                    responseType != TypeConstants.FILE_UPLOAD
                )
                {
                    Console.WriteLine(fields[i].value.GetType());

                    HashSet<string> fieldValue = new HashSet<string>(fields[i].value as List<string>);

                    if (responseType == TypeConstants.CHECKBOX_INPUT)
                    {
                        List<string> values = Constants.ConvertJsonObject(responseValues[i].value);
                        foreach (var value in values)
                        {
                            if (!fieldValue.Contains(value))
                            {
                                formValid = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        string value = responseValues[i].value as string;
                        if (!fieldValue.Contains(value))
                        {
                            formValid = false;
                            break;
                        }
                    }
                }
            }

            return formValid;
        }
    }
}