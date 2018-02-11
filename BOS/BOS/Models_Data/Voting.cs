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
            IsYearOfActivationAvailable = false;
        }
        [Key]
        public int VotingId { get; set; }
        public string VoteCreatorId { get; set; }
        public virtual ApplicationUser VoteCreator { get; set; }
        public string CandidateForGiftId { get; set; }
        public virtual ApplicationUser CandidateForGift { get; set; }
        public ICollection<VotingDetail> VotingDetails { get; set; }

        [Display(Name = "Is Vote Acive?")]
        public bool IsActive { get; set; }
        public bool IsOnlyOnceVoted { get; set; }
        public int YearOfActivationVote { get; set; }
        public bool IsYearOfActivationAvailable { get; set; }
        [Display(Name = "Is Vote Completed?")]
        public bool IsCompleted { get; set; }
    }
}