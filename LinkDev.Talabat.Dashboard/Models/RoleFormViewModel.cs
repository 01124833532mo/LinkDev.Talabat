using System.ComponentModel.DataAnnotations;

namespace LinkDev.Talabat.Dashboard.Models
{
	public class RoleFormViewModel
	{
		[Required(ErrorMessage ="name is requered")]
		[StringLength(256)]

        public string Name { get; set; }
    }
}
