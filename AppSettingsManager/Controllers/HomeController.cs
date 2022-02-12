using AppSettingsManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace AppSettingsManager.Controllers;
public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly IConfiguration _config; // 1. IConfiguration in the Controller
	private readonly TwilioSettings _twilioSettings; //3. Binding a Class to a Settings section(Controler)
	private readonly IOptions<TwilioSettings> _twilioOptions; // 4.A) Binding a Class to a Settings section (Denpendecy Injection - IOptions)
	private readonly TwilioSettings _twilioSettings_di; // 4.B) & 4.C) Denpendecy Injection
	private readonly SocialLoginSettings _socialLoginSettings;

	public HomeController(ILogger<HomeController> logger,
		IConfiguration config, // 1. IConfiguration in the Controller
		IOptions<TwilioSettings> twilioOptions, // 4.A) Binding a Class to a Settings section (Denpendecy Injection - IOptions)
		TwilioSettings twilioSettings_di, // 4.B) & 4.C) Binding a Class to a Settings section (Denpendecy Injection)
		SocialLoginSettings socialLoginSettings
		)
	{
		_logger = logger;

		// 1. IConfiguration in the Controller
		_config = config;

		// 3. Binding a Class to a Settings section (Controler)
		_twilioSettings = new TwilioSettings();
		config.GetSection("Twilio").Bind(_twilioSettings);

		// 4.A) Binding a Class to a Settings section (Denpendecy Injection - IOptions)
		_twilioOptions = twilioOptions;

		// 4.B) & 4.C) Binding a Class to a Settings section (Denpendecy Injection)
		_twilioSettings_di = twilioSettings_di;

		// More Complexe example
		_socialLoginSettings = socialLoginSettings;

	}

	public IActionResult Index()
	{
		// 1.A) IConfiguration in the Controller: One value
		ViewBag.SendGridKey = _config.GetValue<string>("SendGridKey");

		// 1.B) IConfiguration in the Controller: Multiple value
		ViewBag.TwilioAuthToken = _config.GetValue<string>("Twilio:AuthToken");
		ViewBag.TwilioAccountSid = _config.GetSection("Twilio").GetValue<string>("AccountSid");

		// Multiple level example
		ViewBag.BottomLevelSetting_a = _config.GetValue<string>("FirstLevelSetting:SecondLevelSetting:BottomLevelSetting");
		ViewBag.BottomLevelSetting_b = _config.GetSection("FirstLevelSetting").GetSection("SecondLevelSetting").GetValue<string>("BottomLevelSetting");
		ViewBag.BottomLevelSetting_c = _config.GetSection("FirstLevelSetting").GetSection("SecondLevelSetting").GetSection("BottomLevelSetting").Value;
		
		// 3. Binding a Class to a Settings section (Controler)
		ViewBag.TwilioPhoneNumber = _twilioSettings.PhoneNumber;

		// 4.A) Binding a Class to a Settings section (Denpendecy Injection - IOptions)
		ViewBag.TwilioAuthToken_di_opt = _twilioOptions.Value.AuthToken;
		ViewBag.TwilioAccountSid_di_opt = _twilioOptions.Value.AccountSid;
		ViewBag.TwilioPhoneNumber_di_opt = _twilioOptions.Value.PhoneNumber;

		// 4.B) Binding a Class to a Settings section (Denpendecy Injection - Startup.cs)
		// 4.C) Binding a Class to a Settings section (Denpendecy Injection - extention method)
		ViewBag.TwilioAuthToken_di = _twilioSettings_di.AuthToken;
		ViewBag.TwilioAccountSid_di = _twilioSettings_di.AccountSid;
		ViewBag.TwilioPhoneNumber_di = _twilioSettings_di.PhoneNumber;

		// More Complexe example
		ViewBag.FacebookKey = _socialLoginSettings.FacebookSettings.Key;
		ViewBag.GoogleKey = _socialLoginSettings.GoogleSettings.Key;

		// Special case: ConnectionStrings
		ViewBag.ConnectionString = _config.GetConnectionString("AppDb");

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
