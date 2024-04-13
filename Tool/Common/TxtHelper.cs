using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class TxtHelper
    {
        const string LOG_ERROR = "error";
        const string LOG_INFO = "info";
        public static void WriteError(Exception ex)
        {
            dirCheck();
            // 将异常信息写入文本文件
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log/{LOG_ERROR}_log.txt");
            string logHistoryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log/{LOG_ERROR}_log_history.txt");
            if (!File.Exists(logFilePath))
            {
                // 如果日志文件不存在，则创建
                using (StreamWriter writer = File.CreateText(logFilePath))
                {
                    writer.WriteLine($"Error log created at {DateTime.Now}");
                }
            }
            else
            {
                long fileSize = new FileInfo(logFilePath).Length;
                if (fileSize > 1048576) // 1MB = 1048576 bytes
                {
                    if (File.Exists(logHistoryFilePath))
                    {
                        File.Delete(logHistoryFilePath);
                    }
                    // 新日志文件转为旧日志文件
                    File.Move(logFilePath, logHistoryFilePath);

                    // 创建新的日志文件
                    using (StreamWriter writer = File.CreateText(logFilePath))
                    {
                        writer.WriteLine($"Error log created at {DateTime.Now}");
                    }
                }
            }
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"Error occurred at {DateTime.Now}: {ex.Message}");
                writer.WriteLine($"StackTrace: {ex.StackTrace}");
                writer.WriteLine();
            }
        }
        public static void WriteError(string ex)
        {
            dirCheck();
            // 将异常信息写入文本文件
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log/{LOG_ERROR}_log.txt");
            string logHistoryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log/{LOG_ERROR}_log_history.txt");
            if (!File.Exists(logFilePath))
            {
                // 如果日志文件不存在，则创建
                using (StreamWriter writer = File.CreateText(logFilePath))
                {
                    writer.WriteLine($"Error log created at {DateTime.Now}");
                }
            }
            else
            {
                long fileSize = new FileInfo(logFilePath).Length;
                if (fileSize > 1048576) // 1MB = 1048576 bytes
                {
                    if (File.Exists(logHistoryFilePath))
                    {
                        File.Delete(logHistoryFilePath);
                    }
                    // 新日志文件转为旧日志文件
                    File.Move(logFilePath, logHistoryFilePath);

                    // 创建新的日志文件
                    using (StreamWriter writer = File.CreateText(logFilePath))
                    {
                        writer.WriteLine($"Error log created at {DateTime.Now}");
                    }
                }
            }
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {ex}");
                writer.WriteLine();
            }
        }
        public static void WriteInfo(string str)
        {
            dirCheck();
            // 将异常信息写入文本文件
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log/{LOG_INFO}_log.txt");
            string logHistoryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log/{LOG_INFO}_log_history.txt");
            if (!File.Exists(logFilePath))
            {
                // 如果日志文件不存在，则创建
                using (StreamWriter writer = File.CreateText(logFilePath))
                {
                    writer.WriteLine($"Error log created at {DateTime.Now}");
                }
            }
            else
            {
                long fileSize = new FileInfo(logFilePath).Length;
                if (fileSize > 1048576) // 1MB = 1048576 bytes
                {
                    if (File.Exists(logHistoryFilePath))
                    {
                        File.Delete(logHistoryFilePath);
                    }
                    // 新日志文件转为旧日志文件
                    File.Move(logFilePath, logHistoryFilePath);

                    // 创建新的日志文件
                    using (StreamWriter writer = File.CreateText(logFilePath))
                    {
                        writer.WriteLine($"{LOG_INFO} log created at {DateTime.Now}");
                    }
                }
            }
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {str}");
                writer.WriteLine();
            }
        }
        private static void dirCheck()
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log/");
            if (!Directory.Exists(folderPath))
            {
                // 如果文件夹不存在，则创建
                Directory.CreateDirectory(folderPath);
            }
        }
    }
}
