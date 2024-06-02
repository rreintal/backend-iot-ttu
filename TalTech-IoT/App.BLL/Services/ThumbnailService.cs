using App.BLL.Contracts;
using SkiaSharp;

namespace App.BLL.Services;

public class ThumbnailService : IThumbnailService
{
    public string Compress(string base64ImageData)
{
    try
    {
        // Extract the image data from the base64 string
        string imageData = base64ImageData.Split(',')[1]; // Remove the "data:image/png;base64," prefix

        // Convert the base64 image data back to bytes
        byte[] imageBytes = Convert.FromBase64String(imageData);

        // Create an SKBitmap from the image bytes
        using (var stream = new MemoryStream(imageBytes))
        using (var originalBitmap = SKBitmap.Decode(stream))
        {
            // Set the target width while maintaining the aspect ratio
            int targetWidth = 1000;
            // Calculate the new height to maintain the aspect ratio
            double aspectRatio = (double)originalBitmap.Height / originalBitmap.Width;
            int targetHeight = (int)(targetWidth * aspectRatio);

            // Create a new SKBitmap for the thumbnail while maintaining the aspect ratio
            using (var thumbnailBitmap = originalBitmap.Resize(new SKImageInfo(targetWidth, targetHeight), SKFilterQuality.High))
            {
                // Convert the thumbnail to a byte array
                using (var thumbnailImage = SKImage.FromBitmap(thumbnailBitmap))
                using (var thumbnailImageStream = new MemoryStream())
                {
                    thumbnailImage.Encode(SKEncodedImageFormat.Png, 100).SaveTo(thumbnailImageStream);
                    byte[] thumbnailBytes = thumbnailImageStream.ToArray();

                    // Convert the thumbnail bytes back to a base64 string
                    string thumbnailBase64 = Convert.ToBase64String(thumbnailBytes);

                    return "data:image/png;base64," + thumbnailBase64;
                }
            }
        }
    }
    catch (Exception e)
    {
        // Log the exception details including inner exceptions
        Console.WriteLine("Failed to compress image! Exception: " + e.Message);
        if (e.InnerException != null)
            Console.WriteLine("Inner Exception: " + e.InnerException.Message);
    }

    return "image was invalid/empty. exception thrown while compressing.";
}
}