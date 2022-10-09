namespace HardwareStore.Models
{
    public class Characteristic
    {
        public int Id { get; set; }
        public string ThingId { get; set; }
        public Thing Thing { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
    }
}
