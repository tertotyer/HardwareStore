namespace HardwareStore.Models
{
    public class Thing
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public bool Existence { get; set; } = true;


        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<Characteristic> Characteristics { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
