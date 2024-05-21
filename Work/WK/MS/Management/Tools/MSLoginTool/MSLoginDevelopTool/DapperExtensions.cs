using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MSLoginDevelopTool
{
    public static class DapperExtensions
    {
        public static DbString ToDbString(this string value, SqlDbType sqlDbType, int length)
        {
            switch (sqlDbType)
            {
                case SqlDbType.VarChar:
                    return value.ToVarchar(length);

                case SqlDbType.NChar:
                    return value.ToNChar(length);

                case SqlDbType.Char:
                    return value.ToChar(length);

                case SqlDbType.NVarChar:
                default:
                    return value.ToNVarchar(length);
            }
        }

        public static DbString ToVarchar(this string value, int length)
        {
            if (length < -1)
            {
                throw new ArgumentOutOfRangeException();
            }

            return new DbString { Value = value, Length = length, IsAnsi = true };
        }

        public static List<DbString> ToVarchar(this List<string> values, int length)
        {
            if (length < -1)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (values == null)
            {
                return null;
            }

            return values.Select(s => s.ToVarchar(length)).ToList();
        }

        public static DbString ToChar(this string value, int length)
        {
            if (length < -1)
            {
                throw new ArgumentOutOfRangeException();
            }

            return new DbString { Value = value, Length = length, IsAnsi = true, IsFixedLength = true };
        }

        public static DbString ToNVarchar(this string value, int length)
        {
            if (length < -1)
            {
                throw new ArgumentOutOfRangeException();
            }

            return new DbString { Value = value, Length = length };
        }

        public static DbString ToNChar(this string value, int length)
        {
            if (length < -1)
            {
                throw new ArgumentOutOfRangeException();
            }

            return new DbString { Value = value, Length = length, IsFixedLength = true };
        }
    }
}