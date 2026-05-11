using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace RazorDemo.Services;

public class ProductImageStorageService
{
    private const int MaxImageWidth = 600;
    private const int MaxImageHeight = 600;
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    public async Task<List<string>> SaveUploadedImagesAsync(IReadOnlyCollection<IFormFile>? uploadedImages, CancellationToken cancellationToken = default)
    {
        var imageUrls = new List<string>();
        if (uploadedImages is null || uploadedImages.Count == 0)
        {
            return imageUrls;
        }

        var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
        Directory.CreateDirectory(uploadDirectory);

        foreach (var file in uploadedImages)
        {
            if (file is null || file.Length == 0)
            {
                continue;
            }

            var extension = Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Ảnh sản phẩm chỉ hỗ trợ định dạng JPG, PNG hoặc WEBP.");
            }

            var fileName = $"{Guid.NewGuid()}{extension.ToLowerInvariant()}";
            var filePath = Path.Combine(uploadDirectory, fileName);

            await using var inputStream = file.OpenReadStream();

            try
            {
                using var image = await Image.LoadAsync(inputStream, cancellationToken);
                image.Mutate(context => context.AutoOrient().Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(MaxImageWidth, MaxImageHeight)
                }));

                await image.SaveAsync(filePath, cancellationToken);
            }
            catch (UnknownImageFormatException)
            {
                throw new InvalidOperationException("Tệp tải lên không phải là ảnh hợp lệ.");
            }

            imageUrls.Add($"/images/products/{fileName}");
        }

        return imageUrls;
    }
}