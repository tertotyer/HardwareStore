using System.ComponentModel.DataAnnotations;

namespace HardwareStore.Models
{
    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TitleId { get; set; }
        public Title Title { get; set; }
        public ICollection<Thing> Things { get; set; }
    }
}
