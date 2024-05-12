namespace App.Domain.Contracts;

public interface IContainsImageResource
{
    public ICollection<ImageResource>? ImageResources { get; set; }
}