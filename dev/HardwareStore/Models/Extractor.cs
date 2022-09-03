using System.ComponentModel.DataAnnotations;

namespace HardwareStore.Models
{
    public class Extractor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Тип вытяжки")]
        public string Type{ get; set; }

        [Display(Name = "Высота")] // General
        public string Height { get; set; }

        [Display(Name = "Глубина")] // General
        public float Depth { get; set; }

        [Display(Name = "Датчик температуры")]
        public bool TempSensor { get; set; }

        [Display(Name = "Диаметр воздуховода")] // General
        public float DuctDiametr { get; set; }

        [Display(Name = "Жироподавляющий фильтр")]
        public string GreaseFilter { get; set; }

        [Display(Name = "Индикатор загрязнения филтров")]
        public bool FilterPollutionIndicator { get; set; }

        [Display(Name = "Индикация")]
        public bool Indication { get; set; }

        [Display(Name = "Ионизация")]
        public bool Ionisation { get; set; }

        [Display(Name = "Количество ламп")] // General
        public int AmountLamps { get; set; }

        [Display(Name = "Количество моторов")]
        public int AmountMotors { get; set; }

        [Display(Name = "Количество скоростей")]
        public int AmountSpeeds { get; set; }

        [Display(Name = "Конструкция")]
        public string Construction { get; set; }

        [Display(Name = "Максимальная производительность мотора")]
        public int MaxMotorProd { get; set; }

        [Display(Name = "Максимальный уровень шума")]
        public int MaxNoiseLvl { get; set; }

        [Display(Name = "Материал корпуса")]
        public string BodyMaterial { get; set; }

        [Display(Name = "Минимальный уровень шума")]
        public int MinNoiseLvl { get; set; }

        [Display(Name = "Монтаж")]
        public string Mounting { get; set; }

        [Display(Name = "Мощность")] // General
        public int Power { get; set; }

        [Display(Name = "Мощность мотора")]
        public int MotorPower { get; set; }

        [Display(Name = "Периметральное воздухопоглощение")]
        public bool PerimAirAbsorp { get; set; }

        [Display(Name = "Плавная регулировка яркости (диммер)")]
        public bool SmoothBrightControl { get; set; }

        [Display(Name = "Постоянная вентиляция")]
        public bool ConstantVentilation { get; set; }

        [Display(Name = "Сенсор влажности и задымленности")]
        public bool WetSensor { get; set; }

        [Display(Name = "Стиль")]
        public string Style { get; set; }

        [Display(Name = "Таймер автоотключения")]
        public bool TimeAutoShutdown { get; set; }

        [Display(Name = "Тип освещения")]
        public string LightingType { get; set; }

        [Display(Name = "Угольный фильтр (в комплекте)")]
        public string CoalFilter { get; set; }

        [Display(Name = "Управление")]
        public string Control { get; set; }

        [Display(Name = "Цвет")]
        public string Color { get; set; }

        [Display(Name = "Ширина")] // General
        public float Width { get; set; }

        public int EntityId { get; set; }
    }
}