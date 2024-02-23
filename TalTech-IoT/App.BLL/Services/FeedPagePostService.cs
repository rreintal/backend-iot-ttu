using App.BLL.Contracts;
using App.DAL.Contracts;
using App.Domain;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class FeedPagePostService : BaseEntityService<global::BLL.DTO.V1.FeedPagePost,FeedPagePost, IFeedPagePostRepository>, IFeedPagePostService
{
    private readonly IAppUOW _uow;
    public FeedPagePostService(IAppUOW uow, IMapper<global::BLL.DTO.V1.FeedPagePost, FeedPagePost> mapper) : base(uow.FeedPagePostRepository, mapper)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<global::BLL.DTO.V1.FeedPagePost>> AllAsync(string? languageCulture)
    {
        return (await _uow.FeedPagePostRepository.AllAsync(languageCulture)).Select(e => Mapper.Map(e));
    }

    public async Task<global::BLL.DTO.V1.FeedPagePost?> FindAsync(Guid id, string? languageCulture)
    {
        return Mapper.Map(await _uow.FeedPagePostRepository.FindAsync(id, languageCulture));
    }

    public async Task<global::BLL.DTO.V1.FeedPagePost> UpdateAsync(global::BLL.DTO.V1.FeedPagePost entity)
    {
        var domainObject = Mapper.Map(entity);
        var domainResult = await _uow.FeedPagePostRepository.UpdateAsync(domainObject);
        return Mapper.Map(domainResult)!;
    }
}