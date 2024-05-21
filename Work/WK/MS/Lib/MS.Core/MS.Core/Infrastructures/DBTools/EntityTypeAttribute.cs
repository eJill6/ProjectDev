using Amazon.S3.Model;
using Dapper;
using MS.Core.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Data;
namespace MMService.DBTools
{
    public class EntityTypeAttribute : Attribute
    {
        public EntityTypeAttribute(DbType dbType, int length = 32, bool isAnsi = true, bool isFixedLength = false)
        {
            IsAnsi = isAnsi;
            IsFixedLength = isFixedLength;
            Length = length;
            DbType = dbType;
        }

        public EntityTypeAttribute(DbType dbType, StringType stringType, int length = 32)
        {
            switch(stringType)
            {
                case StringType.Nvarchar:
                    IsFixedLength = false;
                    IsAnsi = false;
                    break;
                case StringType.Varchar:
                    IsFixedLength = false;
                    IsAnsi = true;
                    break;
                case StringType.Nchar:
                    IsFixedLength = true;
                    IsAnsi = false;
                    break;
               
                case StringType.Char:
                    IsFixedLength = true;
                    IsAnsi = true;
                    break;
            }
            Length = length;
            DbType = dbType;
        }

        /// <summary>
        /// Ansi vs Unicode 
        /// </summary>
        public bool IsAnsi { get; }
        /// <summary>
        /// Fixed length 
        /// </summary>
        public bool IsFixedLength { get; }
        /// <summary>
        /// Length of the string -1 for max
        /// </summary>
        public int Length { get; }

        public DbType DbType { get; }

        public DynamicParameters GetParameters(string name, object value)
        {
            DynamicParameters parameters = new();
            if(DbType == DbType.String)
            {
                parameters.Add(name, new DbString()
                {
                    IsAnsi = IsAnsi,
                    IsFixedLength = IsFixedLength,
                    Length = Length,
                    Value = (string)value
                }, DbType);
            }
            else
            {
                parameters.Add(name, value, DbType);
            }
           
            return parameters;
        }
    }
}
