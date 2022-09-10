using System.ComponentModel.DataAnnotations;

namespace HardwareStore.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int ThingId { get; set; }
        public Thing Thing { get; set; }
    }
}