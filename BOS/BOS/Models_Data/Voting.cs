using BOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BOS.Models_Data
{
    public class Voting
    {
        public Voting()
        {
            YearOfActivationVote = DateTime.Now.Year;
        }
        [Key]
        public int VotingId { get; set; }
        public bool IsActive { get; set; }
        public bool IsOnlyOnceVoted { get; set; }
        public string InitializerOfVoteEmail { get; set; }
        public string OwnerEmail { get; set; }
        public int OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public int YearOfActivationVote { get; set; }
        public bool? IsYearOfActivationAvailable { get; set; }
        public ICollection<VotingDetail> VotingDetails { get; set; }
 

    }
}