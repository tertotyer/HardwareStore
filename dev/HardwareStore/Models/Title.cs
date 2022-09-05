namespace HardwareStore.Models
{
    public class Title
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Entity> Entities { get; set; }
    }
}
