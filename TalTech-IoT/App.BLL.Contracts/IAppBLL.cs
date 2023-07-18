using Base.DAL.EF.Contracts;

namespace App.BLL.Contracts;

public interface IAppBLL : IBaseUOW
{
    INewsService NewsService { get; }
}