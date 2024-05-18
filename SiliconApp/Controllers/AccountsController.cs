using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using SiliconApp.Data;
using SiliconApp.Models;
using SiliconApp.Models.VM;

namespace SiliconApp.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
  
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountsController(ApplicationDbContext context,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
           
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
             
            return View(await _context.ApplicationUsers.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string? Id)
        {
            if (Id == null)
            {
                return View("Error");
            }
            ApplicationUser? applicationUser =await  _context.ApplicationUsers.FindAsync(Id);
            if (applicationUser == null)
            {
                return View("Error");
            }
            EditUserRole editUserRole = new() { User=applicationUser ,Roles=new()};

            var roles = _roleManager.Roles.ToList();
            foreach (var role in roles) {

                editUserRole.Roles.Add(new()
                {
                    Id = role.Id,
                    Name = role.Name,
                    IsSelected =await _userManager.IsInRoleAsync(applicationUser, role.Name)
                }); ;
            }


            return View(editUserRole);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(string? Id, EditUserRole editUserRole)
        {
            if (Id !=editUserRole.User.Id)
            {
                return View("Error");
            }
            foreach(var role in _roleManager.Roles)
            {
                await _userManager.RemoveFromRoleAsync(editUserRole.User, role.Name);
                 _context.SaveChanges();
            }
         
                await _userManager.AddToRolesAsync(editUserRole.User, editUserRole.Roles.Where(i=>i.IsSelected).Select(i=>i.Name));
                await _context.SaveChangesAsync();

            

            return RedirectToAction("Index");
        }
        }
}
