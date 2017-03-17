using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Bruce.BDao
{
    public class MSSqlEntityHelper
    {
        /// <summary>
        /// 获取自增id
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param> 
        /// <returns></returns>
        public static List<IedntityColumn> GetIdentityColumns(string connectionString)
        {
            var list = new List<IedntityColumn>();

            var sql = @"SELECT object_id,name,column_id,seed_value FROM sys.identity_columns ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //int id = 0;
                        //if (int.TryParse(reader["object_id"].ToString(), out id))
                        //{
                        //    list.Add(id);
                        //}
                        list.Add(new IedntityColumn()
                        {
                            object_id = reader["object_id"].ToString(),
                            name = reader["name"].ToString(),
                            column_id = reader["column_id"].ToString(),
                            seed_value = reader["seed_value"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取表和列的实体数据
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static List<Entity> GetEntities(string connectionString)
        {
            var list = new List<Entity>();

            var sql = @"SELECT T.*,ep.value  AS COLUMN_COMMENT,tp.name DATA_TYPE FROM (SELECT 'dbo' TABLE_SCHEMA, tab.object_id ID,tab.name TABLE_NAME,col.name COLUMN_NAME,col.colid COLUMN_ID,col.isnullable IS_NULLABLE,col.colstat COLUMN_KEY,'' TABLE_COMMENT,col.xtype FROM sys.syscolumns col LEFT JOIN sys.tables tab ON col.id = tab.object_id WHERE tab.object_id IS NOT NULL) AS T
LEFT JOIN sys.extended_properties ep ON ep.major_id = T.ID AND ep.minor_id = T.COLUMN_ID LEFT JOIN sys.types tp ON T.xtype = tp.user_type_id ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ID = reader["ID"].ToString();
                        var db = reader["TABLE_SCHEMA"].ToString();
                        var table = reader["TABLE_NAME"].ToString();
                        var column = reader["COLUMN_NAME"].ToString();
                        var type = reader["DATA_TYPE"].ToString();
                        var comment = reader["COLUMN_COMMENT"].ToString();
                        var entity = list.FirstOrDefault(x => x.EntityName == table);
                        bool isNull = reader["IS_NULLABLE"].ToString() == "1" ? true : false;
                        bool isPk = reader["COLUMN_KEY"].ToString() == "1" ? true : false;
                        var tableComment = reader["TABLE_COMMENT"].ToString();
                        if (entity == null)
                        {
                            entity = new Entity(table, tableComment);
                            entity.Fields.Add(new Field
                            {
                                ID = ID,
                                Name = column,
                                Type = SqlTypeString2CsharpTypeString(type),
                                Comment = comment,
                                IsNull = isNull,
                                IsPk = isPk
                            });

                            list.Add(entity);
                        }
                        else
                        {
                            entity.Fields.Add(new Field
                            {
                                ID = ID,
                                Name = column,
                                Type = SqlTypeString2CsharpTypeString(type),
                                Comment = comment,
                                IsNull = isNull,
                                IsPk = isPk
                            });
                        }
                    }
                }
            }
            return list;
        }

        public static string GetCLRType(string dbType)
        {
            switch (dbType)
            {
                case "bigint":
                    return "long";
                case "tinyint":
                case "smallint":
                case "mediumint":
                case "int":
                case "integer":
                    return "int";
                case "double":
                    return "double";
                case "float":
                    return "float";
                case "decimal":
                    return "decimal";
                case "numeric":
                case "real":
                    return "decimal";
                case "bit":
                    //return "UInt64";
                    return "bool";
                case "date":
                case "time":
                case "year":
                case "datetime":
                case "timestamp":
                    return "DateTime";
                case "tinyblob":
                case "blob":
                case "mediumblob":
                case "longblog":
                case "binary":
                case "varbinary":
                    return "byte[]";
                case "char":
                case "varchar":
                case "tinytext":
                case "text":
                case "mediumtext":
                case "longtext":
                    return "string";
                case "point":
                case "linestring":
                case "polygon":
                case "geometry":
                case "multipoint":
                case "multilinestring":
                case "multipolygon":
                case "geometrycollection":
                case "enum":
                case "set":
                default:
                    return dbType;
            }
        }

        /// <summary>
        /// 数据库中与c#中的数据类型对照
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string changetocsharptype(string type)
        {
            string reval = string.Empty;
            switch (type.ToLower())
            {
                case "int":
                    reval = "int32";
                    break;
                case "text":
                    reval = "string";
                    break;
                case "bigint":
                    reval = "int64";
                    break;
                case "binary":
                    reval = "system.byte[]";
                    break;
                case "bit":
                    reval = "boolean";
                    break;
                case "char":
                    reval = "string";
                    break;
                case "datetime":
                    reval = "system.datetime";
                    break;
                case "decimal":
                    reval = "system.decimal";
                    break;
                case "float":
                    reval = "system.double";
                    break;
                case "image":
                    reval = "system.byte[]";
                    break;
                case "money":
                    reval = "system.decimal";
                    break;
                case "nchar":
                    reval = "string";
                    break;
                case "ntext":
                    reval = "string";
                    break;
                case "numeric":
                    reval = "system.decimal";
                    break;
                case "nvarchar":
                    reval = "string";
                    break;
                case "real":
                    reval = "system.single";
                    break;
                case "smalldatetime":
                    reval = "system.datetime";
                    break;
                case "smallint":
                    reval = "int16";
                    break;
                case "smallmoney":
                    reval = "system.decimal";
                    break;
                case "timestamp":
                    reval = "system.datetime";
                    break;
                case "tinyint":
                    reval = "system.byte";
                    break;
                case "uniqueidentifier":
                    reval = "system.guid";
                    break;
                case "varbinary":
                    reval = "system.byte[]";
                    break;
                case "varchar":
                    reval = "string";
                    break;
                case "variant":
                    reval = "object";
                    break;
                default:
                    reval = "string";
                    break;
            }
            return reval;
        }

        #region
        // sql server中的数据类型，转换为C#中的类型类型
        public static Type SqlTypeString2CsharpType(string sqlTypeString)
        {
            SqlDbType dbTpe = SqlTypeString2SqlType(sqlTypeString);

            return SqlType2CsharpType(dbTpe);
        }

        // SqlDbType转换为C#数据类型
        public static Type SqlType2CsharpType(SqlDbType sqlType)
        {
            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    return typeof(Int64);
                case SqlDbType.Binary:
                    return typeof(Object);
                case SqlDbType.Bit:
                    return typeof(Boolean);
                case SqlDbType.Char:
                    return typeof(String);
                case SqlDbType.DateTime:
                    return typeof(DateTime);
                case SqlDbType.Decimal:
                    return typeof(Decimal);
                case SqlDbType.Float:
                    return typeof(Double);
                case SqlDbType.Image:
                    return typeof(Object);
                case SqlDbType.Int:
                    return typeof(Int32);
                case SqlDbType.Money:
                    return typeof(Decimal);
                case SqlDbType.NChar:
                    return typeof(String);
                case SqlDbType.NText:
                    return typeof(String);
                case SqlDbType.NVarChar:
                    return typeof(String);
                case SqlDbType.Real:
                    return typeof(Single);
                case SqlDbType.SmallDateTime:
                    return typeof(DateTime);
                case SqlDbType.SmallInt:
                    return typeof(Int16);
                case SqlDbType.SmallMoney:
                    return typeof(Decimal);
                case SqlDbType.Text:
                    return typeof(String);
                case SqlDbType.Timestamp:
                    return typeof(Object);
                case SqlDbType.TinyInt:
                    return typeof(Byte);
                case SqlDbType.Udt://自定义的数据类型
                    return typeof(Object);
                case SqlDbType.UniqueIdentifier:
                    return typeof(Object);
                case SqlDbType.VarBinary:
                    return typeof(Object);
                case SqlDbType.VarChar:
                    return typeof(String);
                case SqlDbType.Variant:
                    return typeof(Object);
                case SqlDbType.Xml:
                    return typeof(Object);
                default:
                    return null;
            }
        }

        #endregion

        // 将sql server中的数据类型，转化为C#中的类型的字符串
        public static string SqlTypeString2CsharpTypeString(string sqlTypeString)
        {
            Type type = SqlTypeString2CsharpType(sqlTypeString);

            return type.Name;
        }

        // sql server数据类型（如：varchar）
        // 转换为SqlDbType类型
        public static SqlDbType SqlTypeString2SqlType(string sqlTypeString)
        {
            SqlDbType dbType = SqlDbType.Variant;//默认为Object

            switch (sqlTypeString)
            {
                case "int":
                    dbType = SqlDbType.Int;
                    break;
                case "varchar":
                    dbType = SqlDbType.VarChar;
                    break;
                case "bit":
                    dbType = SqlDbType.Bit;
                    break;
                case "datetime":
                    dbType = SqlDbType.DateTime;
                    break;
                case "decimal":
                    dbType = SqlDbType.Decimal;
                    break;
                case "float":
                    dbType = SqlDbType.Float;
                    break;
                case "image":
                    dbType = SqlDbType.Image;
                    break;
                case "money":
                    dbType = SqlDbType.Money;
                    break;
                case "ntext":
                    dbType = SqlDbType.NText;
                    break;
                case "nvarchar":
                    dbType = SqlDbType.NVarChar;
                    break;
                case "smalldatetime":
                    dbType = SqlDbType.SmallDateTime;
                    break;
                case "smallint":
                    dbType = SqlDbType.SmallInt;
                    break;
                case "text":
                    dbType = SqlDbType.Text;
                    break;
                case "bigint":
                    dbType = SqlDbType.BigInt;
                    break;
                case "binary":
                    dbType = SqlDbType.Binary;
                    break;
                case "char":
                    dbType = SqlDbType.Char;
                    break;
                case "nchar":
                    dbType = SqlDbType.NChar;
                    break;
                case "numeric":
                    dbType = SqlDbType.Decimal;
                    break;
                case "real":
                    dbType = SqlDbType.Real;
                    break;
                case "smallmoney":
                    dbType = SqlDbType.SmallMoney;
                    break;
                case "sql_variant":
                    dbType = SqlDbType.Variant;
                    break;
                case "timestamp":
                    dbType = SqlDbType.Timestamp;
                    break;
                case "tinyint":
                    dbType = SqlDbType.TinyInt;
                    break;
                case "uniqueidentifier":
                    dbType = SqlDbType.UniqueIdentifier;
                    break;
                case "varbinary":
                    dbType = SqlDbType.VarBinary;
                    break;
                case "xml":
                    dbType = SqlDbType.Xml;
                    break;
            }
            return dbType;
        }
    }

    public class Entity
    {
        public Entity()
        {
            this.Fields = new List<Field>();
        }

        public Entity(string name, string comment)
            : this()
        {
            this.EntityName = name;
            this.EntityComment = comment;
        }

        public string EntityComment { get; set; }
        public string EntityName { get; set; }
        public List<Field> Fields { get; set; }
    }

    public class Field
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Comment { get; set; }
        public bool IsNull { get; set; }
        public bool IsPk { get; set; }
    }

    public class IedntityColumn
    {
        public string object_id { get; set; }
        public string name { get; set; }
        public string column_id { get; set; }
        public string seed_value { get; set; }
    }
}
