using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.IO;

namespace ASCII_Project
{
    class Program
    {
        private const double WIDTH_OFFSET = 1.7;
        private const int MAX_WIDTH = 474;
        [STAThread]
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Images | * .bmp; *.png; *.jpg; *.JPEG"
            };

            Console.WriteLine("Нажмите Enter для запуска");

            while (true)
            {
                Console.ReadLine();

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    continue;

                Console.Clear();

                var bitMap = new Bitmap(openFileDialog.FileName);
                bitMap = ResizeBitmap(bitMap);
                bitMap.ToGrayScale();

                var convert = new BitmapToASCIIConverter(bitMap);
                var rows = convert.Convert();

                foreach ( var row in rows)
                    Console.WriteLine(row);

                var rowNegative = convert.ConvertNegative();

                File.WriteAllLines($@"C:\Users\allap\OneDrive\Рабочий стол\Фото от бота\{openFileDialog.SafeFileName.Replace(".jpg", ".txt")}", rowNegative.Select(r => new string(r)));

                Console.SetCursorPosition(0, 0);

            }

        }
        private static Bitmap ResizeBitmap(Bitmap bitmap)
        {
            var maxHieght = bitmap.Height / WIDTH_OFFSET * MAX_WIDTH / bitmap.Width;
            if (bitmap.Width > MAX_WIDTH || bitmap.Height > maxHieght)
                bitmap = new Bitmap(bitmap, new Size(MAX_WIDTH, (int)maxHieght));
            return bitmap;
        }
    }
}
