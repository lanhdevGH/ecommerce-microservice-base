using Product.API.Entities;

namespace Product.API.Persistences
{
    public class ProductContextSeed
    {
        public static async Task SeedProductAsync(ProductContext context, ILogger logger)
        {
            try
            {
                if (!context.Products.Any())
                {
                    context.Products.AddRange(GetPreconfiguredProducts());
                    await context.SaveChangesAsync();
                }
                else
                {
                    logger.LogInformation("Data already exists");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
        private static IEnumerable<CatalogProduct> GetPreconfiguredProducts() =>
        [
            new()
            {
                No = "Lotus",
                Name = "Esprit",
                Summary = "Nondisplaced fracture of greater trochanter of right femur",
                Description = "Nondisplaced fracture of greater trochanter of right femur",
                Price = (decimal)177940.49
            },
            new()
            {
                No = "Cadillac",
                Name = "CTS",
                Summary = "Carbuncle of trunk",
                Description = "Carbuncle of trunk",
                Price = (decimal)114728.21
            }
        ];
    }
}
