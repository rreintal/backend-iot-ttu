using App.Domain;
using Base.DAL.EF.Contracts;
using DAL.DTO.V1;

namespace App.DAL.Contracts;

public interface IHomePageBannerRepository : IBaseTranslateableRepository<HomePageBanner>
{
    public Task<HomePageBanner> Update(UpdateHomePageBanner entity);
}