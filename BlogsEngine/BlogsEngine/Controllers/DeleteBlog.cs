using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using BlogsEngine.Core.Interfaces;
using BlogsEngine.Models.Authentication;
using BlogsEngine.Models.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace BlogsEngine.Controllers
{
    public class DeleteBlog
    {
        private readonly ILogger<DeleteBlog> _logger;
        private readonly IBlogsService blogsService;
        private readonly IUsersService usersService;

        public DeleteBlog(ILogger<DeleteBlog> log, IBlogsService blogsService, IUsersService usersService)
        {
            _logger = log;
            this.blogsService = blogsService;
            this.usersService = usersService;
        }

        [FunctionName("DeleteBlog")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "blogs/{blogId}/reject")] HttpRequest req, int blogId)
        {
            // SIMULATING AUTHORIZATION PROCESS
            JWTValidator auth = new JWTValidator(req);
            if (!auth.IsValid)
            {
                return new UnauthorizedResult();
            }
            // SIMULATING AUTHENTICATION 
            try
            {
                var user = usersService.Authenticate(auth);
            }
            catch
            {
                return new UnauthorizedResult();
            }

            if (Enum.Parse<Role>(auth.Role, true) != Role.Editor) return new UnauthorizedResult();

            var result = blogsService.RejectBlog(blogId);

            return new OkObjectResult(result);
        }
    }
}

