using System;
using System.Collections.Generic;
using System.Threading;

namespace GameLibrary.Helpers
{
    public static class StaticHelperClass
    {
        /// <summary>
        /// Prints exception to console, puts the thread to sleep for x seconds, then checks to clear console
        /// </summary>
        /// <param name="wait">Wait time in seconds</param>
        /// <param name="clearConsole">defaulted to true</param>
        public static void PrintException(Exception e, int wait = 1, bool clearConsole = true)
        {
            var milliSeconds = wait *= 1000;
            Console.WriteLine(e);
            Thread.Sleep(milliSeconds);
            if (clearConsole)
                Console.Clear();
        }

        public static int ReadWriteOptions(List<string> options)
        {
            for(var i = 0; i < options.Count; i++)
            {
                var option = options[i];
                Console.WriteLine($"{(i + 1)}. {option}");
            }

            return Convert.ToInt32(Console.ReadLine());
        }
    }
}
