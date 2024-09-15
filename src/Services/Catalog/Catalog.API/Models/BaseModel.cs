using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.API.Models
{
    public class BaseModel
    {
        [Column(Order = 0)]
        public Guid Id { get; set; }
        [Column(Order = 98)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column(Order = 99)]
        public DateTime? UpdatedAt { get; set; }
        [Column(Order = 100)]
        public DateTime? DeletedAt { get; set; }
    }
}
