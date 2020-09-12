using System;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json;

namespace ImobScan.Entidades.ZapAnuncio
{
    public partial class Anuncio
    {
        [JsonProperty("listing")]
        public Listing Listing { get; set; }
    }

    public partial class Listing
    {
        [JsonProperty("displayAddressType")]
        public string DisplayAddressType { get; set; }

        [JsonProperty("amenities")]
        public List<object> Amenities { get; set; }

        [JsonProperty("usableAreas")]
        public List<long> UsableAreas { get; set; }

        [JsonProperty("constructionStatus")]
        public string ConstructionStatus { get; set; }

        [JsonProperty("listingType")]
        public string ListingType { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("floors")]
        public List<object> Floors { get; set; }

        [JsonProperty("unitTypes")]
        public List<string> UnitTypes { get; set; }

        [JsonProperty("nonActivationReason")]
        public string NonActivationReason { get; set; }

        [JsonProperty("providerId")]
        public string ProviderId { get; set; }

        [JsonProperty("propertyType")]
        public string PropertyType { get; set; }

        [JsonProperty("unitSubTypes")]
        public List<object> UnitSubTypes { get; set; }

        [JsonProperty("unitsOnTheFloor")]
        public long UnitsOnTheFloor { get; set; }

        [JsonProperty("legacyId")]
        public long? LegacyId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("portal")]
        public string Portal { get; set; }

        [JsonProperty("unitFloor")]
        public long UnitFloor { get; set; }

        [JsonProperty("parkingSpaces")]
        public List<long> ParkingSpaces { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("suites")]
        public List<long> Suites { get; set; }

        [JsonProperty("publicationType")]
        public string PublicationType { get; set; }

        [JsonProperty("externalId")]
        public string ExternalId { get; set; }

        [JsonProperty("bathrooms")]
        public List<long> Bathrooms { get; set; }

        [JsonProperty("usageTypes")]
        public List<string> UsageTypes { get; set; }

        [JsonProperty("totalAreas")]
        public List<long> TotalAreas { get; set; }

        [JsonProperty("advertiserId")]
        public Guid AdvertiserId { get; set; }

        [JsonProperty("advertiserContact")]
        public AdvertiserContact AdvertiserContact { get; set; }

        [JsonProperty("whatsappNumber")]
        public string WhatsappNumber { get; set; }

        [JsonProperty("bedrooms")]
        public List<long> Bedrooms { get; set; }

        [JsonProperty("acceptExchange")]
        public bool AcceptExchange { get; set; }

        [JsonProperty("pricingInfos")]
        public List<PricingInfoElement> PricingInfos { get; set; }

        [JsonProperty("showPrice")]
        public bool ShowPrice { get; set; }

        [JsonProperty("resale")]
        public bool Resale { get; set; }

        [JsonProperty("buildings")]
        public long Buildings { get; set; }

        [JsonProperty("capacityLimit")]
        public List<object> CapacityLimit { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("hasAddress")]
        public bool HasAddress { get; set; }

        [JsonProperty("isDevelopment")]
        public bool IsDevelopment { get; set; }

        [JsonProperty("isInactive")]
        public bool IsInactive { get; set; }

        [JsonProperty("isDefaulterInactive")]
        public bool IsDefaulterInactive { get; set; }

        [JsonProperty("pricingInfo")]
        public PurplePricingInfo PricingInfo { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("businessTypeContext")]
        public string BusinessTypeContext { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("videos")]
        public List<object> Videos { get; set; }

        [JsonProperty("videoTour")]
        public List<object> VideoTour { get; set; }

        [JsonProperty("preview")]
        public bool Preview { get; set; }

        [JsonProperty("showPhoneButton")]
        public bool ShowPhoneButton { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("isSpecialRent")]
        public bool IsSpecialRent { get; set; }
    }

    public partial class Address
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("zipCode")]
        public string ZipCode { get; set; }

        [JsonProperty("geoJson")]
        public string GeoJson { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("precision")]
        public string Precision { get; set; }

        [JsonProperty("confidence")]
        public string Confidence { get; set; }

        [JsonProperty("stateAcronym")]
        public string StateAcronym { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("ibgeCityId")]
        public string IbgeCityId { get; set; }

        [JsonProperty("zone")]
        public string Zone { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonProperty("poisList")]
        public List<string> PoisList { get; set; }

        [JsonProperty("complement")]
        public string Complement { get; set; }

        [JsonProperty("pois")]
        public List<object> Pois { get; set; }

        [JsonProperty("valuableZones")]
        public List<ValuableZone> ValuableZones { get; set; }

        [JsonProperty("streetNumber")]
        public string StreetNumber { get; set; }
    }

    public partial class ValuableZone
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("zone")]
        public string Zone { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }

    public partial class AdvertiserContact
    {
        [JsonProperty("chat")]
        public string Chat { get; set; }

        [JsonProperty("phones")]
        public List<string> Phones { get; set; }

        [JsonProperty("emails")]
        public List<string> Emails { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class PurplePricingInfo
    {
        [JsonProperty("monthlyCondoFee")]
        public string MonthlyCondoFee { get; set; }

        [JsonProperty("period")]
        public string Period { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("rentalPrice")]
        public string RentalPrice { get; set; }

        [JsonProperty("rentalTotalPrice")]
        public string RentalTotalPrice { get; set; }

        [JsonProperty("salePrice")]
        public string SalePrice { get; set; }

        [JsonProperty("showPrice")]
        public bool ShowPrice { get; set; }

        [JsonProperty("yearlyIptu")]
        public string YearlyIptu { get; set; }

        [JsonProperty("priceVariation")]
        public object PriceVariation { get; set; }

        [JsonProperty("businessType")]
        public string BusinessType { get; set; }

        [JsonProperty("businessLabel")]
        public string BusinessLabel { get; set; }

        [JsonProperty("businessDescription")]
        public string BusinessDescription { get; set; }

        [JsonProperty("isSale")]
        public bool IsSale { get; set; }

        [JsonProperty("isRent")]
        public bool IsRent { get; set; }
    }

    public partial class PricingInfoElement
    {
        [JsonProperty("yearlyIptu")]
        public long YearlyIptu { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("businessType")]
        public string BusinessType { get; set; }

        [JsonProperty("monthlyCondoFee")]
        public long MonthlyCondoFee { get; set; }
    }
}
