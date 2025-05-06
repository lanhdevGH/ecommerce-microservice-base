using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Product.API.Persistences;

//public class ProductContextFactory : IDesignTimeDbContextFactory<ProductContext>
//{
//    public ProductContext CreateDbContext(string[] args)
//    {
//        var optionsBuilder = new DbContextOptionsBuilder<ProductContext>();
//        var connectionString = "Server=localhost;Port=3306;Database=ProductDB;Uid=root;Pwd=Passw0rd!;";

//        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), e =>
//        {
//            e.MigrationsAssembly("Product.API");
//            e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
//        });

//        return new ProductContext(optionsBuilder.Options);
//    }
//}
