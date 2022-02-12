using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;

namespace AppSettingsManager;

// 4.C) Dependecy Injection, binding in an extention method
public static class ConfigurationExtension
{
	public static void AddConfiguration<T>(this WebApplicationBuilder builder, string configurationTag = null) where T : class
	{
		if (string.IsNullOrEmpty(configurationTag))
		{
			configurationTag = typeof(T).Name;
		}

		var instance = Activator.CreateInstance<T>();
		new ConfigureFromConfigurationOptions<T>(builder.Configuration.GetSection(configurationTag)).Configure(instance);
		builder.Services.AddSingleton(instance);

	}
}
