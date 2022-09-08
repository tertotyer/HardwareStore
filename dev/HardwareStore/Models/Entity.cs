namespace HardwareStore.Models
{
    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
