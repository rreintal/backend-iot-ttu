using App.DAL.Contracts;
using Base.BLL.Contracts;
using Base.DAL.EF.Contracts;
using BLL.DTO.V1;

namespace App.BLL.Contracts;

public interface IHomePageBannerService : IBaseTranslateableRepository<HomePageBanner>, IHomePageBannerRepositoryCustom
{
    public Task<UpdateHomePageBanner?> Update(UpdateHomePageBanner entity);

    public Task<HomePageBanner> UpdateAsync(HomePageBanner entity);
}