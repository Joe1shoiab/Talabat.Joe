namespace Talabat.APIs.Extentions
{
    public static class SwaggerMiddleWareExtention
    {
        public static WebApplication UseSwaggerMiddleWare(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
