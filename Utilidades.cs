using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ImobScan
{
    public static class Utilidades
    {
        public static string ToSingleLine(this string html)
        {
            return html.Replace(Environment.NewLine, " ").Replace("\t","").Replace("\r","").Replace("\n","");
        }

        public static string ToSingleLine(this List<string> lista)
        {
            string linha = string.Empty;
            foreach(var item in lista)
            {
                linha += item + ", ";
            }

            return linha.Trim().TrimEnd(',');
        }

        public static string[] ReadCsv(string caminho)
        {
            using(var reader = new StreamReader(caminho))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    return values;
                }
            }

            return null;
        }
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            StreamReader sr = new StreamReader(strFilePath);
            string[] headers = sr.ReadLine().Split(';'); 
            DataTable dt = new DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ";");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
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
                //TextWriter sw = new StreamWriter(finalPath, true, Encoding.GetEncoding("ISO-8859-1"));
                TextWriter sw = new StreamWriter(finalPath, true, Encoding.UTF8);
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