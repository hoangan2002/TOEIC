﻿

using ChillToeic.Infrastructure.EmailSender;
using ChillToeic.Infrastructure.Middleware;
using ChillToeic.Jwt;
//using ChillToeic.Models.Entity;
using ChillToeic.Repository;
using ChillToeic.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<EducationService>();
builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddTransient<EmailOTPService>();
builder.Services.AddTransient<TestOfUserService>();
builder.Services.AddTransient<QuestionOfTestService>();
builder.Services.AddTransient<TestService>();
builder.Services.AddTransient<QuestionService>();
builder.Services.AddTransient<AnswerService>();
builder.Services.AddTransient<QuestionDetailService>();
builder.Services.AddTransient<CertificateService>();
builder.Services.AddTransient<CourseService>();
builder.Services.AddTransient<LectureDetailService>();
builder.Services.AddTransient<LectureService>();
builder.Services.AddTransient<CenterService>();
builder.Services.AddTransient<LearningProgressService>();
builder.Services.AddTransient<IVnPayService, VnPayService>();

builder.Services.AddTransient<OrdersService>();
// Add Authentication
builder.Services.AddSingleton<JwtService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    { opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
            ValidAudience = builder.Configuration["JwtOptions:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:SigningKey"]))
        };
    });
builder.Services.AddAuthorization(); 
//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiddleware<JwtFromCookieMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();
app.Use(async (context, next) =>
{
    if (context.User.Identity.IsAuthenticated)
    {
        // Kiểm tra vai trò của người dùng
        if (context.User.IsInRole("Admin"))
        {
            // Nếu người dùng có vai trò là "admin" và đường dẫn không bắt đầu bằng "/Admin/", chuyển hướng tới trang cần thiết
            if (!context.Request.Path.StartsWithSegments("/Admin"))
            {
                context.Response.Redirect("/Admin"); // Thay thế "/Admin/AccessDenied" bằng trang cần thiết cho việc chuyển hướng khi truy cập bị từ chối
                return;
            }
        }
    }
    else // Nếu người dùng chưa đăng nhập, chuyển hướng tới trang đăng nhập
    {
        if (!context.Request.Path.StartsWithSegments("/Login") &&
            context.Request.Path != "/")
        {
            context.Response.Redirect("/Login");
            return;
        }
    }
    await next();
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
