using AppSettingsManager;
using AppSettingsManager.Models;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 4.A) Binding a Class to a Settings section (Denpendecy Injection - IOptions)
// Then, this can either be injected inside a controller, or inside a view.
builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));
builder.Services.Configure<SocialLoginSettings>(builder.Configuration.GetSection("SocialLoginSettings"));


// 4.B) Dependecy Injection, binding in Startup.cs
//var twilioSettings = new TwilioSettings();
//new ConfigureFromConfigurationOptions<TwilioSettings>(builder.Configuration.GetSection("Twilio")).Configure(twilioSettings);
//builder.Services.AddSingleton(twilioSettings);

// 4.C) Dependecy Injection, binding in an extention method
builder.AddConfiguration<TwilioSettings>("Twilio");
builder.AddConfiguration<SocialLoginSettings>("SocialLoginSettings");
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

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
