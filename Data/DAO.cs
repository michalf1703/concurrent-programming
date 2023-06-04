using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Numerics;

namespace Data
{
    internal class DAO : IDisposable
    {
        private BlockingCollection<String> buffer = new BlockingCollection<String>();
        private Task fileWritter;
        private StreamWriter sw;

        public DAO()
        {
            fileWritter = new Task(() => writter());
            fileWritter.Start();
        }

        public void addToBuffer(Ball ball)
        {
            string time = DateTime.Now.ToString("h:mm:ss tt");
            string log = time + " Ball "
                        + ball.Id
                        + " moved: "
                        + " Position: "
                        + FormatVector(ball.Position)
                        + " Speed: "
                        + FormatVector(ball.Speed);

            buffer.Add(log);
        }

        private string FormatVector(Vector2 vector)
        {
            return "X: " + Math.Round(vector.X, 4) + " Y: " + Math.Round(vector.Y, 4);
        }


        public void Dispose()
        {
            sw.Dispose();
            fileWritter.Dispose();
            
        }
        public void writter()
        {
            sw = null;
            while (sw == null)
            {
                try
                {
                    sw = new StreamWriter("../../../../Data/log.txt", append: false);
                }
                catch (IOException)
                {
                    // Plik jest używany przez inny proces, oczekiwanie przed ponownym otwarciem
                    Thread.Sleep(1000);
                }
            }

            try
            {
                foreach (string i in buffer.GetConsumingEnumerable())
                {
                    sw.WriteLine(i);
                }
            }
            finally
            {
                Dispose();
            }
        }

        public void wrtiteToFile(string log)
        {
            try
            {
                sw.WriteLine(log);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The file cannot be found.");
            }
            catch (IOException)
            {
                Console.WriteLine("An I/O error has occurred.");
            }
        }
    }
}