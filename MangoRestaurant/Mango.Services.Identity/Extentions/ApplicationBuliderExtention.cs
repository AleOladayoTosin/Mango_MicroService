using Mango.Services.Identity.Initializer;

namespace Mango.Services.Identity.Extentions
{
    public static class ApplicationBuliderExtention
    {
        public static void ExtendApplicationService(this IApplicationBuilder app, IDbInitializer dbInitializer)
        {
            dbInitializer.Initialize();
        }
    }
}
