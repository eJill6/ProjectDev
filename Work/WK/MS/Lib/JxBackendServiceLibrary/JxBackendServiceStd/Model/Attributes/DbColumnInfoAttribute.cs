using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace JxBackendService.Model.Attributes
{
    public class DbColumnInfoAttribute : Attribute
    {
        private readonly SqlParameter _sqlParameter = new SqlParameter();

        public DbColumnInfoAttribute(DbType dbType)
        {
            _sqlParameter.DbType = dbType;
        }

        public DbColumnInfoAttribute(SqlDbType sqlDbType) : this(sqlDbType, 0, 0, 0) { }

        public DbColumnInfoAttribute(SqlDbType sqlDbType, int size) : this(sqlDbType, size, 0, 0) { }

        public DbColumnInfoAttribute(SqlDbType sqlDbType, byte precision, byte scale) : this(sqlDbType, 0, precision, scale) { }

        public DbColumnInfoAttribute(SqlDbType sqlDbType, int size, byte precision, byte scale)
        {
            SqlDbType = sqlDbType;
            Size = size;
            Precision = precision;
            Scale = scale;
        }

        public SqlDbType SqlDbType { get; set; }
        public int Size { get => _sqlParameter.Size; set => _sqlParameter.Size = value; }
        public byte Precision { get => _sqlParameter.Precision; set => _sqlParameter.Precision = value; }
        public byte Scale { get => _sqlParameter.Scale; set => _sqlParameter.Scale = value; }

        public DbType DbType => _sqlParameter.DbType;
    }

    public class NVarcharColumnInfoAttribute : DbColumnInfoAttribute
    {
        public NVarcharColumnInfoAttribute(int size) : base(SqlDbType.NVarChar, size, 0, 0) { }
    }

    public class NCharColumnInfoAttribute : DbColumnInfoAttribute
    {
        public NCharColumnInfoAttribute(int size) : base(SqlDbType.NChar, size, 0, 0) { }
    }

    public class VarcharColumnInfoAttribute : DbColumnInfoAttribute
    {
        public VarcharColumnInfoAttribute(int size) : base(SqlDbType.VarChar, size, 0, 0) { }
    }

    public class CharColumnInfoAttribute : DbColumnInfoAttribute
    {
        public CharColumnInfoAttribute(int size) : base(SqlDbType.Char, size, 0, 0) { }
    }
}
