using App.DAL.EF;
using Base.Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

[ApiController]
public class ExampleController : ControllerBase
{
    //private readonly AppDbContext _context;
    /*

    public ExampleController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult> Add()
    {
        
    }
    
    [HttpPut]
    public async Task<ActionResult> Add()
    {
        
    }
    */
}

public class Item : DomainEntityId
{
    public string Name { get; set; } = default!;
}

public class HasCategory : DomainEntityId
{
    public Guid ItemId { get; set; }
    public Item? Item { get; set; }

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
}

public class Category : DomainEntityId
{
    public string CategoryName { get; set; } = default!;
}