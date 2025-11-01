namespace CarWebApp.Models
{
    public class EntityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CreateEntityDto
    {
        public string Name { get; set; } = string.Empty;
    }
}