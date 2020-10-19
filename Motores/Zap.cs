using System;
using ImobScan.NetEngine;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ImobScan.Entidades;
using System.Threading;
using System.IO;

namespace ImobScan.Motores
{
    public static class Zap
    {        
        public static Action<string> ExibirMensagem;

        private static readonly string Diretorio = @"C:\Users\tccun\Google Drive\Projetos\ImobScan\Arquivos\html.txt";
        public static readonly string DiretorioCSVBase = @"C:\Users\tccun\Google Drive\Projetos\ImobScan\Arquivos\Base.csv";
        public static readonly string DiretorioCSVBaseBairro = @"C:\Users\tccun\Google Drive\Projetos\ImobScan\Arquivos\Bairros";
        public static readonly string DiretorioCSVBairro = @"C:\Users\tccun\Google Drive\Projetos\ImobScan\Arquivos\BaseBairroZap.csv";

        public static void UnificarArquivos()
        {
            List<ZapBase> lstImoveis = new List<ZapBase>();
            ExibirMensagem("Carregando bairros de São Paulo...");
            var lstBairrosExt = CarregarBairrosArquivo();

            foreach(var bairro in lstBairrosExt)
            {
                if(VerificarBaseExtraida(bairro.Bairro))
                {      
                    ExibirMensagem($"Unificando base de {bairro.Bairro}");
                    var tbBase = Utilidades.ConvertCSVtoDataTable(DiretorioCSVBaseBairro + "\\" + bairro.Bairro + ".csv");
                    foreach(DataRow itemBase in tbBase.Rows)
                    {              
                        lstImoveis.Add(new ZapBase{
                            AreaTotal = itemBase[" AreaTotal"].ToString(),
                            AreaUtil = itemBase[" AreaUtil"].ToString(),
                            Bairro = itemBase[" Bairro"].ToString(),
                            Banheiros = itemBase[" Banheiros"].ToString(),
                            CEP = itemBase[" CEP"].ToString(),
                            Cidade = itemBase[" Cidade"].ToString(),
                            Descricao = itemBase[" Descricao"].ToString(),
                            Estado = itemBase[" Estado"].ToString(),
                            IdImovel = itemBase["IdImovel"].ToString(),
                            Local = itemBase[" Local"].ToString(),
                            Pais = itemBase[" Pais"].ToString(),
                            Pros = itemBase[" Pros"].ToString(),
                            Quartos = itemBase[" Quartos"].ToString(),
                            Rua = itemBase[" Rua"].ToString(),
                            Suites = itemBase[" Suites"].ToString(),
                            TipoNegocio = itemBase[" TipoNegocio"].ToString(),
                            TipoPropriedade = itemBase[" TipoPropriedade"].ToString(),
                            TipoUnidade = itemBase[" TipoUnidade"].ToString(),
                            TipoUso = itemBase[" TipoUso"].ToString(),
                            Vagas = itemBase[" Vagas"].ToString(),
                            Valor = itemBase[" Valor"].ToString(),
                            ValorCondominio = itemBase[" ValorCondominio"].ToString()                            
                        });
                    }
                }
            }

            ExibirMensagem($"Exportando { lstImoveis.Count.ToString() } registros para { DiretorioCSVBase }");
            Utilidades.ExportCsv(lstImoveis, DiretorioCSVBase);
        }

        public static void ClassificarBairro()
        {
            List<Bairros> lstBairros = new List<Bairros>();
            var tbBairros = Utilidades.ConvertCSVtoDataTable(Wikipedia.DiretorioCSV);

            ExibirMensagem($"Iniciando a classificação de { tbBairros.Rows.Count } bairros...");
            int contador = 1;
            foreach(DataRow bairro in tbBairros.Rows)
            {
                
                ExibirMensagem($"Consultando bairro: { bairro["Bairro"] }");
                string sugBairro = ConsultarAutocomplete(bairro["Bairro"].ToString());

                var bairros = CarregarBairros(sugBairro);

                var objBairro = bairros.Neighborhood.Result.Locations.FirstOrDefault(x => x.Address.Neighborhood == bairro["Bairro"].ToString());

                if(objBairro == null)
                {
                    ExibirMensagem($"não encontrado");
                    continue;
                }
                
                ExibirMensagem($"encontrado");
                lstBairros.Add(new Bairros { 
                    Bairro = objBairro.Address.Neighborhood, 
                    Zona = objBairro.Address.Zone,
                    Latitude = objBairro.Address.Point.Lat.ToString(),
                    LocationID = objBairro.Address.LocationId,
                    Longitude = objBairro.Address.Point.Lon.ToString(),
                    Url = objBairro.Url
                    });
                
                
                ExibirMensagem($"[{ contador }]/[{ tbBairros.Rows.Count }]");
                contador++;
            }

            ExibirMensagem($"Exportando { lstBairros.Count.ToString() } registros para { DiretorioCSVBairro }");
            Utilidades.ExportCsv(lstBairros, DiretorioCSVBairro);
        }

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
                int tentativas = 0;

                while(true)
                {
                    html = AbrirPaginaCidadeSaoPaulo(i);

                    if(Regex.Match(html, "ERRO 400").Success || Regex.Match(html, "Nossos servidores estão indisponíveis").Success)
                    {
                        ExibirMensagem($"Opa! deu erro 400, vou tentar mais uma vez...");
                        tentativas++;
                        if(tentativas > 2)
                        {
                        //salvar em arquivo para conferencia
                        System.IO.File.WriteAllText(Diretorio, html);
                        break;
                        }

                        continue;
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
                    tentativas = 0;
                }
                
                ExibirMensagem($"Exportando { lstImoveis.Count.ToString() } registros para { DiretorioCSVBase }");
                Utilidades.ExportCsv(lstImoveis, DiretorioCSVBase);

                return "Sucesso";
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }

        private static List<Bairros> CarregarBairrosArquivo()
        {            
            List<Bairros> lstBairros = new List<Bairros>();
            var tbBairros = Utilidades.ConvertCSVtoDataTable(DiretorioCSVBairro);

            foreach(DataRow bairro in tbBairros.Rows)
            {
                if(!bairro[" LocationID"].ToString().Contains("Sao Paulo"))
                {
                    continue;
                }

                lstBairros.Add(new Bairros{
                    Bairro = bairro["Bairro"].ToString(),
                    Latitude = bairro[" Latitude"].ToString(),
                    LocationID = bairro[" LocationID"].ToString(),
                    Longitude = bairro[" Longitude"].ToString(),
                    Url = bairro[" Url"].ToString(),
                    Zona = bairro[" Zona"].ToString()
                });
            }

            return lstBairros;
        }

        private static bool VerificarBaseExtraida(string bairro)
        {
            return File.Exists(DiretorioCSVBaseBairro + "\\" + bairro + ".csv");
        }

        public static string ConsultarImoveisPorBairros()
        {
            try
            {
                ExibirMensagem("Consultando Bairros de São Paulo...");
                var lstBairros = CarregarBairrosArquivo();

                if(lstBairros.Count < 1)
                {                    
                    ExibirMensagem("Nenhum bairro encontrado.");
                    return string.Empty;
                }
                
                ExibirMensagem($"Encontrados { lstBairros.Count.ToString() } bairros em São Paulo");
                
                foreach(var bairro in lstBairros)
                {
                    if(VerificarBaseExtraida(bairro.Bairro))
                    {
                        ExibirMensagem($"Base já extraída para {bairro.Bairro}...");
                        continue;
                    }

                    ExibirMensagem($"iniciando consulta dos imóveis em {bairro.Bairro}...");
                    string html = AbrirPaginaBairro(bairro);
                   
                    var lstImoveis = CarregarImoveis(html);

                    int i =2;
                    int tentativas = 0;

                    while(true)
                    {                        
                        ExibirMensagem($"Consultando página {i} do bairro {bairro.Bairro}");
                        html = AbrirPaginaBairro(bairro,i);

                        if(Regex.Match(html, "ERRO 400").Success || Regex.Match(html, "Nossos servidores estão indisponíveis").Success)
                        {
                            ExibirMensagem($"Opa! deu erro 400, vou tentar mais uma vez...");
                            tentativas++;
                            if(tentativas > 2)
                            {
                                //salvar em arquivo para conferencia
                                System.IO.File.WriteAllText(Diretorio, html);
                                break;
                            }

                            continue;
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
                        tentativas = 0;
                    }
                
                    ExibirMensagem($"Exportando { lstImoveis.Count.ToString() } registros...");
                    Utilidades.ExportCsv(lstImoveis, DiretorioCSVBaseBairro + "\\" + bairro.Bairro + ".csv");
                }

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

        private static Entidades.ZapBairro CarregarBairros(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Entidades.ZapBairro>(json);
            }
            catch(Exception ex)
            {
                return new ZapBairro();
            }        
        }

        private static string ConsultarAutocomplete(string bairro)
        {            
            string urlZap = $"https://glue-api.zapimoveis.com.br/v3/locations/?businessType=SALE&fields=neighborhood&includeFields=address.street%2Caddress.neighborhood%2Caddress.city%2Caddress.state%2Caddress.zone%2Caddress.locationId%2Caddress.point%2Curl%2Cadvertiser.name%2CuriCategory.page&listingType=USED&portal=ZAP&size=6&unitTypes=UnitType_NONE&q={ bairro }";
            
            Random r = new Random();
            int rInt = r.Next(800, 3000); //for ints
            Thread.Sleep(rInt);

            string retorno = Crawler.Get(urlZap).Result;
            //string retorno = Crawler.GerProxy(urlZap);

            return retorno.ToSingleLine();
        }

        private static string AbrirPaginaBairro(Bairros bairro, int idxPagina = 1)        
        {
            string urlZap = $"https://www.zapimoveis.com.br{ bairro.Url.Trim() }/?__zt=srl%3Ab&pagina={ idxPagina }&onde=,S%C3%A3o%20Paulo,S%C3%A3o%20Paulo,{bairro.Zona.Trim()},{bairro.Bairro.Trim()},,,neighborhood,{bairro.LocationID.Trim()},{bairro.Latitude.Trim()},{bairro.Longitude.Trim()}&transacao=Venda&tipo=Im%C3%B3vel%20usado";
            
            Random r = new Random();
            int rInt = r.Next(1500, 4000); 
            Thread.Sleep(rInt);

            string retorno = Crawler.Get(urlZap).Result;
            //string retorno = Crawler.GerProxy(urlZap);

            return retorno.ToSingleLine();
        }
        
        private static string AbrirPaginaCidadeSaoPaulo(int idxPagina = 1)        
        {
            ExibirMensagem($"consultando pagina { idxPagina.ToString() } da cidade de São Paulo...");
            string urlZap = $"https://www.zapimoveis.com.br/venda/imoveis/sp+sao-paulo/?__zt=srl%3Ab&pagina={ idxPagina.ToString() }&transacao=Venda&tipo=Im%C3%B3vel%20usado";
            string retorno = Crawler.Get(urlZap).Result;

            return retorno.ToSingleLine();
        }
    }
}