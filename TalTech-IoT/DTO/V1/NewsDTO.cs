namespace DTO.V1;

// TODO parem nimi create/update on sama
public class NewsDTO
{
    public Guid? Id { get; set; }
    public List<CultureDto> Title { get; set; } = default!;
    public List<CultureDto> Body { get; set; } = default!;
    
    // TODO - uuri kuidas salvestada
    //public Byte[] Image { get; set; } = default!;
}