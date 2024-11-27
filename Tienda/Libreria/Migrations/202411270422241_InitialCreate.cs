namespace Libreria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        CategoriaId = c.Int(nullable: false, identity: true),
                        NombreCategoria = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.CategoriaId);
            
            CreateTable(
                "dbo.Productoes",
                c => new
                    {
                        CodigoProducto = c.Int(nullable: false, identity: true),
                        NombreProducto = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                        ImagenProducto = c.Binary(nullable: false, maxLength: 500),
                        Descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DisponibilidadInventario = c.Int(nullable: false),
                        Estado = c.String(nullable: false, maxLength: 50),
                        CategoriaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CodigoProducto)
                .ForeignKey("dbo.Categorias", t => t.CategoriaId, cascadeDelete: true)
                .Index(t => t.CategoriaId);
            
            CreateTable(
                "dbo.Reseña",
                c => new
                    {
                        ReseñaId = c.Int(nullable: false, identity: true),
                        CodigoProducto = c.Int(nullable: false),
                        CodigoCliente = c.Int(nullable: false),
                        Comentario = c.String(nullable: false, maxLength: 1000),
                        Calificacion = c.Int(nullable: false),
                        FechaReseña = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReseñaId)
                .ForeignKey("dbo.Clientes", t => t.CodigoCliente, cascadeDelete: true)
                .ForeignKey("dbo.Productoes", t => t.CodigoProducto, cascadeDelete: true)
                .Index(t => t.CodigoProducto)
                .Index(t => t.CodigoCliente);
            
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        ClienteId = c.Int(nullable: false, identity: true),
                        NombreCompleto = c.String(nullable: false, maxLength: 100),
                        CorreoElectronico = c.String(nullable: false, maxLength: 100),
                        Telefono = c.String(nullable: false, maxLength: 20),
                        Direccion = c.String(nullable: false, maxLength: 200),
                        Contrasena = c.String(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ClienteId);
            
            CreateTable(
                "dbo.HistorialVentas",
                c => new
                    {
                        HistorialVentasId = c.Int(nullable: false, identity: true),
                        FechaRegistro = c.DateTime(nullable: false),
                        CantidadProductosVendidos = c.Int(nullable: false),
                        GananciaTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.HistorialVentasId);
            
            CreateTable(
                "dbo.Ventas",
                c => new
                    {
                        IdDetalle = c.Int(nullable: false, identity: true),
                        IdPedido = c.Int(nullable: false),
                        CodigoProducto = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        PrecioUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HistorialVentasId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdDetalle)
                .ForeignKey("dbo.HistorialVentas", t => t.HistorialVentasId, cascadeDelete: true)
                .ForeignKey("dbo.Productoes", t => t.CodigoProducto, cascadeDelete: true)
                .Index(t => t.CodigoProducto)
                .Index(t => t.HistorialVentasId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ventas", "CodigoProducto", "dbo.Productoes");
            DropForeignKey("dbo.Ventas", "HistorialVentasId", "dbo.HistorialVentas");
            DropForeignKey("dbo.Reseña", "CodigoProducto", "dbo.Productoes");
            DropForeignKey("dbo.Reseña", "CodigoCliente", "dbo.Clientes");
            DropForeignKey("dbo.Productoes", "CategoriaId", "dbo.Categorias");
            DropIndex("dbo.Ventas", new[] { "HistorialVentasId" });
            DropIndex("dbo.Ventas", new[] { "CodigoProducto" });
            DropIndex("dbo.Reseña", new[] { "CodigoCliente" });
            DropIndex("dbo.Reseña", new[] { "CodigoProducto" });
            DropIndex("dbo.Productoes", new[] { "CategoriaId" });
            DropTable("dbo.Ventas");
            DropTable("dbo.HistorialVentas");
            DropTable("dbo.Clientes");
            DropTable("dbo.Reseña");
            DropTable("dbo.Productoes");
            DropTable("dbo.Categorias");
        }
    }
}
