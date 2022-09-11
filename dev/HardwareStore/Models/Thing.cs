using System.ComponentModel.DataAnnotations;

namespace HardwareStore.Models
{
    public class Thing
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Image> Images { get; set; }
        public ICollection<Characteristic> Characteristics { get; set; }
    }
}
