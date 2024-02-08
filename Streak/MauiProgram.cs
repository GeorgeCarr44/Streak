using Microsoft.Extensions.Logging;
using Streak.Data;
using Streak.Views;

namespace Streak
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>();
            builder.ConfigureFonts(fonts =>
            {
                fonts.AddFont("Roboto-Medium", "Roboto");
                fonts.AddFont("Roboto-Bold", "RobotoBold");
                fonts.AddFont("Roboto-BoldItalic", "RobotoBoldItalic");
                fonts.AddFont("Roboto-BlackItalic", "RobotoBlackItalic");
                fonts.AddFont("Roboto-BlackItalic", "RobotoBlackItalic");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Vegapunk.ttf", "Vegapunk");
            });

            //Pages
            builder.Services.AddSingleton<GoalsPage>();
            builder.Services.AddTransient<EditGoalPage>();


            //Database
            builder.Services.AddSingleton<GoalsDatabase>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
