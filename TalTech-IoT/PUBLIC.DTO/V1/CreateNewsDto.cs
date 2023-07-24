namespace Public.DTO.V1;

// TODO parem nimi create/update on sama
public class CreateNewsDto
{
    public List<ContentDto> Title { get; set; } = default!;
    public List<ContentDto> Body { get; set; } = default!;

    public string Author { get; set; } = default!;
    public string Image { get; set; } = default!;

    // TODO - uuri kuidas salvestada
    //public Byte[] Image { get; set; } = default!;
}