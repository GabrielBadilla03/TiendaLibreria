namespace Libreria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImgByte_LengthEdit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Productoes", "ImagenProducto", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Productoes", "ImagenProducto", c => c.Binary(nullable: false, maxLength: 500));
        }
    }
}
