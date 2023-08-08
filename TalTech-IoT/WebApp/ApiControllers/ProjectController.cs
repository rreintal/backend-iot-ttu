using App.BLL.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;


/// <summary>
/// Projects controller
/// </summary>
public class ProjectController : ControllerBase
{
    private IAppBLL _bll;

    public ProjectController(IAppBLL bll)
    {
        _bll = bll;
    }

    [HttpPost]
    public async Task<int> Create(Public.DTO.V1.PostProjectDto data)
    {
        throw new NotImplementedException();
    }
}