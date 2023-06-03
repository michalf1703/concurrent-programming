using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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

        public void addToBuffer(IBall ball)
        {
            string time = DateTime.Now.ToString("h:mm:ss tt");
            string log = time + " Ball "
                    + ball.Id
                    + " moved: "
                    + " PositionX: "
                    + Math.Round(ball.Position.X, 4)
                    + " PositionY: "
                    + Math.Round(ball.Position.Y, 4)
                    + " SpeedX: "
                    + Math.Round(ball.Speed.X, 4)
                    + " SpeedY: "
                    + Math.Round(ball.Speed.Y, 4);

            buffer.Add(log);
        }

        public void Dispose()
        {
            sw.Dispose();
            fileWritter.Dispose();
        }

        public void writter()
        {
            //sw = new StreamWriter("../../../../../Data/log.txt", append: true);
            sw = new StreamWriter("../../../../Data/log.txt", append: false);
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