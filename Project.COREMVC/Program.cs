using Project.BLL.ServiceInjections;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient(); //Eger bir API consume edilecekse HTTP protokol�nde client taraf�nda oldugumuzu Middleware'e bildirmeliyiz...

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache(); //Eger Session kompleks yap�larlar cal�smak icin Extension metodu eklenme durumuna mar�z kalm�ssa bu kod projenizin sagl�kl� cal�smas� icin gereklidir...

builder.Services.AddSession(x =>
{
    x.IdleTimeout = TimeSpan.FromDays(1); //Projeyi ki�inin bos durma s�resi eger 5 dakikal�k bir s�re olursa Session bosa c�ks�n
    x.Cookie.HttpOnly = true; //document.cookie'den ilgili bilginin g�zlemlenmesi
    x.Cookie.IsEssential = true;
});

builder.Services.AddIdentityServices();
builder.Services.AddDbContextService(); //DbContextService'imizi BLL'den alarak middleware'e entegre ettik...

builder.Services.AddCookieServices();

builder.Services.AddRepServices();
builder.Services.AddManagerServices();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Shopping}/{action=Index}/{id?}");

app.Run();
