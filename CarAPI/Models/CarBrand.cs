namespace CarAPI.Models
{
    public class CarBrand : Entity
    {
        public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}