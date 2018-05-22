namespace AppointmentTest.Data
{
    using System;
    using System.Threading.Tasks;
    using AppointmentTest.Models;
    using Microsoft.AspNetCore.Identity;

    public class IdentityInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public IdentityInitializer(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Initialize()
        {
            if(await this._context.Database.EnsureCreatedAsync())
            {
                await this.CreateRole(Roles.ROLE_MEDIC);

                await this.CreateRole(Roles.ROLE_MEDIC);

                await this.CreateUser(new ApplicationUser
                {
                    UserName = "medic",
                    Email = "medic@test.com",
                    EmailConfirmed = true
                },"medic",Roles.ROLE_MEDIC); 

                await this.CreateUser(new ApplicationUser
                {
                    UserName = "nurse",
                    Email = "nurse@test.com",
                    EmailConfirmed = true
                },"nurse",Roles.ROLE_NURSE);
            }
        }

        private async Task CreateRole(string role)
        {
            if(!(await _roleManager.RoleExistsAsync(role)))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role));

                if(!result.Succeeded)
                    throw new Exception($"Error during cretion of the role {role}");
            }
        }

        private async Task CreateUser(ApplicationUser user, string password, string initialRole = null)
        {
            if((await _userManager.FindByNameAsync(user.UserName)) == null)
            {
                var resultado = await _userManager.CreateAsync(user, password);

                if (resultado.Succeeded && 
                    !String.IsNullOrEmpty(initialRole))
                    await _userManager.AddToRoleAsync(user, initialRole);
            }
        }

    }
}