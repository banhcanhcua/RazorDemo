# RazorDemo

Đây là một dự án ASP.NET Core Razor Pages mẫu cho quản lý sản phẩm, giỏ hàng, người dùng, đơn hàng...

## Yêu cầu hệ thống
- .NET 8.0 SDK (https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Một IDE như Visual Studio, Rider hoặc VS Code
- SQLite (database mặc định)

## Cài đặt các package cần thiết
Các package sẽ được tự động cài khi bạn build project, nhưng nếu cần cài thủ công:

```
dotnet restore
```

## Các bước chạy project
1. **Clone source code về máy:**
   ```
   git clone https://github.com/<your-username>/<your-repo>.git
   cd RazorDemo
   ```
2. **Khôi phục package:**
   ```
   dotnet restore
   ```
3. **Chạy migration để tạo database:**
   ```
   dotnet ef database update
   ```
   Nếu chưa có Entity Framework CLI, cài bằng:
   ```
   dotnet tool install --global dotnet-ef
   ```
4. **Chạy ứng dụng:**
   ```
   dotnet run
   ```
5. **Truy cập ứng dụng:**
   Mở trình duyệt và vào địa chỉ:
   ```
   http://localhost:5000
   ```

## Một số lệnh hữu ích
- Tạo migration mới:
  ```
  dotnet ef migrations add <MigrationName>
  ```
- Xem các package đã cài:
  ```
  dotnet list package
  ```

## Thông tin khác
- Thư mục wwwroot chứa file tĩnh (ảnh, css, js...)
- Thư mục Migrations chứa các file migration database
- Thư mục Models chứa các class dữ liệu
- Thư mục Pages chứa Razor Pages
- Thư mục Controllers chứa các controller MVC
- Thư mục Services chứa các service nghiệp vụ

## Đóng góp
Mọi đóng góp đều được hoan nghênh! Hãy tạo pull request hoặc issue nếu bạn muốn đóng góp hoặc báo lỗi.

---

Nếu có vấn đề khi cài đặt, hãy kiểm tra lại version .NET hoặc gửi issue lên repo.
