using ECommerceApp.Models;

namespace ECommerceApp.View_Models
{
    public class AssignRoleViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> AvailableRoles { get; set; }
        public List<string> CurrentRoles { get; set; }
    }
}