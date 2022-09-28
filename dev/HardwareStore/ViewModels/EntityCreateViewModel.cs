using HardwareStore.Models;
using System.ComponentModel.DataAnnotations;

namespace HardwareStore.ViewModels
{
    public class EntityCreateViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        public IFormFile Image { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}