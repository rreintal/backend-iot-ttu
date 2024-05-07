using App.DAL.Contracts;
using Base.BLL.Contracts;
using Base.DAL.EF.Contracts;
using BLL.DTO.V1;
using Public.DTO;

namespace App.BLL.Contracts;

public interface INewsService : ITranslateableEntityService<News>, INewsRepositoryCustom<App.Domain.News> // App.Domain.News mby sketchy
{
    public Task<List<ContentType>> GetContentTypes();
    
    public Task<IEnumerable<News>> AllAsyncFiltered(NewsFilterSet filterSet, string languageCulture);
    public Task<News?> UpdateAsync(News entity);

    public Task<News> AddAsync(News entity);

    public Task<News> RemoveAsync(News entity);

    public Task<News?> FindByIdAllTranslationsAsync(Guid id);
    
}