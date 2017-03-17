using System;
using System.Text;

namespace Bruce.BDao
{
    class Program
    {
        static void Main(string[] args)
        {
            Common.Log("Strat");
            string connStr = Common.GetConfig("sqlconn");
            if (string.IsNullOrEmpty(connStr))
            {
                Common.Log("Can't Find Config");
                return;
            }
            Common.Log("Start Generate Models...");
            ModelTemplate model = new ModelTemplate(connStr);
            model.Run();
            Common.Log("Start Generate Repository...");
            RepositoryTemplate resp = new RepositoryTemplate(connStr);
            resp.Run();
            Common.Log("Start Generate Service...");
            ServiceTemplate service = new ServiceTemplate(connStr);
            service.Run();
            CreateSln();
            Common.Log("Generate Success in " + Common.GetConfig("path"));
            Common.Log("End");
        }

        /// <summary>
        /// 生成解决方案文件（不完善版）
        /// </summary>
        private static void CreateSln1()
        {
            StringBuilder slnStr = new StringBuilder();
            slnStr.AppendLine("Microsoft Visual Studio Solution File, Format Version 12.00");
            slnStr.AppendLine("# Visual Studio 15");
            slnStr.AppendLine("VisualStudioVersion = 15.0.26228.4");
            slnStr.AppendLine("MinimumVisualStudioVersion = 15.0.26124.0");
            slnStr.AppendLine("Global");
            slnStr.AppendLine("    GlobalSection(SolutionConfigurationPlatforms) = preSolution");
            slnStr.AppendLine("        Debug | Any CPU = Debug | Any CPU");
            slnStr.AppendLine("        Debug | x64 = Debug | x64");
            slnStr.AppendLine("        Debug | x86 = Debug | x86");
            slnStr.AppendLine("        Release | Any CPU = Release | Any CPU");
            slnStr.AppendLine("        Release | x64 = Release | x64");
            slnStr.AppendLine("        Release | x86 = Release | x86");
            slnStr.AppendLine("    EndGlobalSection");
            slnStr.AppendLine("    GlobalSection(SolutionProperties) = preSolution");
            slnStr.AppendLine("        HideSolutionNode = FALSE");
            slnStr.AppendLine("    EndGlobalSection");
            slnStr.AppendLine("EndGlobal");
            Common.WriteFile("", "Slution.sln", slnStr.ToString());
        }

        /// <summary>
        /// 生成解决方案文件
        /// </summary>
        private static void CreateSln()
        {
            StringBuilder slnStr = new StringBuilder();
            slnStr.AppendLine("");

            slnStr.AppendLine("Microsoft Visual Studio Solution File, Format Version 12.00");
            slnStr.AppendLine("# Visual Studio 15");
            slnStr.AppendLine("VisualStudioVersion = 15.0.26228.4");
            slnStr.AppendLine("MinimumVisualStudioVersion = 15.0.26124.0");
            slnStr.AppendLine("Project(\"{9A19103F-16F7-4668-BE54-9A1E7A4F7556}\") = \"Entity\", \"Entity\\Entity.csproj\", \"{E3916901-D917-4FE2-A127-06679AC67EB3}\"");
            slnStr.AppendLine("EndProject");
            slnStr.AppendLine("Project(\"{9A19103F-16F7-4668-BE54-9A1E7A4F7556}\") = \"Repositories\", \"Repositories\\Repositories.csproj\", \"{0C5A504C-DB52-419B-8175-D00B6FF32704}\"");
            slnStr.AppendLine("EndProject");
            slnStr.AppendLine("Project(\"{9A19103F-16F7-4668-BE54-9A1E7A4F7556}\") = \"Services\", \"Services\\Services.csproj\", \"{B525163E-B182-4EF8-9553-131B55B7F7C4}\"");
            slnStr.AppendLine("EndProject");
            slnStr.AppendLine("Global");
            slnStr.AppendLine("    GlobalSection(SolutionConfigurationPlatforms) = preSolution");
            slnStr.AppendLine("        Debug|Any CPU = Debug|Any CPU");
            slnStr.AppendLine("        Debug|x64 = Debug|x64");
            slnStr.AppendLine("        Debug|x86 = Debug|x86");
            slnStr.AppendLine("        Release|Any CPU = Release|Any CPU");
            slnStr.AppendLine("        Release|x64 = Release|x64");
            slnStr.AppendLine("        Release|x86 = Release|x86");
            slnStr.AppendLine("    EndGlobalSection");
            slnStr.AppendLine("    GlobalSection(ProjectConfigurationPlatforms) = postSolution");
            slnStr.AppendLine("        {E3916901-D917-4FE2-A127-06679AC67EB3}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
            slnStr.AppendLine("        {E3916901-D917-4FE2-A127-06679AC67EB3}.Debug|Any CPU.Build.0 = Debug|Any CPU");
            slnStr.AppendLine("        {E3916901-D917-4FE2-A127-06679AC67EB3}.Debug|x64.ActiveCfg = Debug|Any CPU");
            slnStr.AppendLine("        {E3916901-D917-4FE2-A127-06679AC67EB3}.Debug|x64.Build.0 = Debug|Any CPU");
            slnStr.AppendLine("        {E3916901-D917-4FE2-A127-06679AC67EB3}.Debug|x86.ActiveCfg = Debug|Any CPU");
            slnStr.AppendLine("        {E3916901-D917-4FE2-A127-06679AC67EB3}.Debug|x86.Build.0 = Debug|Any CPU");
            slnStr.AppendLine("        {E3916901-D917-4FE2-A127-06679AC67EB3}.Release|Any CPU.ActiveCfg = Release|Any CPU");
            slnStr.AppendLine("        {E3916901-D917-4FE2-A127-06679AC67EB3}.Release|Any CPU.Build.0 = Release|Any CPU");
            slnStr.AppendLine("        {E3916901-D917-4FE2-A127-06679AC67EB3}.Release|x64.ActiveCfg = Release|Any CPU");
            slnStr.AppendLine("        {E3916901-D917-4FE2-A127-06679AC67EB3}.Release|x64.Build.0 = Release|Any CPU");
            slnStr.AppendLine("        {E3916901-D917-4FE2-A127-06679AC67EB3}.Release|x86.ActiveCfg = Release|Any CPU");
            slnStr.AppendLine("        {E3916901-D917-4FE2-A127-06679AC67EB3}.Release|x86.Build.0 = Release|Any CPU");
            slnStr.AppendLine("        {0C5A504C-DB52-419B-8175-D00B6FF32704}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
            slnStr.AppendLine("        {0C5A504C-DB52-419B-8175-D00B6FF32704}.Debug|Any CPU.Build.0 = Debug|Any CPU");
            slnStr.AppendLine("        {0C5A504C-DB52-419B-8175-D00B6FF32704}.Debug|x64.ActiveCfg = Debug|Any CPU");
            slnStr.AppendLine("        {0C5A504C-DB52-419B-8175-D00B6FF32704}.Debug|x64.Build.0 = Debug|Any CPU");
            slnStr.AppendLine("        {0C5A504C-DB52-419B-8175-D00B6FF32704}.Debug|x86.ActiveCfg = Debug|Any CPU");
            slnStr.AppendLine("        {0C5A504C-DB52-419B-8175-D00B6FF32704}.Debug|x86.Build.0 = Debug|Any CPU");
            slnStr.AppendLine("        {0C5A504C-DB52-419B-8175-D00B6FF32704}.Release|Any CPU.ActiveCfg = Release|Any CPU");
            slnStr.AppendLine("        {0C5A504C-DB52-419B-8175-D00B6FF32704}.Release|Any CPU.Build.0 = Release|Any CPU");
            slnStr.AppendLine("        {0C5A504C-DB52-419B-8175-D00B6FF32704}.Release|x64.ActiveCfg = Release|Any CPU");
            slnStr.AppendLine("        {0C5A504C-DB52-419B-8175-D00B6FF32704}.Release|x64.Build.0 = Release|Any CPU");
            slnStr.AppendLine("        {0C5A504C-DB52-419B-8175-D00B6FF32704}.Release|x86.ActiveCfg = Release|Any CPU");
            slnStr.AppendLine("        {0C5A504C-DB52-419B-8175-D00B6FF32704}.Release|x86.Build.0 = Release|Any CPU");
            slnStr.AppendLine("        {B525163E-B182-4EF8-9553-131B55B7F7C4}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
            slnStr.AppendLine("        {B525163E-B182-4EF8-9553-131B55B7F7C4}.Debug|Any CPU.Build.0 = Debug|Any CPU");
            slnStr.AppendLine("        {B525163E-B182-4EF8-9553-131B55B7F7C4}.Debug|x64.ActiveCfg = Debug|Any CPU");
            slnStr.AppendLine("        {B525163E-B182-4EF8-9553-131B55B7F7C4}.Debug|x64.Build.0 = Debug|Any CPU");
            slnStr.AppendLine("        {B525163E-B182-4EF8-9553-131B55B7F7C4}.Debug|x86.ActiveCfg = Debug|Any CPU");
            slnStr.AppendLine("        {B525163E-B182-4EF8-9553-131B55B7F7C4}.Debug|x86.Build.0 = Debug|Any CPU");
            slnStr.AppendLine("        {B525163E-B182-4EF8-9553-131B55B7F7C4}.Release|Any CPU.ActiveCfg = Release|Any CPU");
            slnStr.AppendLine("        {B525163E-B182-4EF8-9553-131B55B7F7C4}.Release|Any CPU.Build.0 = Release|Any CPU");
            slnStr.AppendLine("        {B525163E-B182-4EF8-9553-131B55B7F7C4}.Release|x64.ActiveCfg = Release|Any CPU");
            slnStr.AppendLine("        {B525163E-B182-4EF8-9553-131B55B7F7C4}.Release|x64.Build.0 = Release|Any CPU");
            slnStr.AppendLine("        {B525163E-B182-4EF8-9553-131B55B7F7C4}.Release|x86.ActiveCfg = Release|Any CPU");
            slnStr.AppendLine("        {B525163E-B182-4EF8-9553-131B55B7F7C4}.Release|x86.Build.0 = Release|Any CPU");
            slnStr.AppendLine("    EndGlobalSection");
            slnStr.AppendLine("    GlobalSection(SolutionProperties) = preSolution");
            slnStr.AppendLine("        HideSolutionNode = FALSE");
            slnStr.AppendLine("    EndGlobalSection");
            slnStr.AppendLine("EndGlobal");
            Common.WriteFile("", "Slution.sln", slnStr.ToString());
        }
    }
}