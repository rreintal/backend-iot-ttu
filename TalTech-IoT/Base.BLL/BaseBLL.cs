using Base.BLL.Contracts;
using Base.DAL.EF.Contracts;

namespace Base.BLL;

public abstract class BaseBLL<TUOW> : IBaseBLL
where TUOW : IBaseUOW
{
    protected readonly TUOW Uow;
    
    public BaseBLL(TUOW uow)
    {
        Uow = uow;
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await Uow.SaveChangesAsync();
    }
}