using Platinum.Core.Abstractions.Models.Response;
using RazorEngine;
using RazorEngine.Templating;
using RazorLight;

namespace Platinum.Core.Helpers
{
    public static class RazorHelper
    {
        public static string Template<T>(string templateHtml, T model)
        {
            //string template = "Hello @Model.Name!";
            var result = Engine
               .Razor
               .RunCompile(templateHtml, "templateKey", typeof(T), model);
            return result;
        }

        public static async Task<string> TemplateAsync<T>(string templateHtml, T model)
        {
            var engine = new RazorLightEngineBuilder()
            // required to have a default RazorLightProject type,
            // but not required to create a template from string.
            .UseEmbeddedResourcesProject(typeof(T))
            .SetOperatingAssembly(typeof(T).Assembly)
            .UseMemoryCachingProvider()
            .Build();

           // ViewModel model = new ViewModel { Name = "John Doe" };

            string result = await engine.CompileRenderStringAsync(Guid.NewGuid().ToString(), templateHtml, model);
            return result;
            //using (var writer = new StringWriter())
            //{
            //    Engine.Razor
            //   .RunCompile(templateHtml, Guid.NewGuid().ToString(), writer, typeof(T), model);
            //    await writer.FlushAsync();
            //    return writer.ToString();
            //}
        }
    }
}
