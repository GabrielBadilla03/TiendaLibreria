namespace Libreria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Jozzu : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Reseña", new[] { "CodigoCliente" });
            RenameColumn(table: "dbo.Reseña", name: "CodigoCliente", newName: "Usuario_Id");
            AlterColumn("dbo.Reseña", "Usuario_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Reseña", "Usuario_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Reseña", new[] { "Usuario_Id" });
            AlterColumn("dbo.Reseña", "Usuario_Id", c => c.String(nullable: false, maxLength: 128));
            RenameColumn(table: "dbo.Reseña", name: "Usuario_Id", newName: "CodigoCliente");
            CreateIndex("dbo.Reseña", "CodigoCliente");
        }
    }
}
