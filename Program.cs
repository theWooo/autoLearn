global using System.Diagnostics;
using diplom.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace diplom
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(c=> { c.LoginPath = "/Auth/Authorization";
                c.LogoutPath = "/Auth/Deauthorization";    
            }
            );
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();                                  //---ADDED
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("Authorization", "middleware response");
            //    await next();
            //});
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();                                        //---ADDED
            app.UseAuthorization();

            app.UseSession();                                               //---ADDED

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            
            
            app.Run();
        }
    }
}
