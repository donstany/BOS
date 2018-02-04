using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BOS.Models_Data
{
    public class Gift
    {
        [Key]
        public int GiftId { get; set; }

        [Required]
        [Display(Name = "Gift Description")]
        public string Description { get; set; }
        public ICollection<VotingDetail> VotingDetails { get; set; }
    }
}