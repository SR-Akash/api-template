using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.middleware
{
    public class ResponceChangeMiddlware
    {
        private readonly RequestDelegate _next;
        public ResponceChangeMiddlware(RequestDelegate next)
        {
            _next = next; 
        }
    public async Task Invoke(HttpContext context)
    {
        bool modifyResponse = true;
        Stream originBody = null;

        if (modifyResponse)
        {
            //uncomment this line only if you need to read context.Request.Body stream
            //context.Request.EnableRewind();

            originBody = ReplaceBody(context.Response);
        }

       // await _next(context);

        if (modifyResponse)
        {
            //as we replaced the Response.Body with a MemoryStream instance before,
            //here we can read/write Response.Body
            //containing the data written by middlewares down the pipeline 

            //finally, write modified data to originBody and set it back as Response.Body value
            ReturnBody(context.Response, originBody);
        }
    }
    private Stream ReplaceBody(HttpResponse response)
    {
        var originBody = response.Body;
        response.Body = new MemoryStream();
        return originBody;
    }

    private void ReturnBody(HttpResponse response, Stream originBody)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        response.Body.CopyTo(originBody);
        response.Body = originBody;
    }

    }
}
