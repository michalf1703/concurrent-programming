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
        private bool boardSizeAdded = false;

        public DAO()
        {
            fileWritter = new Task(() => writter());
            fileWritter.Start();
        }

        public void addToBuffer(Ball ball)
        {
            if (boardSizeAdded == false) { 
            string board = "Boardz Size: " + ball.board_size + "x" + ball.board_size;
            buffer.Add(board);
                boardSizeAdded = true;
            }

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

        internal void addSizeBoardToBuffer(Ball ball) {
            string board = "Boardz Size: " + ball.board_size + "x" + ball.board_size;
            buffer.Add(board);
        }

        public void Dispose()
        {
            sw.Dispose();
            fileWritter.Dispose();
            
        }
        public void writter()
        {
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