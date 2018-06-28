using System;

namespace Forms.Config
{
    public static class Config
    {
        public const string DBUri = "mongodb://localhost:27017";

        public const string DBName = "Forms";

        public const string FormCollectionName = "forms";

        public const string FieldCollectionName = "fields";

        public const string ResponseCollectionName = "responses";

        public const string PORT = "3000";

        public const string hostUrl = "http://*:";
    }
}