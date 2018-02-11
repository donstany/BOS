using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using BOS.Models_Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BOS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //complicated FK
        public virtual ICollection<Voting> VoteCreators { get; set; }
        public virtual ICollection<Voting> CandidateForGifts { get; set; }
        public ICollection<VotingDetail> VotingDetails { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Full Name of Employee")]
        public string FullName { get; set; }

        //[Required]
        //[Display(Name = "Date of  your BirthDay")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM}", ApplyFormatInEditMode = true)]
        [Required]
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM}")]
        public DateTime BirthDate { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("FullName", this.FullName));
            userIdentity.AddClaim(new Claim("BirthDay", this.BirthDate.ToString("dd-MM")));
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Voting> Votings { get; set; }
        public DbSet<VotingDetail> VotingDetials { get; set; }
        //public DbSet<Employee> Employees { get; set; }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //// configures one-to-many relationship
            //modelBuilder.Entity<Student>()
            //    .HasRequired<Grade>(s => s.CurrentGrade)
            //    .WithMany(g => g.Students)
            //    .HasForeignKey<int>(s => s.CurrentGradeId);


            // https://stackoverflow.com/questions/28531201/entitytype-identityuserlogin-has-no-key-defined-define-the-key-for-this-entit
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Voting>()
                        .HasRequired(s => s.VoteCreator)
                        .WithMany(m => m.VoteCreators)
                        .HasForeignKey(s => s.VoteCreatorId);

            modelBuilder.Entity<Voting>()
                        .HasRequired(s => s.CandidateForGift)
                        .WithMany(m => m.CandidateForGifts)
                        .HasForeignKey(s => s.CandidateForGiftId);

            //modelBuilder.Entity<VotingDetail>()
            //        .HasRequired(s => s.Employee)
            //        .WithMany(m => m.CandidateForGifts)
            //        .HasForeignKey(s => s.CandidateForGiftId);

            // configures one-to-many relationship
            //modelBuilder.Entity<VotingDetail>()
            //    .HasRequired<Voting>(v => v.Employee.Id)
            //    .WithMany(g => g.Students)
            //    .HasForeignKey<int>(s => s.CurrentGradeId);

        }

        //public System.Data.Entity.DbSet<BOS.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}
