using ApiCatalogo.Repository;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;

namespace ApiCatalogo.GraphQL
{
    public class TesteGraphQLMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUnitofWork _unitofWork;

        public TesteGraphQLMiddleware(RequestDelegate next, IUnitofWork unitofWork)
        {
            _next = next;
            _unitofWork = unitofWork;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.StartsWithSegments("/graphql"))
            {
                using(var stream = new StreamReader(httpContext.Request.Body))
                {
                    var query = await stream.ReadToEndAsync();

                    if(!String.IsNullOrEmpty(query))
                    {
                        var schema = new Schema
                        {
                            Query = new CategoryQuery(_unitofWork)
                        };

                        var result = await new DocumentExecuter().ExecuteAsync(opt =>
                        {
                            opt.Schema = schema;
                            opt.Query = query;
                        });
                        await WriteResult(httpContext, result);
                    }
                }
            }
            else
            {
                await _next(httpContext);
            }
        }

        private async Task WriteResult(HttpContext httpContext, ExecutionResult result)
        {
            var json = new DocumentWriter(indent: true).Write(result);
            httpContext.Response.StatusCode = 200;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(json);
        }
    }
}
