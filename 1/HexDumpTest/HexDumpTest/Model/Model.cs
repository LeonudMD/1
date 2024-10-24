using System;
using System.IO;
using System.Text;

namespace HexDumpApp
{
    public class Model : IModel
    {
        public string GenerateHexDump(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл не найден.");

            byte[] bytes = File.ReadAllBytes(filePath);
            StringBuilder hexBuilder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i += 16)
            {
                StringBuilder line = new StringBuilder();

                // Адрес
                line.Append($"{i:X8}  ");

                // Hex-значения
                for (int j = 0; j < 16; j++)
                {
                    if (i + j < bytes.Length)
                        line.Append($"{bytes[i + j]:X2} ");
                    else
                        line.Append("   ");
                }

                line.Append(" ");

                // Символы
                for (int j = 0; j < 16 && i + j < bytes.Length; j++)
                {
                    byte b = bytes[i + j];
                    if (b >= 32 && b <= 126)
                        line.Append((char)b);
                    else
                        line.Append('.');
                }

                hexBuilder.AppendLine(line.ToString());
            }

            return hexBuilder.ToString();
        }
    }
}
