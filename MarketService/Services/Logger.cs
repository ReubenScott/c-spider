using System;
using System.IO;
using System.Threading;

namespace Market.Services
{
    public static class Logger
    {
        private static readonly string _logfile = Config.logfile;

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private static readonly int logLevel = Config.LogLevel;

        // Define an enum named DebugLevel
        public enum Level
        {
            Debug,       // All debug messages
            Info,       // Informational messages, warnings, and errors
            Warning,    // Warnings and errors
            Error      // Error messages only
        }


        private static void Log(string message)
        {
            try
            {
                _semaphore.Wait();
                using (StreamWriter sw = new StreamWriter(_logfile, true))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {message}");
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it to another location or rethrow)
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
            finally
            {
                _semaphore.Release();
            }
        }


        public static void Debug(string message) {
            switch (logLevel)
            {
                case (int)Level.Debug:
                    Log($"[Info]: {message}");
                    break;
                default:
                    break;
            }
        }

        public static void Info(string message)
        {
            switch (logLevel)
            {
                case (int)Level.Debug:
                case (int)Level.Info:
                    Log($"[Info]: {message}");
                    break;
                default:
                    break;
            }
        }

        public static void Waring(string message)
        {
            switch (logLevel)
            {
                case (int)Level.Debug:
                case (int)Level.Info:
                case (int)Level.Warning:
                    Log($"[Waring]: {message}");
                    break;
                default:
                    break;
            }
        }

        public static void Error(string message)
        {
            switch (logLevel)
            {
                case (int)Level.Debug:
                case (int)Level.Info:
                case (int)Level.Warning:
                case (int)Level.Error:
                    Log($"[Error]: {message}");
                    break;
                default:
                    break;
            }
        }
    }

}

