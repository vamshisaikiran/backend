using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("stadium")]
public class Stadium : BaseEntity
{
    public Stadium()
    {
        Events = new HashSet<Event>();
    }

    [Required(ErrorMessage = "Name field is required")]
    [StringLength(100, ErrorMessage = "Name field must be between 3 and 100 characters", MinimumLength = 3)]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address field is required")]
    [StringLength(100, ErrorMessage = "Address field must be between 3 and 100 characters", MinimumLength = 3)]
    [Display(Name = "Address")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Capacity field is required")]
    [Display(Name = "Capacity")]
    public int Capacity { get; set; } = 0;

    public ICollection<Event> Events { get; set; }
}