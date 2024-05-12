using App.Domain;
using App.Domain.Contracts;
using Contracts;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Helpers;

public static class ImageResourcesHelper
{
    public static void HandleImageResourcesStates<T>(T newDomainObject, T existingEntity, AppDbContext dbContext)
        where T: IContainsImageResource, IDomainEntityId
    {
        if (newDomainObject.ImageResources != null)
        {
            if (existingEntity.ImageResources != null)
            {
                // Mark as deleted, because just clearing removes the NewsId but its still in the DB!
                dbContext.ImageResources.RemoveRange(existingEntity.ImageResources);
                
                
                foreach (var imageResource in newDomainObject.ImageResources)
                {
                    var item = new ImageResource()
                    {
                        NewsId = existingEntity.Id,
                        Link = imageResource.Link,
                    };
                    dbContext.Entry(item).State = EntityState.Added;
                    existingEntity.ImageResources.Add(item);
                }
            }
            else
            {
                existingEntity.ImageResources = new List<ImageResource>();
                foreach (var imageResource in newDomainObject.ImageResources)
                {
                    var item = new ImageResource()
                    {
                        NewsId = existingEntity.Id,
                        Link = imageResource.Link,
                    };
                    dbContext.Entry(item).State = EntityState.Added;
                }
            }
        }
    }
}

