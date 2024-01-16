using BankPayy.Services;
using BankPayy.Services.IServices;
using BankPayy.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<BankSettings>(builder.Configuration.GetSection("BankSettings"));
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
var app = builder.Build();


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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();
