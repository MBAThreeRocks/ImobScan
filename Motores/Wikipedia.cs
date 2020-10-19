using System;
using ImobScan.NetEngine;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using ImobScan.Entidades;

namespace ImobScan.Motores
{
    public static class Wikipedia
    {
        public static Action<string> ExibirMensagem;
        public static readonly string DiretorioCSV = @"C:\Users\tccun\Google Drive\Projetos\ImobScan\Arquivos\BaseBairro.csv";

        public static void GravabaseBairros()
        {
            var lstBairros = ConsultarBairrosSaoPaulo();
            
            ExibirMensagem($"Exportando { lstBairros.Count.ToString() } registros para { DiretorioCSV }");
            Utilidades.ExportCsv(lstBairros, DiretorioCSV);
        }

        public static List<Bairros> ConsultarBairrosSaoPaulo()
        {
            List<Bairros> lstBairros = new List<Bairros>();

            try
            {
                ExibirMensagem("iniciando consulta dos bairros...");
                string html = AbrirPaginaBairrosSaoPaulo();

                //Carregar Bairro
                MatchCollection regBairros = Regex.Matches(html, "<div class=\"CategoryTreeItem\">(.*?)<\\/div>");

                if(regBairros.Count < 1)
                {
                    ExibirMensagem("nenhum bairro encontrado...");
                    return lstBairros;
                }

                foreach(Match bairro in regBairros)
                {
                    var divBairro = bairro.Groups[1].Value;

                    var regNomeBairro = Regex.Match(divBairro, "Categoria:Bairros d. (.*?)\"");
                    var regUrlSubBairro = Regex.Match(divBairro, "<a href=\"(.*?)\"");
                    
                    ExibirMensagem($"gravando bairro { regNomeBairro.Groups[1].Value }...");
                    lstBairros.Add(new Bairros { Bairro = regNomeBairro.Groups[1].Value, Zona = string.Empty });
                    ExibirMensagem($"total de bairros extraídos: { lstBairros.Count }...");  
                    var htmlSubBairro = Crawler.Get("https://pt.wikipedia.org/" + regUrlSubBairro.Groups[1].Value).Result.ToSingleLine();

                    var regDivSubBairro = Regex.Match(htmlSubBairro, "<div class=\\\"mw-category\\\">(.*?)<\\/div><\\/div>");
                    
                    if(!regDivSubBairro.Success)
                    {
                        regDivSubBairro = Regex.Match(htmlSubBairro, "<div lang=\\\"pt\\\" dir=\\\"ltr\\\" class=\\\"mw-content-ltr\\\">(.*?)<\\/div><\\/div>");
                    }

                    if(!regDivSubBairro.Success)
                    {
                        continue;
                    }

                    MatchCollection regNomeSubBairro = Regex.Matches(regDivSubBairro.Groups[1].Value, "title=\"(.*?)\"");

                    foreach(Match subBairro in regNomeSubBairro)
                    {
                        var nome = subBairro.Groups[1].Value;
                        var sufixo = Regex.Match(nome, " \\((.*?)\\)"); 

                        if(sufixo.Success)
                        {
                            nome = nome.Replace(sufixo.Groups[0].Value, "");
                        }

                        if(!lstBairros.Any(x => x.Bairro == nome))  
                        {   
                            ExibirMensagem($"gravando sub-bairro { nome }...");                   
                            lstBairros.Add(new Bairros { Bairro = nome, Zona = string.Empty });
                            
                            ExibirMensagem($"total de bairros extraídos: { lstBairros.Count }"); 
                        }
                    }
                }

                ExibirMensagem("extração de bairros finalizada");
                return lstBairros;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        private static string AbrirPaginaBairrosSaoPaulo()        
        {
            string url = $"https://pt.wikipedia.org/wiki/Categoria:Bairros_de_S%C3%A3o_Paulo_(cidade)";
            string retorno = Crawler.Get(url).Result;

            return retorno.ToSingleLine();
        }   
    }
}