using System;
using System.Collections.Generic;
using System.Text;

namespace Bruce.BDao
{
    public class ServiceTemplate
    {
        // 数据库连接
        private string connectionString;

        public ServiceTemplate(string conStr)
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
                string pk = "";
                string pkType = "";
                string className = entity.EntityName + "Service.cs";
                entity.Fields.ForEach(item =>
                {
                    if (item.IsPk)
                    {
                        pk = item.Name;
                        pkType = item.Type;
                    }
                });

                sb.AppendLine("/*============================");
                sb.AppendLine("* 该文件由模板生成，请不要修改");
                sb.AppendLine("=============================*/");
                sb.AppendLine("using System;");
                sb.AppendLine("using Entity;");
                sb.AppendLine("using Repositories;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("namespace Services");
                sb.AppendLine("{");
                sb.AppendLine("       /// <summary>");
                sb.AppendLine("       /// " + entity.EntityComment);
                sb.AppendLine("       /// </summary>");
                sb.AppendLine("       public class " + entity.EntityName + "Service");
                sb.AppendLine("       {");
                sb.AppendLine("           private readonly " + entity.EntityName + "Repository _repository;");
                sb.AppendLine("           public " + entity.EntityName + "Service()");
                sb.AppendLine("           {");
                sb.AppendLine("               _repository = new " + entity.EntityName + "Repository();");
                sb.AppendLine("           }");

                sb.AppendLine("           /// <summary>  ");
                sb.AppendLine("           /// 插入操作");
                sb.AppendLine("           /// <summary> ");
                sb.AppendLine("           public int Insert(" + entity.EntityName + "Entity entity)");
                sb.AppendLine("           {");
                sb.AppendLine("               return _repository.Insert(entity);");
                sb.AppendLine("           }");

                if (!string.IsNullOrEmpty(pk))
                {
                    sb.AppendLine("           /// <summary>  ");
                    sb.AppendLine("           /// 删除操作");
                    sb.AppendLine("           /// <summary> ");
                    sb.AppendLine("           public int Delete(" + pkType + " pk)");
                    sb.AppendLine("           {");
                    sb.AppendLine("               return _repository.Delete(pk);");
                    sb.AppendLine("           }");

                    sb.AppendLine("           /// <summary>  ");
                    sb.AppendLine("           /// 更新操作");
                    sb.AppendLine("           /// <summary> ");
                    sb.AppendLine("           public int Update(" + entity.EntityName + "Entity entity)");
                    sb.AppendLine("           {");
                    sb.AppendLine("               return _repository.Update(entity);");
                    sb.AppendLine("           }");

                    sb.AppendLine("           /// <summary>  ");
                    sb.AppendLine("           /// 获取实体");
                    sb.AppendLine("           /// <summary> ");
                    sb.AppendLine("           public " + entity.EntityName + "Entity GetModel(" + pkType + " pk)");
                    sb.AppendLine("           {");
                    sb.AppendLine("               return _repository.GetModel(pk);");
                    sb.AppendLine("           }");
                }

                sb.AppendLine("           /// <summary>  ");
                sb.AppendLine("           /// 获取实体列表");
                sb.AppendLine("           /// <summary> ");
                sb.AppendLine("           public List<" + entity.EntityName + "Entity> GetList()");
                sb.AppendLine("           {");
                sb.AppendLine("               return _repository.GetList();");
                sb.AppendLine("           }");
                sb.AppendLine("       }");
                sb.AppendLine("}");

                Common.WriteFile("Services", className, sb.ToString());
                Common.Log(entity.EntityName);
            }
            CreateCsProj();
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
            text.AppendLine("        <ProjectReference Include=\"..\\Repositories\\Repositories.csproj\" />");
            text.AppendLine("    </ItemGroup>");
            text.AppendLine("");
            text.AppendLine("</Project>");

            Common.WriteFile("Services", "Services.csproj", text.ToString());
        }
    }
}
