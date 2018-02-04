using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BOS.Models_Data
{
    public class VotingDetail
    {
        [Key]
        public int VotingDetailId { get; set; }
        public int VotingId { get; set; }
        public Voting Voting { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int GiftId { get; set; }
        public Gift Gift { get; set; }
    }
}