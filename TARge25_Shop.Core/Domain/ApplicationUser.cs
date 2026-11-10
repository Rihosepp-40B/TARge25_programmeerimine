using Microsoft.AspNet.Identity.EntityFramework;

namespace TARge25_Shop.Core.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string City { get; set; }
    }
}
