using System;
using ImobScan.NetEngine;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ImobScan.Motores
{
    public static class Zap
    {        
        public static Action<string> ExibirMensagem;

        private static readonly string Diretorio = @"C:\Users\tccun\Google Drive\Projetos\ImobScan\Arquivos\html.txt";
        private static readonly string DiretorioCSV = @"C:\Users\tccun\Google Drive\Projetos\ImobScan\Arquivos\Base.csv";

        public static string ConsultarImoveis()
        {
            try
            {
                ExibirMensagem("iniciando consulta dos imóveis...");
                string html = AbrirPaginaCidadeSaoPaulo();

                //Carregar informações de paginação da consulta
                var regPaginacao = Regex.Match(html, "pagination\":{(.*?)}},\"loading\":false}");

                if(!regPaginacao.Success)
                {
                    return string.Empty;
                }

                string txtPaginacao = "{" + regPaginacao.Groups[1].Value + "}}";
                var paginacao = JsonConvert.DeserializeObject<Entidades.ZapAnuncio.Pagination>(txtPaginacao);

                var lstImoveis = CarregarImoveis(html);

                int i =2;
                while(true)
                {
                    html = AbrirPaginaCidadeSaoPaulo(i);

                    if(Regex.Match(html, "ERRO 400").Success)
                    {
                        //salvar em arquivo para conferencia
                        System.IO.File.WriteAllText(Diretorio, html);
                        break;
                    }

                    var lista = CarregarImoveis(html);

                    if(lista == null || lista.Count < 1)
                    {
                        //salvar em arquivo para conferencia
                        System.IO.File.WriteAllText(Diretorio, html);
                        break;
                    }

                    lstImoveis.AddRange(lista);
                    i++;
                }
                /*
                for(int i=2; i < paginacao.PageCount; i++)
                {
                    html = AbrirPaginaCidadeSaoPaulo(i);
                    lstImoveis.AddRange(CarregarImoveis(html));
                }
                */
                
                ExibirMensagem($"Exportando { lstImoveis.Count.ToString() } registros para { DiretorioCSV }");
                Utilidades.ExportCsv(lstImoveis, DiretorioCSV);

                return "Sucesso";
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }

        private static List<Entidades.ZapBase> CarregarImoveis(string html)
        {
            List<Entidades.ZapBase> lstAnuncios = new List<Entidades.ZapBase>();

            MatchCollection regAnuncios = Regex.Matches(html, "\"listing\":{\"displayAddressType\"(.*?)\"isSpecialRent\":.*?}");

            if(regAnuncios.Count < 1)
            {
                return null;
            }

            foreach(Match anuncio in regAnuncios)
            {
                string txtAnuncio = "{" + anuncio.Value + "}";
                lstAnuncios.Add(new Entidades.ZapBase( JsonConvert.DeserializeObject<Entidades.ZapAnuncio.Anuncio>(txtAnuncio)));
            }

            return lstAnuncios;
        }

        private static string AbrirPaginaVilaGuarani(int idxPagina = 1)        
        {
            string urlZap = $"https://www.zapimoveis.com.br/venda/apartamentos/sp+sao-paulo+zona-sul+vl-guarani/?__zt=srl%3Ab&pagina={ idxPagina.ToString() }&onde=,S%C3%A3o%20Paulo,S%C3%A3o%20Paulo,Zona%20Sul,Vila%20Guarani,,,,BR%3ESao%20Paulo%3ENULL%3ESao%20Paulo%3EZona%20Sul%3EVila%20Guarani,-23.6388918,-46.636921&transacao=Venda&tipo=Im%C3%B3vel%20usado&tipoUnidade=Residencial,Apartamento";
            
            string retorno = Crawler.Get(urlZap).Result;

            return retorno.ToSingleLine();
        }
        private static string AbrirPaginaCidadeSaoPaulo(int idxPagina = 1)        
        {
            ExibirMensagem($"consultando pagina { idxPagina.ToString() } da cidade de São Paulo...");
            string urlZap = $"https://www.zapimoveis.com.br/venda/imoveis/sp+sao-paulo/?__zt=srl%3Ab&pagina={ idxPagina.ToString() }&onde=,S%C3%A3o%20Paulo,S%C3%A3o%20Paulo,,,,,city,BR%3ESao%20Paulo%3ENULL%3ESao%20Paulo,-23.6026684,-46.9194693&transacao=Venda&tipo=Im%C3%B3vel%20usado";
            
            string retorno = Crawler.Get(urlZap).Result;

            return retorno.ToSingleLine();
        }
    }
}