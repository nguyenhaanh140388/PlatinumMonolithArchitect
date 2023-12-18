using RazorEngine;
using RazorEngine.Templating;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Anhny010920.Core.Helpers
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
            using (var writer = new StringWriter())
            {
                Engine.Razor
               .RunCompile(templateHtml, Guid.NewGuid().ToString(), writer, typeof(T), model);
                await writer.FlushAsync();
                return writer.ToString();
            }
        }
    }
}
