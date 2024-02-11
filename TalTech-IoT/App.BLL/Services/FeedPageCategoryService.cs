using App.BLL.Contracts;
using App.DAL.Contracts;
using App.Domain;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class FeedPageCategoryService : BaseEntityService<global::BLL.DTO.V1.FeedPageCategory, FeedPageCategory, IFeedPageCategoryRepository>, IFeedPageCategoryService
{
    private IAppUOW _uow;
    public FeedPageCategoryService(IAppUOW uow, IMapper<global::BLL.DTO.V1.FeedPageCategory, FeedPageCategory> mapper) : base(uow.FeedPageCategoryRepository, mapper)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<global::BLL.DTO.V1.FeedPageCategory>> AllAsync(string? languageCulture)
    {
        return (await _uow.FeedPageCategoryRepository.AllAsync(languageCulture)).Select(e => Mapper.Map(e));
    }

    public async Task<global::BLL.DTO.V1.FeedPageCategory?> FindAsync(Guid id, string? languageCulture)
    {
        return Mapper.Map(await _uow.FeedPageCategoryRepository.FindAsync(id, languageCulture));
    }

    public async Task<bool> DoesCategoryHavePostsAsync(Guid id)
    {
        return await _uow.FeedPageCategoryRepository.DoesCategoryHavePostsAsync(id);
    }

    public async Task<global::BLL.DTO.V1.FeedPageCategory> UpdateAsync(global::BLL.DTO.V1.FeedPageCategory entity)
    {
        
        var domainObject = Mapper.Map(entity);
        var domainResult = await _uow.FeedPageCategoryRepository.UpdateAsync(domainObject);
        return Mapper.Map(domainResult)!;
    }
}