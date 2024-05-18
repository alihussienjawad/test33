namespace SiliconApp.Models.VM
{
    public class EditUserRole
    {
        public ApplicationUser User { get; set; } = null!;
        public List<Role> Roles { get; set; } = null!;
    }
}
