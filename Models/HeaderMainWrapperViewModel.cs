namespace RazorDemo.Models;

public class HeaderMainWrapperViewModel
{
    public bool IsAuthenticated { get; init; }

    public string? Username { get; init; }

    public string? Role { get; init; }
}