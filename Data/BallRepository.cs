using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Data.IBall;

namespace Data
{
    internal class BallRepository
    {
        public List<Ball> balls { get; set; } // Lista wszystkich piłek utworzonych w repozytorium
        public int BoardSize { get; private set; } = 515; // Rozmiar planszy gry, który jest stały i wynosi 515

        public BallRepository()
        {
            balls = new List<Ball>(); // Inicjalizacja listy piłek
        }

        // Tworzy określoną liczbę piłek i dodaje je do listy
        public void CreateBalls(int ballsAmount)
        {
            for (int i = 0; i < ballsAmount; i++)
            {
                balls.Add(new Ball(i + 1));
            }
        }

        // Pobiera piłkę na podstawie jej unikalnego identyfikatora
        public Ball GetBall(int ballId)
        {
            return balls[ballId - 1];
        }
    }
}
