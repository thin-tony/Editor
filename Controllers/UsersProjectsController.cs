using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Editor.Services;

namespace Editor.Controllers
{
    [ApiController]
    [Route("/users/{userId}/projects")]
    public class UsersProjectsController : ControllerBase
    {
        private IProjectsService service;
        public UsersProjectsController(IProjectsService service)
        {
            this.service = service;
        }
        [HttpGet]
        public IActionResult GetProjects([FromRoute] int ownerId)
        {
            var response = service.GetProjectsOfOwner(ownerId);
            return new ObjectResult(response.ResponseData) { StatusCode = response.StatusCode };
        }
    }
}
