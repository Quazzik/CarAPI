namespace CarAPI.DTOs
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CarBrandId { get; set; }
        public string CarBrandName { get; set; } = string.Empty;
        public int TrimLevelId { get; set; }
        public string TrimLevelName { get; set; } = string.Empty;
    }

    public class CreateCarDto
    {
        public string Name { get; set; } = string.Empty;
        public int CarBrandId { get; set; }
        public int TrimLevelId { get; set; }
    }
}