using AppSettingsManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppSettingsManager.Controllers;
public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly IConfiguration _config;

	public HomeController(ILogger<HomeController> logger, IConfiguration config)
	{
		_logger = logger;
		_config = config;
	}

	public IActionResult Index()
	{
		// One value
		ViewBag.SendGridKey = _config.GetValue<string>("SendGridKey");
		// Multiple value
		ViewBag.TwilioAuthToken = _config.GetValue<string>("Twilio:AuthToken");
		ViewBag.TwilioAccountSid = _config.GetSection("Twilio").GetValue<string>("AccountSid");
		return View();
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
