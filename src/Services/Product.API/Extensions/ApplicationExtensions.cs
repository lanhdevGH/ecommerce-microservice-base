using Microsoft.AspNetCore.Builder;

namespace Product.API.Extensions
{
    public static class ApplicationExtensions
    {
        public static void UseInfrastructure(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();  // For production, you might want to enable HTTPS redirection

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
