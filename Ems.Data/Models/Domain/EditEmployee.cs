using System.ComponentModel.DataAnnotations;

namespace Ems.Data.Models.Domain
{
    public class EditEmployee
    {
        public int EmpID { get; set; }
        // Password fields are removed or made optional here

        [Required(ErrorMessage = "Role is required")]
        public string EmpRole { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string EmpStatus { get; set; }
    }

}
