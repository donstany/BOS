using System.ComponentModel.DataAnnotations;

namespace BOS.Models_Data
{
    public class Employee
    {
        [Key]
        public int MyProperty { get; set; }
    }
}