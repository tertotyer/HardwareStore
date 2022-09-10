using HardwareStore.Models;
using Microsoft.Build.Framework;

namespace HardwareStore.ViewModels
{
    public class ImageCreateViewModel
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
        public int ThingId { get; set; }
        public Thing Thing { get; set; }

    }
}
