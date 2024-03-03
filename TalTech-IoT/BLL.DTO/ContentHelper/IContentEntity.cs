using BLL.DTO.Contracts;

namespace BLL.DTO.ContentHelper;

public interface IContentEntity
{
    public List<BLL.DTO.V1.Content> Content { get; set; }
}