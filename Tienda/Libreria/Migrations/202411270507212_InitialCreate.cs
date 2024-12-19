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
                        Descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DisponibilidadInventario = c.Int(nullable: false),
                        Estado = c.String(nullable: false, maxLength: 50),
                        CategoriaId = c.Int(nullable: false),
                        ImagenProducto = c.Binary(),
                    })
                .PrimaryKey(t => t.CodigoProducto)
                .ForeignKey("dbo.Categorias", t => t.CategoriaId)
                .Index(t => t.CategoriaId);
                        
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
                .ForeignKey("dbo.HistorialVentas", t => t.HistorialVentasId)
                .ForeignKey("dbo.Productoes", t => t.CodigoProducto)
                .Index(t => t.CodigoProducto)
                .Index(t => t.HistorialVentasId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.Reseña",
                c => new
                {
                    ReseñaId = c.Int(nullable: false, identity: true),
                    CodigoProducto = c.Int(nullable: false),
                    CodigoCliente = c.String(nullable: false, maxLength: 128),
                    Comentario = c.String(nullable: false, maxLength: 1000),
                    Calificacion = c.Int(nullable: false),
                    FechaReseña = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.ReseñaId)
                .ForeignKey("dbo.AspNetUsers", t => t.CodigoCliente)
                .ForeignKey("dbo.Productoes", t => t.CodigoProducto)
                .Index(t => t.CodigoProducto)
                .Index(t => t.CodigoCliente);


        }

        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Ventas", "CodigoProducto", "dbo.Productoes");
            DropForeignKey("dbo.Ventas", "HistorialVentasId", "dbo.HistorialVentas");
            DropForeignKey("dbo.Reseña", "CodigoProducto", "dbo.Productoes");
            DropForeignKey("dbo.Reseña", "CodigoCliente", "dbo.AspNetUsers");
            DropForeignKey("dbo.Productoes", "CategoriaId", "dbo.Categorias");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Ventas", new[] { "HistorialVentasId" });
            DropIndex("dbo.Ventas", new[] { "CodigoProducto" });
            DropIndex("dbo.Reseña", new[] { "CodigoCliente" });
            DropIndex("dbo.Reseña", new[] { "CodigoProducto" });
            DropIndex("dbo.Productoes", new[] { "CategoriaId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Ventas");
            DropTable("dbo.HistorialVentas");
            DropTable("dbo.Reseña");
            DropTable("dbo.Productoes");
            DropTable("dbo.Categorias");
        }
    }
}
