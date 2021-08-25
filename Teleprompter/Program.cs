using System;
using System.IO;
using System.Threading.Tasks;

namespace Teleprompter
{
    public class Program
    {
        private static async Task Main(string[] args)
        {

            await new Teleprompter().RunTeleprompter();
        }
    }
}
