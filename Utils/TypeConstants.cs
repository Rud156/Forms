namespace Forms.Utils
{
    public static class TypeConstants
    {
        public const string SINGLE_LINE_INPUT = "SINGLE_LINE_INPUT";
        public const string PARAGRAPH_TEXT_INPUT = "PARAGRAPH_TEXT_INPUT";
        public const string RADIO_INPUT = "RADIO_INPUT";
        public const string DROP_DOWN_INPUT = "DROP_DOWN_INPUT";
        public const string CHECKBOX_INPUT = "CHECKBOX_INPUT";
        public const string DATE_INPUT = "DATE_INPUT";
        public const string TIME_INPUT = "TIME_INPUT";
        public const string FILE_UPLOAD = "FILE_UPLOAD";

        public static bool IsValidFieldType(string value)
        {
            return value == SINGLE_LINE_INPUT || value == PARAGRAPH_TEXT_INPUT ||
            value == RADIO_INPUT || value == DROP_DOWN_INPUT ||
            value == DATE_INPUT || value == TIME_INPUT ||
            value == FILE_UPLOAD || value == CHECKBOX_INPUT;
        }
    }
}