using System.ComponentModel.DataAnnotations;

namespace HardwareStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "Поле Имя обязательно к заполнению")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя не может быть < 2 символов")]
        [Display(Name = "Имя")]
        public string NameBuyer { get; set; }

        [Required(ErrorMessage = "Поле Email обязательно к заполнению")]
        [EmailAddress(ErrorMessage = "Пожалуйста, введите корректный email адресс")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле Номер телефона обязательно к заполнению")]
        [Phone(ErrorMessage = "Введите корректный номер телефона")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "{0} должен быть не меньше {2} символов в длину.")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Способ доставки")]
        public string DeliveryMethod { get; set; }
        public ICollection<Thing> Things { get; set; } = new List<Thing>();

    }
}
