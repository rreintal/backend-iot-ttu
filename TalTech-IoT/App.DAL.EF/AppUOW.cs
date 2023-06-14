using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : EFBaseUOW<AppDbContext>
{
    public AppUOW(AppDbContext uowDbContext) : base(uowDbContext)
    {
        
    }
}