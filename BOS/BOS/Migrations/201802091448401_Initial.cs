namespace BOS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.VotingDetails", new[] { "Voting_VotingId" });
            DropIndex("dbo.VotingDetails", new[] { "Gift_GiftId" });
            RenameColumn(table: "dbo.VotingDetails", name: "Gift_GiftId", newName: "GiftId");
            RenameColumn(table: "dbo.VotingDetails", name: "Voting_VotingId", newName: "VotingId");
            AddColumn("dbo.VotingDetails", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.VotingDetails", "VotingId", c => c.Int(nullable: false));
            AlterColumn("dbo.VotingDetails", "GiftId", c => c.Int(nullable: false));
            CreateIndex("dbo.VotingDetails", "VotingId");
            CreateIndex("dbo.VotingDetails", "GiftId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.VotingDetails", new[] { "GiftId" });
            DropIndex("dbo.VotingDetails", new[] { "VotingId" });
            AlterColumn("dbo.VotingDetails", "GiftId", c => c.Int());
            AlterColumn("dbo.VotingDetails", "VotingId", c => c.Int());
            DropColumn("dbo.VotingDetails", "Id");
            RenameColumn(table: "dbo.VotingDetails", name: "VotingId", newName: "Voting_VotingId");
            RenameColumn(table: "dbo.VotingDetails", name: "GiftId", newName: "Gift_GiftId");
            CreateIndex("dbo.VotingDetails", "Gift_GiftId");
            CreateIndex("dbo.VotingDetails", "Voting_VotingId");
        }
    }
}
