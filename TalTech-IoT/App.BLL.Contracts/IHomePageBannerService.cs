using Base.BLL.Contracts;
using Base.DAL.EF.Contracts;
using BLL.DTO.V1;

namespace App.BLL.Contracts;

public interface IHomePageBannerService : IBaseTranslateableRepository<HomePageBanner>
{
    public Task<UpdateHomePageBanner?> Update(UpdateHomePageBanner entity);
}