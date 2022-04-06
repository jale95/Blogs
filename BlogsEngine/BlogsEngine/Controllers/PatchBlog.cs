using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using BlogsEngine.Core.Interfaces;
using BlogsEngine.Models.Authentication;
using BlogsEngine.Models.Authorization;
using BlogsEngine.Models.Blogs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace BlogsEngine.Controllers
{
    public class PatchBlog
    {
        private readonly ILogger<PatchBlog> _logger;
        private readonly IBlogsService blogsService;
        private readonly IUsersService usersService;

        public PatchBlog(ILogger<PatchBlog> log, IBlogsService blogsService, IUsersService usersService)
        {
            _logger = log;
            this.blogsService = blogsService;
            this.usersService = usersService;

        }

        [FunctionName("PatchBlog")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "blogs/{blogId}")] HttpRequest req, int blogId)
        {
            // SIMULATING AUTHORIZATION PROCESS
            JWTValidator auth = new JWTValidator(req);
            if (!auth.IsValid)
            {
                return new UnauthorizedResult();
            }
            //SIMULATING AUTHENTICATION 
            try
            {
                var user = usersService.Authenticate(auth);
            }
            catch
            {
                return new UnauthorizedResult();

            }

            if (Enum.Parse<Role>(auth.Role, true) != Role.Writer) return new UnauthorizedResult();

            try
            {
                var blog = new Blog();            
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Blog>(requestBody);
                data.Id = blogId;
                data.AuthorId = auth.Id;
                data.Status = 3;
                var result = this.blogsService.PatchBlog(data);

                if (result == null) return new NotFoundObjectResult("Object not found");

                return new ObjectResult(result);
            }
            catch(Exception e)
            {
                return new BadRequestObjectResult("");
            }
            

        }
    }
}

