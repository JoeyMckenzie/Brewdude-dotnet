using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Brewdude.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Brewdude.Application.Infrastructure
{
    public class RequestHandler
    {
        private readonly RequestDelegate _requestDelegate;

        public RequestHandler(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            var currentBody = context.Request.Body;

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    // Grab the current request pipeline body
                    context.Response.Body = memoryStream;
                    await _requestDelegate(context);

                    // Reset the body 
                    context.Response.Body = currentBody;
                    context.Response.ContentType = "application/json";
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var readToEnd = new StreamReader(memoryStream).ReadToEnd();
                    var objResult = JsonConvert.DeserializeObject(readToEnd);
                    var result = BrewdudeResponse.Create((HttpStatusCode) context.Response.StatusCode, objResult);
                    var deserialized = JsonConvert.SerializeObject(result);
                    await context.Response.WriteAsync(deserialized);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}