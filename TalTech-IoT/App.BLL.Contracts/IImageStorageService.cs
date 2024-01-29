namespace App.BLL.Contracts;

public interface IImageStorageService
{
    // TODO: dto, 
    /*
     * { result: [
     *  { sequence: Int,
     *    link: iot.ttu.ee/public/images/0000-0000-0000-0001.jpg
     *  }
     *      .
     *      .
     *      .
     * ]
     * }
     */
    public Task<SaveImagesResultsDTO?> Save(SaveImagesDTO payload);

    public void Delete(DeleteImageDTO payload);
}

public class SaveImagesResultsDTO
{
    public List<SaveImageResultDTO> Results { get; set; } = default!;
}

public class SaveImageResultDTO
{
    public int Sequence { get; set; } = default!; // sequence, in which the image was in the content
    public string Link { get; set; } = default!; // Link to image
}

public class SaveImagesDTO
{
    public List<SaveImageDTO> data { get; set; }= default!;
}
public class SaveImageDTO
{
    public int Sequence { get; set; } = default!;
    public string ImageContent { get; set; } = default!;

    public string FileFormat { get; set; } = default!;
}

public class DeleteImageDTO
{
    // TODO: mõtle veel välja!
    public string ImageLink { get; set; } = default!;
}