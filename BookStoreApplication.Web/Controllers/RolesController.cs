using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApplication.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly PermRoleRepository _permroleRepository;

        public RolesController(PermRoleRepository permroleRepository)
        {
            _permroleRepository = permroleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _permroleRepository.GetRoles();

            return Ok(roles);
        }
    }
}