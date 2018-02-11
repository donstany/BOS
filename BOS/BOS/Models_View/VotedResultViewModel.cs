using System.ComponentModel.DataAnnotations;

namespace BOS.Models_View
{
  public class VotedResultViewModel
  {
    [Display(Name = "Name Of Employee With Birthday")]
    public string NameOfEmployeeWithBd { get; set; }
    [Display(Name = "Email Of Employee With Birthday")]
    public string EmailOfEmployeeWithBd { get; set; }
    [Display(Name = "Year of Vote Activation")]
    public int? YearOfActivationVote { get; set; }
    [Display(Name = "Gift Description")]
    public string GiftDescription { get; set; }
    [Display(Name = "Voter's name")]
    public string NameOfVoter { get; set; }
    [Display(Name = "Voter's email")]
    public string EmailOfVoter { get; set; }
  }
}