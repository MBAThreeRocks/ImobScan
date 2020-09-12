using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ImobScan
{
    public static class Utilidades
    {
        public static string ToSingleLine(this string html)
        {
            return html.Replace(Environment.NewLine, " ").Replace("\t","").Replace("\r","").Replace("\n","");
        }

        public static void ExportCsv<T>(List<T> genericList, string fileName)
        {
            if(File.Exists(fileName))
                File.Delete(fileName);

            var sb = new StringBuilder();
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var finalPath = Path.Combine(basePath, fileName);
            var header = "";
            var info = typeof(T).GetProperties();
            if (!File.Exists(finalPath))
            {
                var file = File.Create(finalPath);
                file.Close();
                foreach (var prop in typeof(T).GetProperties())
                {
                    header += prop.Name + "; ";
                }
                header = header.Substring(0, header.Length - 2);
                sb.AppendLine(header);
                TextWriter sw = new StreamWriter(finalPath, true, Encoding.GetEncoding("ISO-8859-1"));
                sw.Write(sb.ToString());
                sw.Close();
            }
            foreach (var obj in genericList)
            {
                sb = new StringBuilder();
                var line = "";
                foreach (var prop in info)
                {
                    line += prop.GetValue(obj, null) + "; ";
                }
                line = line.Substring(0, line.Length - 2);
                sb.AppendLine(line);
                TextWriter sw = new StreamWriter(finalPath, true);
                sw.Write(sb.ToString());
                sw.Close(); 
            }
        }
    }
}