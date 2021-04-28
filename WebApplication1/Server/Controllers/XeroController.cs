using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xero.NetStandard.OAuth2.Api;
using Xero.NetStandard.OAuth2.Model.Files;
using WebApplication1.Shared;
using System.IO;

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
            var tenantId = await GetTenantId(accessToken);
            var response = await FilesApi.GetFilesAsync(accessToken, tenantId);
            var filesItems = response.Items;
            Console.WriteLine("hello");
            return Ok(filesItems);
        }

        [HttpGet]
        [Route("getfolders")]
        public async Task<IActionResult> GetFolders()
        {
            var FilesApi = new FilesApi();
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
            var tenantId = await GetTenantId(accessToken);
            var response = await FilesApi.GetFoldersAsync(accessToken, tenantId);
            return Ok(response);
        }

        [HttpGet]
        [Route("getinvoices")]
        public async Task<IActionResult> GetInvoices()
        {
            var accountingApi = new AccountingApi();
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
            var tenantId = await GetTenantId(accessToken);
            var response = await accountingApi.GetInvoicesAsync(accessToken, tenantId);
            return Ok(response);
        }

        [HttpGet]
        [Route("getfile")]
        public async Task<IActionResult> GetFile(Guid id)
        {
            var FilesApi = new FilesApi();
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
            var tenantId = await GetTenantId(accessToken);
            var response = await FilesApi.GetFileContentAsync(accessToken, tenantId, id);
            using (var memoryStream = new MemoryStream())
            {
                response.CopyTo(memoryStream);
                return Ok(memoryStream.ToArray());
            }
        }

        [HttpGet]
        [Route("getassociation")]
        public async Task<IActionResult> GetAssociation(Guid id)
        {
            var FilesApi = new FilesApi();
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
            var tenantId = await GetTenantId(accessToken);
            var response = await FilesApi.GetFileAssociationsAsync(accessToken, tenantId, id);
            return Ok(response);
        }

        [HttpPost]
        [Route("uploadfile")]
        public async Task<IActionResult> UploadFile([FromBody] XeroUpload file)
        {
            var FilesApi = new FilesApi();
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
            var tenantId = await GetTenantId(accessToken);
            if (file.FolderId == null || file.FolderId == Guid.Empty)
            {
                await FilesApi.UploadFileAsync(accessToken, tenantId, file.FileContent, file.FileName, file.FileName, file.ContentType);
            }
            else
            {
                await FilesApi.UploadFileToFolderAsync(accessToken, tenantId, file.FolderId ?? Guid.Empty, file.FileContent, file.FileName, file.FileName, file.ContentType);
            }
            return Redirect("/");
        }


        [HttpPost]
        [Route("createfolder")]
        public async Task<IActionResult> CreateFolder([FromBody] XeroFolder createFolder)
        {
            var FilesApi = new FilesApi();
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
            var tenantId = await GetTenantId(accessToken);
            var folder = new Folder();
            folder.Name = createFolder.Name;
            var test = await FilesApi.CreateFolderAsync(accessToken, tenantId, folder);
            return Redirect("/");
        }

        [HttpPost]
        [Route("deletefolder")]
        public async Task<IActionResult> DeleteFolder([FromBody] Folder folder)
        {
            var FilesApi = new FilesApi();
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
            var tenantId = await GetTenantId(accessToken);
            await FilesApi.DeleteFolderAsync(accessToken, tenantId, folder.Id ?? Guid.Empty);
            return Redirect("/");
        }

        [HttpPost]
        [Route("deleteassociation")]
        public async Task<IActionResult> DeleteAssociation([FromBody] Association association)
        {
            var FilesApi = new FilesApi();
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
            var tenantId = await GetTenantId(accessToken);
            await FilesApi.DeleteFileAssociationAsync(accessToken, tenantId, association.FileId ?? Guid.Empty, association.ObjectId ?? Guid.Empty);
            return Redirect("/");
        }

        [HttpPost]
        [Route("deletefile")]
        public async Task<IActionResult> DeleteFile([FromBody] FileObject file)
        {
            var FilesApi = new FilesApi();
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
            var tenantId = await GetTenantId(accessToken);
            await FilesApi.DeleteFileAsync(accessToken, tenantId, file.Id ?? Guid.Empty);
            return Redirect("/");
        }

        [HttpPost]
        [Route("createassociation")]
        public async Task<IActionResult> CreateAssociation([FromBody] Association association)
        {
            var FilesApi = new FilesApi();
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
            var tenantId = await GetTenantId(accessToken);
            await FilesApi.CreateFileAssociationAsync(accessToken, tenantId, association.FileId ?? Guid.Empty, association);
            return Redirect("/");
        }


        [HttpGet]
        [Route("getassociations")]
        public async Task<IActionResult> GetAssociations(Guid id)
        {
            var filesApi = new FilesApi();
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
            var tenantId = await GetTenantId(accessToken);
            var response = await filesApi.GetFileAssociationsAsync(accessToken, tenantId, id);
            return Ok(response);
        }

        [HttpGet]
        [Route("getconnection")]
        public async Task<IActionResult> GetConnection()
        {
            var FilesApi = new FilesApi();
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
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
