using System.ComponentModel.DataAnnotations;

namespace MoYobuDb.Models
{
    public class CreateRoleViewModel
    {
        [Required] public string RoleName { get; set; }
    }
}