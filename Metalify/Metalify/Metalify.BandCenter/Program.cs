using Metalify.BandCenter.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add HttpClient for API communication
builder.Services.AddHttpClient<IBandCenterService, BandCenterService>(client =>
{
    // Configure base address for Bandcenter API
    client.BaseAddress = new Uri("http://localhost:5066/"); // Update with actual Bandcenter API URL
});

// Add session service as singleton to maintain state across the app
builder.Services.AddScoped<IBandCenterService, BandCenterService>();
builder.Services.AddSingleton<IBandSessionService, BandSessionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
