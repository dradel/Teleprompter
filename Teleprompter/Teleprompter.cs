using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Teleprompter
{
    public class Teleprompter
    {
        internal async Task RunTeleprompter()
        {
            var config = new TelePrompterConfig();

            var displayTask = ShowTeleprompter(config);
            var speedTask = GetInput(config);

            await Task.WhenAny(displayTask, speedTask);
        }

        private static async Task ShowTeleprompter(TelePrompterConfig config)
        {
            foreach (var word in ReadFrom(config.FilePath))
            {
                Console.Write(word);
                if (!string.IsNullOrWhiteSpace(word))
                {
                    await Task.Delay(config.DelayInMilliseconds);
                }
            }
            config.SetDone();
        }

        private static async Task GetInput(TelePrompterConfig config)
        {
            Action work = () =>
            {
                do
                {
                    switch (Console.ReadKey(true).KeyChar)
                    {
                        case '+':
                            config.UpdateDelay(-30);
                            break;
                        case '-':
                            config.UpdateDelay(30);
                            break;
                        case 'x':
                            config.SetDone();
                            break;
                    }
                } while (!config.Done);
            };
            await Task.Run(work);
        }
        private static IEnumerable<string> ReadFrom(string file)
        {
            string line;
            using var reader = File.OpenText(file);

            while ((line = reader.ReadLine()) != null)
            {
                var words = line.Split(' ');
                var lineLength = 0;
                foreach (var word in words)
                {
                    foreach (var letter in word.ToCharArray())
                    {
                        yield return letter.ToString();
                    }

                    yield return " ";

                    lineLength += word.Length + 1;
                    if (lineLength <= 50) continue;

                    yield return Environment.NewLine;
                    lineLength = 0;
                }
                yield return Environment.NewLine;
            }
        }
    }
}
