namespace HardwareStore.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int ThingId { get; set; }
        public Thing Thing { get; set; }
        
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int Quantity { get; set; } = 1;


        public override bool Equals(object item2)
        {
            if (item2 == null) return false;

            CartItem p = item2 as CartItem;
            if ((object)p == null)
            {
                return false;
            }

            return (Id == p.Id) && (ThingId == p.ThingId) && (OrderId == p.OrderId);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
