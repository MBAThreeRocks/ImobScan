using System.Linq;

namespace ImobScan.Entidades
{
    public class ZapBase
    {
        public string IdImovel {get;set;}
        public string TipoUnidade {get;set;}
        public string TipoPropriedade {get;set;}
        public string Pais {get;set;}
        public string Cidade {get;set;}
        public string Estado {get;set;}
        public string Rua {get;set;}
        public string Local {get;set;}
        public string Bairro {get;set;}
        public string TipoUso {get;set;}
        public string AreaTotal {get;set;}
        public string AreaUtil {get;set;}
        public string TipoNegocio {get;set;}
        public string Valor {get;set;}        
        public string Vagas {get;set;}
        public string Suites {get;set;}
        public string Banheiros {get;set;}
        public string Quartos {get;set;}
        public string ValorCondominio {get;set;}

        public ZapBase()
        {

        }

        public ZapBase(Entidades.ZapAnuncio.Anuncio anuncio)
        {
            this.IdImovel = anuncio.Listing.Id;
            double areaTotal = 0;          
            if(anuncio.Listing.TotalAreas.Count > 0)
                areaTotal = anuncio.Listing.TotalAreas.Min();
            else
                areaTotal = 0;
            this.AreaTotal = areaTotal.ToString();

            double areaUtil = 0;
            if(anuncio.Listing.UsableAreas.Count > 0)
                areaUtil = anuncio.Listing.UsableAreas.Min();  
            else
                areaUtil = 0;

            this.AreaUtil = areaUtil.ToString();
            this.Bairro = anuncio.Listing.Address.Neighborhood;
            this.Cidade = anuncio.Listing.Address.City;
            this.Estado = anuncio.Listing.Address.StateAcronym;
            this.Local = anuncio.Listing.Address.LocationId;
            this.Pais = anuncio.Listing.Address.Country;

            double qtdeBanheiros = 0;
            if(anuncio.Listing.Bathrooms.Count > 0)
                qtdeBanheiros = anuncio.Listing.Bathrooms.Min();
            this.Banheiros = qtdeBanheiros.ToString();

            double qtdeQuartos = 0;
            if(anuncio.Listing.Bedrooms.Count > 0)
                qtdeQuartos = anuncio.Listing.Bedrooms.Min();
            this.Quartos = qtdeQuartos.ToString();

            double qtdeSuites = 0;
            if(anuncio.Listing.Suites.Count > 0)
                qtdeSuites = anuncio.Listing.Suites.Min();

            this.Suites = qtdeSuites.ToString();

            double qtdeVagas = 0;
            if(anuncio.Listing.ParkingSpaces.Count > 0)
                qtdeVagas = anuncio.Listing.ParkingSpaces.Min();

            this.Vagas = qtdeVagas.ToString();

            this.TipoNegocio = anuncio.Listing.PricingInfo.BusinessLabel;
            this.TipoPropriedade = anuncio.Listing.PropertyType;
            this.TipoUnidade = anuncio.Listing.UnitTypes.Count > 0? anuncio.Listing.UnitTypes[0] : string.Empty;
            this.TipoUso = anuncio.Listing.UsageTypes.Count > 0? anuncio.Listing.UsageTypes[0]: string.Empty;

            if(!string.IsNullOrEmpty(anuncio.Listing.PricingInfo.Price))
            {
                this.Valor = anuncio.Listing.PricingInfo.Price;  
            }
            else if(anuncio.Listing.PricingInfos.Count > 0 && anuncio.Listing.PricingInfos[0].Price > 0)
            {
                this.Valor = anuncio.Listing.PricingInfos[0].Price.ToString();  
            }
            else
            {
                this.Valor = "0";
            }

            this.Valor = this.Valor.Replace("R$","").Replace(".","").Trim();

            this.Rua = anuncio.Listing.Address.Street.Replace("\t", "").Trim();

            if(!string.IsNullOrEmpty(anuncio.Listing.PricingInfo.MonthlyCondoFee))
            {
                this.ValorCondominio = anuncio.Listing.PricingInfo.MonthlyCondoFee;  
            }
            else if(anuncio.Listing.PricingInfos.Count > 0 && anuncio.Listing.PricingInfos[0].MonthlyCondoFee > 0)
            {
                this.ValorCondominio = anuncio.Listing.PricingInfos[0].MonthlyCondoFee.ToString();  
            }
            else
            {
                this.ValorCondominio = "0";
            }
            
            this.ValorCondominio = this.ValorCondominio.Replace("R$","").Replace(".","").Trim();

            if(IdImovel == "2470960784")
            {
                AreaTotal = AreaTotal;
            }
        }
    }
}