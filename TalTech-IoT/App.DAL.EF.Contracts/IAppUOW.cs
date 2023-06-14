using Base.DAL.EF.Contracts;

namespace App.DAL.EF.Contracts;

public interface IAppUOW : IBaseUOW
{
    // this is app level contract, which specifies what rules the app level UOW has to follow
    // for example repositories
}