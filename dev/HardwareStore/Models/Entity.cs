namespace HardwareStore.Models
{
    public class Entity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
