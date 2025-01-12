# Ticketing System Project

این پروژه یک سیستم تیکت ساده است که با استفاده از **ASP.NET Core** و **Entity Framework** ساخته شده است. در این پروژه کاربران می‌توانند تیکت هایی ایجاد کرده و پاسخ‌هایی برای آن‌ها ارسال کنند. همچنین، مدیران می‌توانند کاربران را مدیریت کنند و وضعیت تیکت ها را نظارت کنند.



## همکاران پروژه

### این پروژه توسط تیمی از توسعه‌دهندگان با همکاری زیر ساخته شده است:

- **ایلحان امانی** - مسئول بخش Backend و پایگاه داده
- **نیما نوری** - مسئول بخش Frontend و طراحی UI/UX
- **عرفان صالحی** - مسئول بخش امنیت و تست پروژه

## ساختار پروژه

پروژه شامل ساختار پوشه‌ای زیر است:
<details>
<summary>All Stracture </summary>

- Volume serial number is 743C-8B59
- C:.
- │   appsettings.Development.json
- │   appsettings.json
- │   Program.cs
- │   project-structure.txt
- │   SeedData.cs
- │   TicketingSystem.csproj
- │   TicketingSystem.csproj.user
- │   
- 
- │                   
- ├───Controllers
- │       AccountController.cs
- │       AdminController.cs
- │       HomeController.cs
- │       TicketResponsesController.cs
- │       TicketsController.cs
- │       
- ├───Data
- │       ApplicationDbContext.cs
- │       
- 
- │       
- ├───Models
- │       ApplicationUser.cs
- │       ErrorViewModel.cs
- │       HomeViewModel.cs
- │       RegisterViewModel.cs
- │       Ticket.cs
- │       TicketingSystemModel.cs
- │       TicketListViewModel.cs
- │       TicketResponse.cs
- │       UserViewModel.cs
- │       
- 
- │           
- ├───Services
- │       ITicketService.cs
- │       IUserService.cs
- │       TicketService.cs
- │       UserService.cs
- │       
- ├───ViewModels
- │       LoginViewModel.cs
- │       TicketViewModel.cs
- │       
- ├───Views
- │   │   _ViewImports.cshtml
- │   │   _ViewStart.cshtml
- │   │   
- │   ├───Account
- │   │       AccessDenied.cshtml
- │   │       Login.cshtml
- │   │       Logout.cshtml
- │   │       Register.cshtml
- │   │       UserAccount.cshtml
- │   │       
- │   ├───Admin
- │   │       Dashboard.cshtml
- │   │       EditUser.cshtml
- │   │       Index.cshtml
- │   │       
- │   ├───Home
- │   │       Index.cshtml
- │   │       
- │   ├───Shared
- │   │       Error.cshtml
- │   │       _Layout.cshtml
- │   │       _Layout.cshtml.css
- │   │       _LoginPartial.cshtml
- │   │       _ValidationScriptsPartial.cshtml
- │   │       
- │   ├───TicketResponses
- │   │       Create.cshtml
- │   │       
- │   └───Tickets
- │           Create.cshtml
- │           Details.cshtml
- │           Index.cshtml
- │           

</details>


## پیش‌نیازها

برای اجرای پروژه نیاز به نصب ابزارهای زیر دارید:

- **.NET Core SDK** نسخه 6.0 یا بالاتر
- **SQL Server** یا پایگاه داده مشابه برای ذخیره‌سازی داده‌ها
- **IIS** یا **Kestrel** برای میزبانی وب‌سایت (در صورت نیاز به راه‌اندازی بر روی سرور)

## راه‌اندازی پروژه

### 1. کلون کردن مخزن

ابتدا مخزن پروژه را از گیت‌لب کلون کنید:
```sh
git https://github.com/naderii/ticketing.git
cd ticketing
```

### بسته‌های NuGet مورد نیاز

پروژه برای عملکرد صحیح نیاز به بسته‌های NuGet زیر دارد:

- **Azure.Identity** نسخه 1.11.4
- **Microsoft.Extensions.Caching.Memory** نسخه 8.0.1
- **System.Text.Json** نسخه 8.0.5
- **System.Formats.Asn1** نسخه 6.0.1
- **Microsoft.EntityFrameworkCore** نسخه 8.0.8
- **Microsoft.EntityFrameworkCore.SqlServer** نسخه 8.0.8
- **Microsoft.AspNetCore.Identity.EntityFrameworkCore** نسخه 8.0.8
- **Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore** نسخه 8.0.8
- **Microsoft.AspNetCore.Identity.UI** نسخه 8.0.8
- **Microsoft.EntityFrameworkCore.Tools** نسخه 8.0.8


### برای نصب این بسته‌ها، می‌توانید از دستور زیر استفاده کنید:

```sh

 dotnet add package Azure.Identity --version 1.11.4
 dotnet add package Microsoft.Extensions.Caching.Memory --version 8.0.1
 dotnet add package System.Text.Json --version 8.0.5
 dotnet add package System.Formats.Asn1 --version 6.0.1
 dotnet add package Microsoft.EntityFrameworkCore --version 8.0.8
 dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.8
 dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.8
 dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore --version 8.0.8
 dotnet add package Microsoft.AspNetCore.Identity.UI --version 8.0.8
 dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.8
```

## تنظیمات پایگاه داده
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TicketingDB;User Id=youruser;Password=yourpassword;"
  }
}

### مایگریشن پایگاه داده
```sh
Add-Migration Intials
Update-Database
```

## ویژگی‌های پروژه
- مدیریت کاربران با استفاده از ASP.NET Identity
- امکان ایجاد و پیگیری تیکت‌ها
- مدیریت پاسخ‌ها به تیکت‌ها
- صفحه داشبورد برای مدیران سیستم
- صفحات ورود و ثبت‌نام برای کاربران


## 🔗 Links


[![portfolio](https://img.shields.io/badge/my_portfolio-000?style=for-the-badge&logo=ko-fi&logoColor=white)](https://ticket.naderinia.ir/)
[![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/nader-naderi-13247417b?utm_source=share&utm_campaign=share_via&utm_content=profile&utm_medium=android_app)
[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)
[![GPLv3 License](https://img.shields.io/badge/License-GPL%20v3-yellow.svg)](https://opensource.org/licenses/)
[![AGPL License](https://img.shields.io/badge/license-AGPL-blue.svg)](http://www.gnu.org/licenses/agpl-3.0)


