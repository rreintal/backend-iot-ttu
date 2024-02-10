using App.Domain;
using Base.DAL.EF.Contracts;
using DAL.DTO.V1;

namespace App.DAL.Contracts;

public interface IHomePageBannerRepository : IBaseTranslateableRepository<HomePageBanner>, IHomePageBannerRepositoryCustom
{
    public Task<HomePageBanner> UpdateAsync(HomePageBanner entity);
}

public interface IHomePageBannerRepositoryCustom
{
    public Task UpdateSequenceBulkAsync(List<HomePageBannerSequence> data);
}