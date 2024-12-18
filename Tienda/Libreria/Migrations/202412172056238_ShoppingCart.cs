namespace Libreria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShoppingCart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carritoes",
                c => new
                    {
                        CarritoId = c.Int(nullable: false, identity: true),
                        CodigoProducto = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CarritoId)
                .ForeignKey("dbo.Productoes", t => t.CodigoProducto)
                .Index(t => t.CodigoProducto);
            
            DropColumn("dbo.Ventas", "IdPedido");
            DropColumn("dbo.Ventas", "Subtotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ventas", "Subtotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Ventas", "IdPedido", c => c.Int(nullable: false));
            DropForeignKey("dbo.Carritoes", "CodigoProducto", "dbo.Productoes");
            DropIndex("dbo.Carritoes", new[] { "CodigoProducto" });
            DropTable("dbo.Carritoes");
        }
    }
}
