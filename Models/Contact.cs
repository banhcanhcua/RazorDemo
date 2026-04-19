using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RazorDemo.Models;

public class CustomBirthDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime date)
        {
            return date <= DateTime.Today
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage ?? "Ngày sinh phải nhỏ hơn hoặc bằng ngày hiện tại");
        }

        return ValidationResult.Success;
    }
}

public class Contact
{
    [DisplayName("Id của bạn")]
    [Range(1, 100, ErrorMessage = "Nhập sai")]
    public int ContactId { get; set; }

    [Required(ErrorMessage = "Tên là bắt buộc")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Họ là bắt buộc")]
    public string LastName { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [CustomBirthDate(ErrorMessage = "Ngày sinh nhỏ hơn hoặc bằng ngày hiện tại")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Nhập sai định dạng")]
    public string Email { get; set; } = string.Empty;
}
