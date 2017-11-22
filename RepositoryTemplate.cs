using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bruce.BDao
{
    public class RepositoryTemplate
    {
        // 数据库连接
        private string connectionString;

        public RepositoryTemplate(string conStr)
        {
            connectionString = conStr;
        }

        /// <summary>
        /// 生成的方法
        /// </summary>
        /// <param name="conStr">数据库连接字符串</param>
        public void Run(string conStr = null)
        {
            if (!string.IsNullOrEmpty(conStr))
            {
                connectionString = conStr;
            }
            var entities = MSSqlEntityHelper.GetEntities(connectionString);
            var identitys = MSSqlEntityHelper.GetIdentityColumns(connectionString);
            foreach (Entity entity in entities)
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder updateSql = new StringBuilder();
                string updateWhere = "";
                List<string> cols = new List<string>();
                string pk = "";
                string pkType = "";
                string className = entity.EntityName + "Repositority.cs";
                StringBuilder getCol = new StringBuilder();

                updateSql.Append("UPDATE " + entity.EntityName + " SET");
                entity.Fields.ForEach(item =>
                {
                    getCol.Append(string.Format("{0},", item.Name));
                    if (identitys.Exists(ident => { if (ident.name == item.Name && ident.object_id == item.ID) { return true; } return false; }))
                    {
                    }
                    else
                    {
                        cols.Add(item.Name);
                        if (!item.IsPk)
                        {
                            updateSql.Append(string.Format(" {0} = @{0}, ", item.Name));
                        }
                        else
                        {
                            updateWhere = string.Format(" WHERE {0} = @{0}", item.Name);
                        }
                    }
                    if (item.IsPk)
                    {
                        pk = item.Name;
                        pkType = item.Type;
                    }
                });
                updateSql.Remove(updateSql.ToString().LastIndexOf(","), 1);
                getCol.Remove(getCol.ToString().LastIndexOf(","), 1);
                updateSql.Append(updateWhere);

                sb.AppendLine("/*============================");
                sb.AppendLine("* 该文件由模板生成，请不要修改");
                sb.AppendLine("=============================*/");
                sb.AppendLine("using System;");
                sb.AppendLine("using Entity;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("namespace Repositories");
                sb.AppendLine("{");
                sb.AppendLine("       /// <summary>");
                sb.AppendLine("       /// " + entity.EntityComment);
                sb.AppendLine("       /// </summary>");
                sb.AppendLine("       public class " + entity.EntityName + "Repository:BaseRepository");
                sb.AppendLine("       {");

                string insertStr = string.Format("INSERT INTO {0} ({1}) VALUES (@{2})", entity.EntityName, string.Join(",", cols), string.Join(",@", cols));
                sb.AppendLine("           /// <summary>  ");
                sb.AppendLine("           /// 插入操作");
                sb.AppendLine("           /// <summary> ");
                sb.AppendLine("           public int Insert(" + entity.EntityName + "Entity entity)");
                sb.AppendLine("           {");
                sb.AppendLine("               string sql = \"" + insertStr + "\";");
                sb.AppendLine("               return base.ExecuteNonQuery(OpenConnection(), sql, entity);");
                sb.AppendLine("           }");

                if (!string.IsNullOrEmpty(pk))
                {
                    sb.AppendLine("           /// <summary>  ");
                    sb.AppendLine("           /// 删除操作");
                    sb.AppendLine("           /// <summary> ");
                    sb.AppendLine("           public int Delete(" + pkType + " pk)");
                    sb.AppendLine("           {");
                    sb.AppendLine("               string sql = \"DELETE FROM " + entity.EntityName + " WHERE " + pk + " = @Id\";");
                    sb.AppendLine("               return base.ExecuteNonQuery(OpenConnection(), sql, new { Id = pk });");
                    sb.AppendLine("           }");

                    sb.AppendLine("           /// <summary>  ");
                    sb.AppendLine("           /// 更新操作");
                    sb.AppendLine("           /// <summary> ");
                    sb.AppendLine("           public int Update(" + entity.EntityName + "Entity entity)");
                    sb.AppendLine("           {");
                    sb.AppendLine("               string sql = \"" + updateSql + "\";");
                    sb.AppendLine("               return base.ExecuteNonQuery(OpenConnection(), sql, entity);");
                    sb.AppendLine("           }");

                    sb.AppendLine("           /// <summary>  ");
                    sb.AppendLine("           /// 获取实体");
                    sb.AppendLine("           /// <summary> ");
                    sb.AppendLine("           public " + entity.EntityName + "Entity GetModel(" + pkType + " pk)");
                    sb.AppendLine("           {");
                    sb.AppendLine("               string sql = \"" + string.Format("SELECT {0} FROM {1} WHERE {2}=@pk", getCol, entity.EntityName, pk) + "\";");
                    sb.AppendLine("               return base.QuerySingle<" + entity.EntityName + "Entity>(OpenConnection(), sql, new { pk = pk }); ");
                    sb.AppendLine("           }");
                }

                sb.AppendLine("           /// <summary>  ");
                sb.AppendLine("           /// 获取实体列表");
                sb.AppendLine("           /// <summary> ");
                sb.AppendLine("           public List<" + entity.EntityName + "Entity> GetList()");
                sb.AppendLine("           {");
                sb.AppendLine("               string sql = \"" + string.Format("SELECT TOP 100 {0} FROM {1}", getCol, entity.EntityName) + "\";");
                sb.AppendLine("               return base.Query<" + entity.EntityName + "Entity>(OpenConnection(), sql, new {}); ");
                sb.AppendLine("           }");
                sb.AppendLine("       }");
                sb.AppendLine("}");

                Common.WriteFile("Repositories", className, sb.ToString());
                Common.Log(entity.EntityName);
            }
            CopyFile();
            CreateCsProj();
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        private void CopyFile()
        {
            try
            {
                string path = Path.Combine(AppContext.BaseDirectory, "BaseRepository.cs");
                if (File.Exists(path))
                {
                    string text = File.ReadAllText(path);
                    Common.WriteFile("Repositories", "BaseRepository.cs", text.Replace("/*", "").Replace("*/", ""));
                }
                else
                {
                    Common.Log("BaseRepository.cs Not Exist!");
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex.ToString());
            }
        }

        /// <summary>
        /// 创建项目配置文件
        /// </summary>
        private void CreateCsProj()
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine("<Project Sdk=\"Microsoft.NET.Sdk\">");
            text.AppendLine("");
            text.AppendLine("    <PropertyGroup>");
            text.AppendLine("        <TargetFramework>netcoreapp1.1</TargetFramework>");
            text.AppendLine("    </PropertyGroup>");
            text.AppendLine("");
            text.AppendLine("    <ItemGroup>");
            text.AppendLine("        <ProjectReference Include=\"..\\Entity\\Entity.csproj\" />");
            text.AppendLine("    </ItemGroup>");
            text.AppendLine("");
            text.AppendLine("    <ItemGroup> ");
            text.AppendLine("        <PackageReference Include=\"Dapper\" Version=\"1.50.2\" />");
            text.AppendLine("        <PackageReference Include=\"System.Data.SqlClient\" Version=\"4.3.0\" />");
            text.AppendLine("    </ItemGroup>");
            text.AppendLine("");
            text.AppendLine("</Project>");

            Common.WriteFile("Repositories", "Repositories.csproj", text.ToString());
        }
    }

}
