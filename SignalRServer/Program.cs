using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Owin;

namespace SignalRServer
{
    partial class Program
    {
        const string ServerUri = "http://localhost:8084";

        static void Main(string[] args)
        {

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            serializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            var serializer = JsonSerializer.Create(serializerSettings);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);

            using (WebApp.Start<Startup>(ServerUri))
            {
                Console.WriteLine("Hub on " + ServerUri);
                Console.ReadLine();
            }
        }
    }

    internal class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
            app.MapSignalR("/signalr", new HubConfiguration());
        }
    }
}