using System.ComponentModel.DataAnnotations;

namespace HardwareStore.Models
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Extractor> Extractors { get; set; }
    }
}
