using Microsoft.Extensions.DependencyInjection;
using ProductovichokBot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using ProductovichokBot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using MySql.Data.MySqlClient;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("ПродуктовичокBot");

        string connectionString = "server=rc1b-kspwzb8gf9wxum7u.mdb.yandexcloud.net;user=kek;password=productovichok2116;database=productovichok";
        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("ебать");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        var botClient = new TelegramBotClient("6198240767:AAEc1CO7BTwcMvSd37QT6d9YqYSzW9De85o");
        botClient.StartReceiving(Update, Error);
        Console.ReadLine();
    }

    public static string CodeGenerate()
    {
        Random generator = new Random();
        string code = generator.Next(0, 1000000).ToString("D6");
        return code;
    }

    async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        var message = update.Message;
        TimeSpan times = DateTime.UtcNow - update.Message.Date;

        if (times.TotalMinutes > 1)
            Console.WriteLine("skipping old update");
        else if (!string.IsNullOrWhiteSpace(message.Text))
        {
            if (message.Text.ToLower() == "/reg")
            {
                if (UserService.RegistrationCheck((int)message.Chat.Id))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Вы уже зарегистрированы. Введите команду '/code', чтобы получить проверочный код для авторизации. ");
                    return;
                }
                else
                {

                    UserService.Registration((int)message.Chat.Id, message.Chat.Username, message.Chat.FirstName);
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Вы успешно зарегистрировались. Введите команду '/code', чтобы получить проверочный код для авторизации.");
                    return;
                }
            }
            else if (message.Text.ToLower() == "/code")
            {
                if (UserService.AuthorizationCheck((int)message.Chat.Id))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Вы уже получили код для авторизации.");
                    return;
                }
                else
                {
                    string code = CodeGenerate();
                    UserService.Authorization((int)message.Chat.Id, Int32.Parse(code));
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Ваш код: `{code}`. Код действует в течение 5 минут. " +
                        $"Новый можно будет получить, когда данный код перестанет быть действительным ", parseMode: ParseMode.Markdown);
                    return;
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Такой комманды не существует. " +
                    "Если вы не зарегистрированы, то сделайте это с помощью комманды '/reg' или же получите проверочный код для авторизации с помощью комманды '/code'");
                return;
            }
        }
    }
    async static Task Error(ITelegramBotClient botClient, Exception arg2, CancellationToken arg3)
    {

    }

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = "server=192.168.0.123;user=kek;password=1234;database=productovichok";
        var serverVersion = new MySqlServerVersion(new Version(10, 4, 28));
        services.AddDbContext<ProductovichokContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(connectionString, serverVersion)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
        );
    }
}