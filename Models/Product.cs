using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace RazorDemo.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mô tả là bắt buộc")]
    public string Description { get; set; } = string.Empty;

    public string Images { get; set; } = "[]"; // JSON array of image URLs

    [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho phải >= 0")]
    public int Stock { get; set; }

    public int? CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    [NotMapped]
    public List<string> ImageUrls
    {
        get => JsonSerializer.Deserialize<List<string>>(Images) ?? new();
        set => Images = JsonSerializer.Serialize(value);
    }

    public string ImageUrl => ImageUrls.FirstOrDefault() ?? "";
}

