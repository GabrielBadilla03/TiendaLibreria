namespace Libreria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Users : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reseña", "CodigoCliente", "dbo.Clientes");
            DropIndex("dbo.Reseña", new[] { "CodigoCliente" });
            AlterColumn("dbo.Reseña", "CodigoCliente", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Reseña", "CodigoCliente");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reseña", "CodigoCliente", "dbo.AspNetUsers");
            DropIndex("dbo.Reseña", new[] { "CodigoCliente" });
            AlterColumn("dbo.Reseña", "CodigoCliente", c => c.Int(nullable: false));
            CreateIndex("dbo.Reseña", "CodigoCliente");
        }
    }
}
