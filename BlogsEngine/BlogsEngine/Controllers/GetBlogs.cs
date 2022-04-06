using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using BlogsEngine.Core.Interfaces;
using BlogsEngine.Models.Authentication;
using BlogsEngine.Models.Authorization;
using BlogsEngine.Models.Blogs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace BlogsEngine.Controllers
{
    public class GetBlogs
    {
        private readonly ILogger<GetBlogs> _logger;
        private readonly IBlogsService blogsService;
        private readonly IUsersService usersService;
        public GetBlogs(ILogger<GetBlogs> log, IBlogsService blogsService, IUsersService usersService)
        {
            _logger = log;
            this.blogsService = blogsService;
            this.usersService = usersService;
        }

        [FunctionName("GetBlogs")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "blogs")] HttpRequest req)
        {
            _logger.LogInformation("Get Blogs is running");

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

            StringValues ownedValue;
            StringValues pendingValue;

            IEnumerable<Blog> blogs = new List<Blog>();
            if (!req.Query.TryGetValue("owned", out ownedValue) & !req.Query.TryGetValue("pending", out pendingValue))
            {
                blogs = blogsService.GetBlogs();
            }

            if (ownedValue == "true" && pendingValue == "true") return new BadRequestObjectResult("Invalid search criteria");

            if (ownedValue == "true")
            {
                if (Enum.Parse<Role>(auth.Role, true) != Role.Writer) return new UnauthorizedResult();
                blogs = blogsService.GetMyOwnBlogs(auth.Id);
            }

            if (pendingValue == "true")
            {
                if (Enum.Parse<Role>(auth.Role, true) != Role.Editor) return new UnauthorizedResult();
                blogs = blogsService.GetPendingBlogs();
            }      

            return new OkObjectResult(blogs);
        }
    }
}

