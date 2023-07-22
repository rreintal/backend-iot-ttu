using Base.DAL.EF.Contracts;

namespace App.DAL.Contracts;

public interface IAppUOW : IBaseUOW
{
    // this is app level contract, which specifies what rules the app level UOW has to follow
    // for example repositories
    
    INewsRepository NewsRepository { get; }
    
    IContentTypesRepository ContentTypesRepository { get; }

    ITopicAreaRepository TopicAreaRepository { get; }
}