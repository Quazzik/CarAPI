namespace CarAPI.Models
{
    public class TrimLevel : Entity
    {
        public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}