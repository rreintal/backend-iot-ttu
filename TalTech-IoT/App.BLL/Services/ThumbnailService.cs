using App.BLL.Contracts;
using SkiaSharp;

namespace App.BLL.Services;

public class ThumbnailService : IThumbnailService
{
    public string Compress(string base64ImageData)
    {
        // Extract the image data from the base64 string
        string imageData = base64ImageData.Split(',')[1]; // Remove the "data:image/png;base64," prefix

        // Convert the base64 image data back to bytes
        byte[] imageBytes = Convert.FromBase64String(imageData);

        // Create an SKBitmap from the image bytes
        using (var stream = new MemoryStream(imageBytes))
        using (var originalBitmap = SKBitmap.Decode(stream))
        {
            // Set the size of the thumbnail
            int thumbnailWidth = 250;
            int thumbnailHeight = 250;

            // Create a new SKBitmap for the thumbnail
            using (var thumbnailBitmap = originalBitmap.Resize(new SKImageInfo(thumbnailWidth, thumbnailHeight), SKBitmapResizeMethod.Lanczos3))
            {
                // Convert the thumbnail to a byte array
                using (var thumbnailImage = SKImage.FromBitmap(thumbnailBitmap))
                using (var thumbnailImageStream = new MemoryStream())
                {
                    thumbnailImage.Encode(SKEncodedImageFormat.Png, 100).SaveTo(thumbnailImageStream);
                    byte[] thumbnailBytes = thumbnailImageStream.ToArray();

                    // Convert the thumbnail bytes to a base64 string
                    string thumbnailBase64 = Convert.ToBase64String(thumbnailBytes);

                    return "data:image/png;base64," + thumbnailBase64;
                }
            }
        }
    }
}