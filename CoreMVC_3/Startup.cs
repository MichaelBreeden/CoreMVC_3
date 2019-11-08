using CoreMVC_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace CoreMVC_3
{
    public class Startup // .Net Core 2.1 - Starting with video 16
    {
        private IConfiguration _config;

        // 9. ASP NET Core appsettings.json file
        // Used to store App settings in web.config, but Core settings can come from 
        // Files - appsettings.json, appsettings.{Environment}.json
        // User Secrets
        // Environment Variables
        // Command-line arguments
        // 
        // IConfiguration Service can read all of these.
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.    
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc();  // (1) or AddMvcCore(); ...... Adds all required MVC Services including AddMvcCore()
                                // (3) It also adds Views, Razor, Json, and other stuff.
                                // services.AddMvcCore();  // (2) Fails because there is not Json format handler ... Only adds MVC Core Services
                                // (4) Only adds Mvc Core Services.

            services.AddMvc().AddXmlSerializerFormatters();

            // The Dependency Injection. here are 3 ways to do it... each with a different lifetime
            services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>(); // Created when first requested and is always re-used
            // services.AddTransient // Transient Service created each time requested
            // services.AddScopedn   // Created once instance per Web Request

            // For Serving Razor pages.
            // From https://docs.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-3.0&tabs=visual-studio
            // services.AddRazorPages(); // Says I need to have 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,  // Original services
                              ILogger<Startup> logger) // Added Service(s) Logger goes to output window
        {

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            if (env.IsDevelopment())
            {
                // First entry in the pipeline. If Dev and exception, do this. 
                // (1)app.UseDeveloperExceptionPage(); // This has options avialable too - DeveloperExceptionPageOptions 
                // Displays verbose Exception Page. Without this, the error page sucks.
                // Needs to be early in Processing Pipeline

                // (2)
                DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions()
                  { SourceCodeLineCount = 10 }; // Sets number of source code lines to display before and after exception
                app.UseDeveloperExceptionPage(developerExceptionPageOptions);
            }



#if NO_LONGER_NEEDED
            app.Use(async (context, next) => // Use is not Terminal Middleware. "next" means do the next Middleware
            {
                await context.Response.WriteAsync("Hello World! ...With Use - MW1...");
                logger.LogInformation("MW1: Incoming Request.");
                await next();
                logger.LogInformation("MW1: Outgoing Response."); // Logger goes to output window
            });
            app.Use(async (context, next) => // Use is not Terminal Middleware. "next" means do the next Middleware
            {
                await context.Response.WriteAsync("Hello World! ...With Use - MW2...");
                logger.LogInformation("MW2: Incoming Request.");
                await next();
                logger.LogInformation("MW2: Outgoing Response.");
            });
#endif

#if region_USING_LANDING_PAGE_STUFF
            // Also needed if you are serving default.htm[l], index.htm[l]
            // (1) It sets what the default files are.
            // app.UseDefaultFiles(); // Normal useage. Works in 2.2, 3.0

            // (2) This has two overloads, so you can:
            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions(); // Works in 2.2, 3.0
            //defaultFilesOptions.DefaultFileNames.Clear();
            //defaultFilesOptions.DefaultFileNames.Add("foo.html");
            //app.UseDefaultFiles(defaultFilesOptions); 

            // (3) or use Directory Browser Middleware
            FileServerOptions fileServerOptions = new FileServerOptions(); // Works in 2.2, 3.0
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("default.htm");
            app.UseFileServer(fileServerOptions);
#endif
            //-app.UseHttpsRedirection(); // Was hoping it would go to the Pages/*.cshtml
           
            // To serve static files, (1) they must be in wwwroot, Middleware must be added to Pipeline
            // Will only serve files located in wwwroot folder.
            // The default document is also in that folder - default.htm[l], index.htm[l]
            // works in 2.2, 3.0
            app.UseStaticFiles();

            //-app.UseMvc(); // hopefully this will redirect to Pages.. Nope 

            app.UseMvcWithDefaultRoute(); // Only stops here if there is a Home Controller or the URL includes a controller

            
            app.Run(async (context) =>
            {
                // Didn't know it could split this way...
                await context.Response.WriteAsync("Hello World! ...Context..." + System.Diagnostics
                                                  .Process.GetCurrentProcess().ProcessName + ". MyKey:" + _config["MyKey"] +
                                                  ". Hosting Environment=" + env.EnvironmentName + "...");
                logger.LogInformation("MW3: Request handled and Response produced.");
            });
        }
    }
}
