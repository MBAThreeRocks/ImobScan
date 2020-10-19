using System.Collections.Generic;
using Newtonsoft.Json;

namespace ImobScan.Entidades
{
    public partial class ZapBairro
    {
        [JsonProperty("neighborhood")]
        public Neighborhood Neighborhood { get; set; }
    }

    public partial class Neighborhood
    {
        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("maxScore")]
        public double MaxScore { get; set; }

        [JsonProperty("totalCount")]
        public long TotalCount { get; set; }

        [JsonProperty("result")]
        public Result Result { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("locations")]
        public List<Location> Locations { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("uriCategory")]
        public UriCategory UriCategory { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public partial class Address
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("zone")]
        public string Zone { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonProperty("point")]
        public Point Point { get; set; }
    }

    public partial class Point
    {
        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }
    }

    public partial class UriCategory
    {
        [JsonProperty("page")]
        public string Page { get; set; }
    }
}
