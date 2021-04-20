using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xero.NetStandard.OAuth2.Api;

namespace WebApplication1.Server.Controllers
{
    public class XeroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("getfiles")]
        public async Task<IActionResult> GetFiles()
        {
            var FilesApi = new FilesApi();
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
            var bearerToken = Request.Headers[HeaderNames.Authorization];
            var tenantId = await GetTenantId(accessToken);
            var response = await FilesApi.GetFilesAsync(accessToken, tenantId);
            var filesItems = response.Items;
            Console.WriteLine("hello");
            return Ok(filesItems);
        }

        [HttpGet]
        [Route("getconnection")]
        public async Task<IActionResult> GetConnection()
        {
            var FilesApi = new FilesApi();
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
            var bearerToken = Request.Headers[HeaderNames.Authorization];
            var tenantId = GetTenantId(accessToken);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                List<Connection> connections = await client.GetFromJsonAsync<List<Connection>>("https://api.xero.com/connections");
                return Ok(connections.FirstOrDefault());
            }
        }

        public async Task<string> GetTenantId(string accessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                List<Connection> connections = await client.GetFromJsonAsync<List<Connection>>("https://api.xero.com/connections");
                var tenantId = connections.FirstOrDefault().tenantId;
                return tenantId;
            }
        }
    }

}
