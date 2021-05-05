using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFilesApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("BlazorFilesApp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorFilesApp.ServerAPI"));

            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth
                builder.Configuration.Bind("Xero", options.ProviderOptions);
                options.ProviderOptions.DefaultScopes.Add("accounting.transactions");
                options.ProviderOptions.DefaultScopes.Add("email");
                options.ProviderOptions.DefaultScopes.Add("files");
                options.ProviderOptions.DefaultScopes.Add("accounting.attachments");
                options.ProviderOptions.DefaultScopes.Add("accounting.contacts");
                options.ProviderOptions.DefaultScopes.Add("accounting.settings");
                options.ProviderOptions.ResponseType = "code";
                options.ProviderOptions.PostLogoutRedirectUri = "https://localhost:44356";
            });

            await builder.Build().RunAsync();
        }
    }
}
