using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication1.Shared;

namespace WebApplication1.Client
{
    public static class Utility
    {
        public static void StoreTenant(OrgDetails tenant)
        {
            string org = JsonSerializer.Serialize(tenant);
            System.IO.File.WriteAllText("./orgdetails.json", org);
        }

        public static string GetOrgName()
        {
            string name;
            try
            {
                string org = System.IO.File.ReadAllText("./orgdetails.json");
                name = JsonSerializer.Deserialize<OrgDetails>(org).TenantName;
            }
            catch (IOException)
            {
                name = String.Empty;
            }
            return name;
        }
        
    }
}