using System.Collections.Generic;

namespace Forms.Utils
{
    public static class Constants
    {
        public const string BASE_URL = "http://localhost:3000/api";

        public static IList<string> FieldTypes = new[] {
            TypeConstants.SINGLE_LINE_INPUT,
            TypeConstants.PARAGRAPH_TEXT_INPUT, TypeConstants.RADIO_INPUT,
            TypeConstants.TIME_INPUT, TypeConstants.DATE_INPUT, TypeConstants.DROP_DOWN_INPUT,
            TypeConstants.CHECKBOX_INPUT, TypeConstants.FILE_UPLOAD };
    }
}