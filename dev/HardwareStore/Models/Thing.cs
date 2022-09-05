using System.ComponentModel.DataAnnotations;

namespace HardwareStore.Models
{
    public class Thing
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название модели")]
        public string ModelName { get; set; }

        public int EntityId { get; set; }
        public Entity Entity { get; set; }

        public ICollection<Image> Images { get; set; }
        public ICollection<Characteristic> Characteristics { get; set; }
    }
}
