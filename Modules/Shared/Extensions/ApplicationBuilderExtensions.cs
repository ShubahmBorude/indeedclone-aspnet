namespace IndeedClone.Modules.Shared.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHomepageRedirect(this IApplicationBuilder app, IConfiguration config)
        {
            var targetPath = config["Homepage:DefaultPath"] ?? "/IndeedClone/Retrieve";

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect(targetPath);
                    return;
                }
                await next();
            });

            return app;

        }

    }
}
