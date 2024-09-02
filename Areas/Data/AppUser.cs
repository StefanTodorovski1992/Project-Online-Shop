using Microsoft.AspNetCore.Identity;

namespace Project.MVC.Areas.Data
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get;set; }
        public string LastName { get;set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
