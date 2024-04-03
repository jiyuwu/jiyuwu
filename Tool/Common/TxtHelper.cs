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
        public static void WriteError(Exception ex)
        {
            // 将异常信息写入文本文件
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_log.txt");
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
                    File.Delete(logFilePath);

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
    }
}
