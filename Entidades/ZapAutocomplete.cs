using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ImobScan.Entidades.Json
{

    public partial class Autocomplete
    {
        [JsonProperty("street")]
        public Account Street { get; set; }

        [JsonProperty("neighborhood")]
        public Account Neighborhood { get; set; }

        [JsonProperty("city")]
        public Account City { get; set; }

        [JsonProperty("account")]
        public Account Account { get; set; }
    }

    public partial class Account
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
        [JsonProperty("advertiser", NullValueHandling = NullValueHandling.Ignore)]
        public Advertiser Advertiser { get; set; }

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
        public Zone Zone { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("state")]
        public State State { get; set; }

        [JsonProperty("neighborhood")]
        public Neighborhood Neighborhood { get; set; }

        [JsonProperty("point", NullValueHandling = NullValueHandling.Ignore)]
        public Point Point { get; set; }
    }

    public partial class Point
    {
        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }
    }

    public partial class Advertiser
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class UriCategory
    {
        [JsonProperty("page")]
        public Page Page { get; set; }
    }

    public enum Neighborhood { VilaGuarani, VilaGuaraniZSul, VlGuaraniZSul };

    public enum Source { GeoPointSourceNone };

    public enum State { Paraná, Sp, SãoPaulo };

    public enum Zone { Empty, ZonaSul };

    public enum Page { Publisher, Result };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                NeighborhoodConverter.Singleton,
                SourceConverter.Singleton,
                StateConverter.Singleton,
                ZoneConverter.Singleton,
                PageConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class NeighborhoodConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Neighborhood) || t == typeof(Neighborhood?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "VL GUARANI (Z SUL)":
                    return Neighborhood.VlGuaraniZSul;
                case "Vila Guarani":
                    return Neighborhood.VilaGuarani;
                case "Vila Guarani (Z Sul)":
                    return Neighborhood.VilaGuaraniZSul;
            }
            throw new Exception("Cannot unmarshal type Neighborhood");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Neighborhood)untypedValue;
            switch (value)
            {
                case Neighborhood.VlGuaraniZSul:
                    serializer.Serialize(writer, "VL GUARANI (Z SUL)");
                    return;
                case Neighborhood.VilaGuarani:
                    serializer.Serialize(writer, "Vila Guarani");
                    return;
                case Neighborhood.VilaGuaraniZSul:
                    serializer.Serialize(writer, "Vila Guarani (Z Sul)");
                    return;
            }
            throw new Exception("Cannot marshal type Neighborhood");
        }

        public static readonly NeighborhoodConverter Singleton = new NeighborhoodConverter();
    }

    internal class SourceConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Source) || t == typeof(Source?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "GeoPointSource_NONE")
            {
                return Source.GeoPointSourceNone;
            }
            throw new Exception("Cannot unmarshal type Source");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Source)untypedValue;
            if (value == Source.GeoPointSourceNone)
            {
                serializer.Serialize(writer, "GeoPointSource_NONE");
                return;
            }
            throw new Exception("Cannot marshal type Source");
        }

        public static readonly SourceConverter Singleton = new SourceConverter();
    }

    internal class StateConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(State) || t == typeof(State?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Paraná":
                    return State.Paraná;
                case "SP":
                    return State.Sp;
                case "São Paulo":
                    return State.SãoPaulo;
            }
            throw new Exception("Cannot unmarshal type State");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (State)untypedValue;
            switch (value)
            {
                case State.Paraná:
                    serializer.Serialize(writer, "Paraná");
                    return;
                case State.Sp:
                    serializer.Serialize(writer, "SP");
                    return;
                case State.SãoPaulo:
                    serializer.Serialize(writer, "São Paulo");
                    return;
            }
            throw new Exception("Cannot marshal type State");
        }

        public static readonly StateConverter Singleton = new StateConverter();
    }

    internal class ZoneConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Zone) || t == typeof(Zone?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "":
                    return Zone.Empty;
                case "Zona Sul":
                    return Zone.ZonaSul;
            }
            throw new Exception("Cannot unmarshal type Zone");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Zone)untypedValue;
            switch (value)
            {
                case Zone.Empty:
                    serializer.Serialize(writer, "");
                    return;
                case Zone.ZonaSul:
                    serializer.Serialize(writer, "Zona Sul");
                    return;
            }
            throw new Exception("Cannot marshal type Zone");
        }

        public static readonly ZoneConverter Singleton = new ZoneConverter();
    }

    internal class PageConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Page) || t == typeof(Page?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "PUBLISHER":
                    return Page.Publisher;
                case "RESULT":
                    return Page.Result;
            }
            throw new Exception("Cannot unmarshal type Page");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Page)untypedValue;
            switch (value)
            {
                case Page.Publisher:
                    serializer.Serialize(writer, "PUBLISHER");
                    return;
                case Page.Result:
                    serializer.Serialize(writer, "RESULT");
                    return;
            }
            throw new Exception("Cannot marshal type Page");
        }

        public static readonly PageConverter Singleton = new PageConverter();
    }
}
