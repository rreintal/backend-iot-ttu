namespace Public.DTO.V1;

public class ProjectAllLangs
{
    public Guid Id { get; set; }
    public List<ContentDto> Title { get; set; } = default!;
    public List<ContentDto> Body { get; set; } = default!;

    public int Year { get; set; } 
    public double ProjectVolume { get; set; } 
    public string ProjectManager { get; set; } = default!;
}