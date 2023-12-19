using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                    if (!name.StartsWith("/sys/") && !name.StartsWith("/run/"))
                        str.AppendLine($"{disk.Name}: {Math.Round((float)disk.AvailableFreeSpace / disk.TotalSize * 100, 2)}%");
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