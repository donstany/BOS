using BOS.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOS.Models_Data
{
    public class VotingDetail
    {
        [Key]
        public int VotingDetailId { get; set; }
        public int VotingId { get; set; }
        public Voting Voting { get; set; }

        public string ElectorateId { get; set; }
        [ForeignKey("ElectorateId")]
        public ApplicationUser ApplicationUser { get; set; }

        public int GiftId { get; set; }
        public Gift Gift { get; set; }
    }
}