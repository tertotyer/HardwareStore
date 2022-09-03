using System.ComponentModel.DataAnnotations;

namespace HardwareStore.Models
{
    public class Image<T> where T : class
    {
        public int Id { get; set; }

        [Display(Name = "Путь картинки")]
        public string ImagePath { get; set; }
        public string EntityName { get; set; } = typeof(T).Name;
        public int ObjectId { get; set; }
    }
}