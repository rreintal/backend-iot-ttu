using Base.DAL.EF.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class EFBaseUOW<TDContext> : IBaseUOW
    where TDContext : DbContext
{
    protected readonly TDContext UowDbContext;
    
    public EFBaseUOW(TDContext uowDbContext)
    {
        UowDbContext = uowDbContext;
    }
    
    public virtual async Task<int> SaveChangesAsync()
    {
        return await UowDbContext.SaveChangesAsync();
    }
}