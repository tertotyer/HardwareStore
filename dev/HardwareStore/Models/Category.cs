using System.ComponentModel.DataAnnotations;

namespace HardwareStore.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EntityId { get; set; }
        public Entity Entity { get; set; }
        public ICollection<Thing> Things { get; set; }
    }
}
