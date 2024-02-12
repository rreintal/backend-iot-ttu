using App.BLL.Contracts;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;
using FeedPageCategory = App.Domain.FeedPageCategory;

namespace App.BLL.Services;

public class FeedPageCategoryService : BaseEntityService<global::BLL.DTO.V1.FeedPageCategory, FeedPageCategory, IFeedPageCategoryRepository>, IFeedPageCategoryService
{
    private IAppUOW _uow;
    private IMapper _mapper;
    public FeedPageCategoryService(IAppUOW uow, IMapper<global::BLL.DTO.V1.FeedPageCategory, FeedPageCategory> mapper, IMapper mapper1) : base(uow.FeedPageCategoryRepository, mapper)
    {
        _uow = uow;
        _mapper = mapper1;
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

    public async Task<List<global::BLL.DTO.V1.FeedPageCategory>> GetCategoryWithoutPosts(Guid feedPageId, string languageCulture)
    {

        return (await _uow.FeedPageCategoryRepository.GetFeedPageCategoryWithoutPosts(feedPageId, languageCulture))
            .Select(e => _mapper.Map<global::BLL.DTO.V1.FeedPageCategory>(e)).ToList();
    }
}