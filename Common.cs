using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bruce.BDao
{
    public class Common
    {
        /// <summary>
        /// 获取Json节点
        /// </summary>
        /// <param name="node">node1:node11</param>
        /// <returns></returns>
        public static string GetConfig(string node)
        {
            if (string.IsNullOrEmpty(node))
                return "";
            try
            {
                var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                if (File.Exists(path))
                {
                    string confStr = File.ReadAllText(path);
                    if (!string.IsNullOrEmpty(confStr))
                    {
                        return GetValueByNode(node, confStr).Trim('"');
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 获取Json节点
        /// </summary>
        /// <param name="node">node1:node11</param>
        /// <param name="text">文本内容</param>
        /// <returns></returns>
        private static string GetValueByNode(string node, string text)
        {
            if (string.IsNullOrEmpty(node))
                return "";
            string nextNode = "";
            string nodeStr = "";
            int splitIdx = node.IndexOf(":");
            if (splitIdx > 0)
            {
                nextNode = node.Substring(splitIdx + 1);
                nodeStr = $"\"{ node.Substring(0, splitIdx)}\":";

                int textIdx = text.IndexOf(nodeStr);
                text = text.Substring(textIdx + nodeStr.Length).Trim();
                if (text[0] == '{')
                {
                    int textEdcIdx = text.IndexOf("}");
                    return GetValueByNode(nextNode, text.Substring(0, textEdcIdx + 1));
                }
                else if (text[0] == '[')
                {
                    int textEdcIdx = text.IndexOf("]");
                    return text.Substring(0, textEdcIdx + 1);
                }
                else
                {
                    int textEdcIdx = text.IndexOf("\",");
                    return text.Substring(0, textEdcIdx + 1);
                }
            }
            else
            {
                nodeStr = $"\"{ node }\":";
                int textIdx = text.IndexOf(nodeStr);
                text = text.Substring(textIdx + nodeStr.Length).Trim();
                if (text[0] == '{')
                {
                    int textEdcIdx = text.IndexOf("}");
                    return text.Substring(0, textEdcIdx + 1);
                }
                else if (text[0] == '[')
                {
                    int textEdcIdx = text.IndexOf("]");
                    return text.Substring(0, textEdcIdx + 1);
                }
                else
                {
                    int textEdcIdx = text.IndexOf("\",");
                    return text.Substring(0, textEdcIdx + 1);
                }
            }

            return "";
        }

        /// <summary>
        /// 记录程序执行信息
        /// </summary>
        /// <param name="logContent"></param>
        public static void Log(string logContent)
        {
            // edit by hepw 2017-01-12 
            //var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InfoLog");
            var path = Path.Combine(AppContext.BaseDirectory, "InfoLog");
            WriteLog(path, logContent);
            Console.WriteLine(logContent);
        }

        /// <summary>
        /// 写文件方法
        /// </summary>
        /// <param name="path"></param>
        /// <param name="logContent"></param>
        private static void WriteLog(string path, string logContent)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var logName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            var txtLogUrl = Path.Combine(path, logName);
            using (var fileStream = File.Open(txtLogUrl, FileMode.Append, FileAccess.Write))
            using (var streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.WriteLine(string.Format("[{0}]-->\n\r{1}", GetDateTimeStr(), logContent));
                // edit by hepw 2017-01-12 
                streamWriter.Flush();
            }
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        private static string GetDateTimeStr()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 写文件
        /// </summary> 
        /// <param name="type">类型（Model,Repositoroes,Services）</param>
        /// <param name="file">文件名</param>
        /// <param name="text">内容</param>
        public static void WriteFile(string type, string file, string text)
        {
            try
            {
                string path = GetConfig("path");
                if (string.IsNullOrEmpty(path))
                {
                    path = Path.Combine(AppContext.BaseDirectory, type);
                }
                else
                {
                    path = Path.Combine(path, type);
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file = Path.Combine(path, file);
                if (!File.Exists(file))
                {
                    using (var sw = File.CreateText(file))
                    {
                        sw.Write(text);
                        sw.Flush();
                    }
                }
                else
                {
                    File.WriteAllText(file, text);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="type">类型（Model,Repositoroes,Services）</param>
        /// <param name="file">文件名</param>
        /// <param name="text">内容</param>
        public static void WriteFile(string path, string type, string file, string text)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = Path.Combine(AppContext.BaseDirectory, type);
                }
                else
                {
                    path = Path.Combine(path, type);
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file = Path.Combine(path, file);
                if (!File.Exists(file))
                {
                    using (var sw = File.CreateText(file))
                    {
                        sw.Write(text);
                        sw.Flush();
                    }
                }
                else
                {
                    File.WriteAllText(file, text);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }
}
