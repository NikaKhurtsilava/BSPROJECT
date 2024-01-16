using Microsoft.AspNetCore.Identity;

namespace FirstProjectTest.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrivateNumber {  get; set; } 

    }
}
