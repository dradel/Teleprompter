using System.IO;
using static System.Math;

namespace Teleprompter
{
    internal class TelePrompterConfig
    {
        private readonly object _lockHandle = new object();
        public int DelayInMilliseconds { get; private set; } = 100;
        public bool Done { get; private set; }

        public string FilePath { get; } = Path.Combine(Directory.GetCurrentDirectory(), "Input.txt");

        public void UpdateDelay(int increment)
        {
            lock (_lockHandle)
            {
                DelayInMilliseconds = Max(Min(DelayInMilliseconds + increment, 1000), 20);
            }
        }

        public void SetDone() => Done = true;
    }
}
