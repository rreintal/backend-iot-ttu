using App.Domain;
using Base.DAL.EF.Contracts;

namespace App.DAL.Contracts;

public interface IPartnerImageRepository : IBaseRepository<PartnerImage>
{
    
}

public interface IPartnerImageRepositoryCustom<TEntity>
{
    
}