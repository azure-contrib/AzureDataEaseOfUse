using System;

namespace AzureDataEaseOfUse.Tables
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class TableNameAttribute : Attribute
    {
        public TableNameAttribute(string name)
        {
            TableName = name;
        }

        public readonly string TableName;
    }
}