﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Forms
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>()
                .UseUrls($"{Config.Config.hostUrl}{Config.Config.PORT}");
    }
}