using System;
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
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace BlogsEngine.Controllers
{
    public class PostComment
    {
        private readonly ILogger<PostComment> _logger;
        private readonly IBlogsService blogsService;
        private readonly IUsersService usersService;
        public PostComment(ILogger<PostComment> log, IBlogsService blogsService, IUsersService usersService)
        {
            _logger = log;
            this.blogsService = blogsService;
            this.usersService = usersService;

        }

        [FunctionName("PostComment")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "blogs/{blogId}/comments")] HttpRequest req, int blogId)
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

            string requestBody = String.Empty;
            using (StreamReader streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }
            var data = JsonConvert.DeserializeObject<Comment>(requestBody);
            data.BlogId = blogId;
            data.AuthorId = auth.Id;

            var result = blogsService.CommentBlog(blogId, data);

            return new OkObjectResult(result);
        }
    }
}

