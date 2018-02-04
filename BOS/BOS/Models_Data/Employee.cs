using BOS.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BOS.Models_Data
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

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

        public ApplicationUser AppUser { get; set; }

       // public ICollection<VotingDetail> votingDetails { get; set; }
    }
}