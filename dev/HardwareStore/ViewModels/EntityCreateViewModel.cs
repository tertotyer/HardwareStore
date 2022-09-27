using HardwareStore.Models;

namespace HardwareStore.ViewModels
{
    public class EntityCreateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}