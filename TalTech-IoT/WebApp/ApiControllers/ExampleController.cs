using App.DAL.EF;
using App.DAL.EF.Example;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1")]
public class ExampleController : ControllerBase
{
    private AppDbContext _ctx { get; set; }

    public ExampleController(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet("{id}")]
    public async Task<Person?> Get(Guid id)
    {
        return await _ctx.Persons
            .Include(e => e.Phones)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
    
    [HttpPost]
    public async Task<Person?> Add([FromBody] Person person)
    {
        return (await _ctx.AddAsync(person)).Entity;
    }

    [HttpPut]
    public Person Update(Person person)
    {
        return _ctx.Update(person).Entity;
    }
    
    
    
}