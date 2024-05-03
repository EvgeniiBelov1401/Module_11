using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using VoiceTexterBot.Modules;
using VoiceTexterBot.Controllers;
using VoiceTexterBot.Services;
using VoiceTexterBot.Configuration;

namespace VoiceTexterBot
{
    internal class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding=Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder().ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }
        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings());

            services.AddSingleton<IFileHandler, AudioFileHandler>();

            services.AddSingleton<IStorage, MemoryStorage>();

            services.AddTransient<DefaultMessageController>();
            services.AddTransient<VoiceMessageController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("6777125585:AAEsltiy1v7CLEuV-rZ88x1scYojW9wps48"));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }
        static AppSettings BuildAppSettings()
        {

            return new AppSettings()
            {
                DownloadsFolder = @"C:\Users\evgen\Downloads",
                BotToken = "6777125585:AAEsltiy1v7CLEuV-rZ88x1scYojW9wps48",
                AudioFileName = "audio",
                InputAudioFormat = "ogg",
            };
        }
    }
}
