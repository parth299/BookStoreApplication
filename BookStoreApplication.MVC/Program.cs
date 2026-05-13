//using BookStoreMVC.Services.Api;
//using BookStoreMVC.Services.Catalog;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllersWithViews();

//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession();

//builder.Services.AddHttpClient("BookStoreAPI", client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7092/");
//    client.DefaultRequestHeaders.Add("Accept", "application/json");
//});

//builder.Services.AddScoped<InventoryApiService>();
//builder.Services.AddScoped<BookApiService>();
//builder.Services.AddScoped<ShoppingCartApiService>();
//builder.Services.AddScoped<PurchaseApiService>();
//builder.Services.AddScoped<ProductCatalogService>();
//builder.Services.AddScoped<CartDisplayService>();

//var app = builder.Build();

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseSession();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Store}/{action=Index}/{id?}");

//app.Run();