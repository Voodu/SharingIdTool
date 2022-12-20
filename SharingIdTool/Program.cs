using Core.MergerService;
using Core.TrackedDomainService;
using Core.ValidationService;
using Microsoft.Fast.Components.FluentUI;
using SharingIdTool;
using SharingIdTool.Interop.Custom;
using SharingIdTool.Interop.TeamsSDK;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddFluentUIComponents();

builder.Services.AddTeamsFx(builder.Configuration.GetSection("TeamsFx"));
builder.Services.AddScoped<MicrosoftTeams>();
builder.Services.AddScoped<TeamsThemeChange<App>>();
builder.Services.AddTransient<IClipboardCopy, ClipboardCopy>();
builder.Services.AddTransient<IMergerService, MergerService>();
builder.Services.AddTransient<ITrackedDomainService, TrackedDomainService>();
builder.Services.AddTransient<IValidationService, ValidationService>();

builder.Services.AddControllers();
builder.Services.AddHttpClient("WebClient", client => client.Timeout = TimeSpan.FromSeconds(600));
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapControllers();
});

app.Run();