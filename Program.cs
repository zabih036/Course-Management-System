using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Dynamic;
using System.IO;

namespace JahanInstitute
{
    public class Program
    {
       
        public static void Main(string[] args)
        {
            //var appfile = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "bundleconfig.json");
            //var json = File.ReadAllText(appfile);

            //var jsonfile = new JsonSerializerSettings();

            //jsonfile.Converters.Add(new ExpandoObjectConverter());
            //jsonfile.Converters.Add(new StringEnumConverter());

            //dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(json, jsonfile);

            //if (config.sourceData)
            //{
            //    var date = DateTime.Now.Date;
            //    var until = Convert.ToDateTime("8/14/2022");
            //    if (date > until)
            //    {

            //        config.sourceData = false;

            //        var newjson = JsonConvert.SerializeObject(config, Formatting.Indented, jsonfile);

            //        File.WriteAllText(appfile, newjson);

            //    }

            //}

            BuildWebHost(args).Run();

        }

        public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();


    }
}
