using System.Net;
using System.Text;

namespace FreeSpacePercentBot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Incorrect");
            return;
        }

        if (!long.TryParse(args[1], out var chatId))
        {
            Console.WriteLine("Incorrect chatId");
            return;
        }

        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cancellationTokenSource.Cancel();
        };
        using var periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(15));
        var botClient = new TelegramBotClient(args[0], chatId);
        while (!token.IsCancellationRequested)
        {
            try
            {
                var hostname = Dns.GetHostName();
                var disks = DriveInfo.GetDrives();
                var str = new StringBuilder();
                str.AppendLine($"[{hostname}]");
                foreach (var disk in disks)
                {
                    var name = disk.Name;
                    if (name.StartsWith("/sys/") || name.StartsWith("/run/") || name.StartsWith("/var")) 
                        continue;
                    var diskSpacePercent = Math.Round((double)disk.AvailableFreeSpace / disk.TotalSize * 100, 2);
                    if (double.IsNaN(diskSpacePercent))
                        continue;
                    str.AppendLine($"{disk.Name}: {diskSpacePercent}%");
                }

                await botClient.SendMessageAsync(str.ToString(), token);
                await periodicTimer.WaitForNextTickAsync(token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }
}