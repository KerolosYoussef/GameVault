namespace Catalog.API.Models
{
    public class Warehouse : BaseModel
    {
        public Warehouse(string name, string? description, string address, decimal capacity)
        {
            Name = name;
            Description = description;
            Address = address;
            Capacity = capacity;
        }
        public Warehouse() { }

        public string Name { get; set; }
        public string? Description { get; set; }
        public string Address { get; set; }
        public decimal Capacity { get; set; }
        public List<Product> Products { get; set; } = [];
    }
}
