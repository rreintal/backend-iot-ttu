using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;

namespace App.BLL.Services;

public class FeedPageService : BaseEntityService<FeedPage, Domain.FeedPage, IFeedPageRepository>, IFeedPageService
{
    private IAppUOW _uow;
    public FeedPageService(IAppUOW Uow, IMapper<FeedPage, Domain.FeedPage> mapper) : base(Uow.FeedPageRepository, mapper)
    {
        _uow = Uow;
    }

    public async Task<IEnumerable<FeedPage>> AllAsync(string? languageCulture)
    {
        return (await _uow.FeedPageRepository.AllAsync(languageCulture)).Select(e => Mapper.Map(e));
    }

    public async Task<FeedPage?> FindAsync(Guid id, string? languageCulture)
    {
        return Mapper.Map(await _uow.FeedPageRepository.FindAsync(id, languageCulture));
    }

    public async Task<FeedPage?> FindAsyncByName(string identifier)
    {
        return Mapper.Map(await _uow.FeedPageRepository.FindAsyncByName(identifier));
    }
}