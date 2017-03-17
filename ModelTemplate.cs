using System;
using System.Collections.Generic;
using System.Text;

namespace Bruce.BDao
{
    public class ModelTemplate
    {
        // 数据库连接
        private string connectionString;

        public ModelTemplate(string conStr)
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
            foreach (Entity entity in entities)
            {
                StringBuilder sb = new StringBuilder();
                string className = entity.EntityName + ".cs";

                sb.AppendLine(@"/*============================");
                sb.AppendLine(@"* 该文件由模板生成，请不要修改");
                sb.AppendLine(@"=============================*/");
                sb.AppendLine(@"using System;");
                sb.AppendLine(@"namespace Entity");
                sb.AppendLine(@"{");
                sb.AppendLine(@"       /// <summary>");
                sb.AppendLine(@"       /// " + entity.EntityComment);
                sb.AppendLine(@"       /// </summary>");
                sb.AppendLine(@"       public class " + entity.EntityName);
                sb.AppendLine(@"       {");

                for (int i = 0; i < entity.Fields.Count; i++)
                {
                    if (i == 0)
                    {
                        sb.AppendLine(@"           /// <summary>  ");
                        sb.AppendLine(@"           /// " + entity.Fields[i].Comment);
                        sb.AppendLine(@"           /// <summary> ");

                        if (entity.Fields[i].IsPk)
                        {

                            //sb.AppendLine(@"           [PK]");
                        }
                        sb.AppendLine(@"           public " + entity.Fields[i].Type + " " + ((entity.Fields[i].Name == "id") ? "Id" : entity.Fields[i].Name) + " { get; set; }");
                    }
                    else
                    {
                        sb.AppendLine(@"           /// <summary>  ");
                        sb.AppendLine(@"           /// " + entity.Fields[i].Comment);
                        sb.AppendLine(@"           /// <summary> ");
                        sb.AppendLine(@"           public " + entity.Fields[i].Type + " " + ((entity.Fields[i].Name == "id") ? "Id" : entity.Fields[i].Name) + "{ get; set; }");
                    }
                }
                sb.AppendLine(@"       }");
                sb.AppendLine(@"}");
                Common.WriteFile("Entity", className, sb.ToString());
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
            text.AppendLine("</Project>");

            Common.WriteFile("Entity", "Entity.csproj", text.ToString());
        }
    }
}
