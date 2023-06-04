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
        private readonly Mutex mutexBufor = new Mutex();
        private readonly Mutex mutexFile = new Mutex();
        private bool boardSizeAdded = false;

        public DAO()
        {
            // Tworzenie nowego zadania dla writter() i uruchamianie go w tle.
            fileWritter = new Task(() => writter());
            fileWritter.Start();
        }

        // Dodawanie obiektu Ball do bufora.
        public void addToBuffer(Ball ball)
        {
           
            // Sprawdzanie, czy identyfikator piłki wynosi 1 i czy rozmiar planszy nie został jeszcze dodany.
            if (ball.Id == 1 && boardSizeAdded == false)
            {
                string board = "Board Size: " + ball.board_size + "x" + ball.board_size;
                buffer.Add(board);
                boardSizeAdded = true;
            }

            mutexBufor.WaitOne();
            try
            {
                // Tworzenie logu z informacjami o piłce.
                string time = DateTime.Now.ToString("h:mm:ss tt");
                string log = time + " Ball "
                            + ball.Id
                            + " moved: "
                            + " Position: "
                            + FormatVector(ball.Position)
                            + " Speed: "
                            + FormatVector(ball.Speed);

                // Dodawanie logu do bufora.
                buffer.Add(log);
            }
            finally { 
                mutexBufor.ReleaseMutex(); 
            }
        }

        // Formatowanie wektora.
        private string FormatVector(Vector2 vector)
        {
            return "X: " + Math.Round(vector.X, 4) + " Y: " + Math.Round(vector.Y, 4);
        }

        public void Dispose()
        {
            // Zwalnianie zasobów.
            sw.Dispose();
            fileWritter.Dispose();
        }

        // Metoda odpowiedzialna za zapisywanie logów do pliku.
        public void writter()
        {
            // Tworzenie obiektu StreamWriter dla pliku log.txt.
            sw = new StreamWriter("../../../../Data/log.txt", append: false);

            mutexFile.WaitOne();
            try
            {
                // Przechodzenie przez elementy bufora i zapisywanie ich do pliku.
                foreach (string i in buffer.GetConsumingEnumerable())
                {
                    sw.WriteLine(i);
                }
            }
            finally
            {
                // Zwalnianie zasobów po zakończeniu zapisu.
                Dispose();
                mutexFile.ReleaseMutex();
            }
        }
    }
}
