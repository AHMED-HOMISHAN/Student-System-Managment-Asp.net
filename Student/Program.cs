using Microsoft.EntityFrameworkCore;
using Student.DataAccess;
using Student.Models;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IRepository<Students>, Repository<Students>>();
builder.Services.AddScoped<IRepository<Teachers>, Repository<Teachers>>();
builder.Services.AddScoped<IRepository<Subjects>, Repository<Subjects>>();
builder.Services.AddScoped<IRepository<Departments>, Repository<Departments>>();
builder.Services.AddScoped<IRepository<Invoices>, Repository<Invoices>>();
builder.Services.AddScoped<IRepository<Libraries>, Repository<Libraries>>();
builder.Services.AddScoped<IRepository<Courses>, Repository<Courses>>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<stdDBContext>(options => options.UseSqlServer("Data Source=AHMED;Initial Catalog=SchoolManagement;Integrated Security=True;"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
