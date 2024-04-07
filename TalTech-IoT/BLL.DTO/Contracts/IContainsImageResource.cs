using BLL.DTO.V1;

namespace BLL.DTO.Contracts;

public interface IContainsImageResource
{
    public List<ImageResource> ImageResources { get; set; }
}

public interface IContainsOneImageResource
{
    public ImageResource ImageResources { get; set; }
}