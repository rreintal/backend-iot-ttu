using BLL.DTO.V1;

namespace BLL.DTO.Contracts;

public interface IContainsImageResource
{
    public List<ImageResource> ImageResources { get; set; }
}