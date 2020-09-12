using Newtonsoft.Json;

namespace ImobScan.Entidades.ZapAnuncio
{
    public partial class Temperatures
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("loading")]
        public bool Loading { get; set; }
    }

    public partial class Pagination
    {
        [JsonProperty("currentPage")]
        public long CurrentPage { get; set; }

        [JsonProperty("pageSize")]
        public long PageSize { get; set; }

        [JsonProperty("listingsSize")]
        public long ListingsSize { get; set; }

        [JsonProperty("pageCount")]
        public long PageCount { get; set; }

        [JsonProperty("totalCount")]
        public long TotalCount { get; set; }

        [JsonProperty("totalCounts")]
        public TotalCounts TotalCounts { get; set; }
    }

    public partial class TotalCounts
    {
        [JsonProperty("search")]
        public long Search { get; set; }

        [JsonProperty("developments")]
        public long Developments { get; set; }

        [JsonProperty("superPremium")]
        public long SuperPremium { get; set; }
    }
}