namespace Catalog.API.Models
{
    public class Price : BaseModel
    {
        public decimal BasePrice { get; set; }
        public decimal Discount { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime EffectiveDate { get; set; }

    }
}
