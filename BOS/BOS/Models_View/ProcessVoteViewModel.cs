using System;
using System.ComponentModel.DataAnnotations;

namespace BOS.Models_View
{

  public class ProcessVoteViewModel
  {
    public int EmployeeId { get; set; }
    public int VotingId { get; set; }
    public int OwnerId { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Full Name of Employee")]
    public string EmployeeFullName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM}", ApplyFormatInEditMode = true)]
    public DateTime Birthdate { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Employee Email")]
    public string Email { get; set; }
  }
}