using Microsoft.AspNetCore.Identity;

namespace SiliconApp.Models
{
    public class ApplicationUser:IdentityUser
    {


        public string FirstName { get; set; } = null!;
        public string LastName { get; set;} = null!;

        public string FullName
        {
            get { return FirstName + " " + LastName; }
            set { _=FirstName + " " + LastName; }
        }

        public string? PersonImg { get; set; }
        public string? Bio { get; set; }
        public string Address1 { get; set; }=null!;
        public string Address2 { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;


        public ICollection<UserSavedItem>? UserSavedItems { get; set; }

    }
}
