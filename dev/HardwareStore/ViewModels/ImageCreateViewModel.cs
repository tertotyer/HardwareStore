using HardwareStore.Models;
using System.ComponentModel.DataAnnotations;

namespace HardwareStore.ViewModels
{
    public class ImageCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        public IFormFile Image { get; set; }
        public int ThingId { get; set; }
        public Thing Thing { get; set; }

    }
}
