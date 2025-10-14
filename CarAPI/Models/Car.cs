namespace CarAPI.Models
{
    public class Car : Entity
    {
        public int CarBrandId { get; set; }
        public virtual CarBrand CarBrand { get; set; } = null!;
        
        public int TrimLevelId { get; set; }
        public virtual TrimLevel TrimLevel { get; set; } = null!;
    }
}