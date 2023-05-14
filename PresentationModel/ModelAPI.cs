using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive;
using System.Text;
using Logic;

namespace Model
{
    public interface IBall : INotifyPropertyChanged
    {
        double Top { get; } // Górna pozycja piłki
        double Left { get; } // Lewa pozycja piłki
        int Diameter { get; } // Średnica piłki
    }

    public class BallChaneEventArgs : EventArgs  // Argumenty niestandardowego zdarzenia BallChanged
    {
        public IBall Ball { get; set; } // Piłka, która wywołała zdarzenie
    }

    public abstract class ModelAPI : IObservable<IBall> // Abstrakcyjna klasa bazowa ModelAPI, która umożliwia obserwowanie piłek
    {
        public static ModelAPI CreateApi() // Metoda fabrykująca służąca do tworzenia instancji ModelAPI
        {
            return new ModelBall();
        }

        public abstract void AddBallsAndStart(int ballsAmount); // Metoda dodająca piłki do modelu i uruchamiająca ich ruch

        #region IObservable

        public abstract IDisposable Subscribe(IObserver<IBall> observer); // Metoda subskrypcji zdarzeń ruchu piłek

        #endregion IObservable

        internal class ModelBall : ModelAPI  // Implementacja klasy ModelAPI dla modelu piłki
        {
            private LogicAPI logicApi;      // Instancja API logiki służąca do komunikacji z warstwą logiczną modelu
            public event EventHandler<BallChaneEventArgs> BallChanged;  // Obserwowalne zdarzenie wywołujące informowanie obserwatorów o zdarzeniach ruchu piłek

            private IObservable<EventPattern<BallChaneEventArgs>> eventObservable = null; // Obserwowalne zdarzenie wywołujące informowanie obserwatorów o zdarzeniach ruchu piłek
            private List<BallInModel> Balls = new List<BallInModel>();  // Lista piłek obecnie w modelu

            public ModelBall()
            {
                logicApi = logicApi ?? LogicAPI.CreateLayer(); // Tworzenie API logiki, jeśli jeszcze nie zostało utworzone
                // Subskrypcja do API logiki w celu otrzymania aktualizacji dotyczących ruchu piłki
                IDisposable observer = logicApi.Subscribe<int>(x => Balls[x - 1].Move(logicApi.getBallPositionX(x), logicApi.getBallPositionY(x)));
                eventObservable = Observable.FromEventPattern<BallChaneEventArgs>(this, "BallChanged"); // Tworzenie obserwowalnego zdarzenia, które będzie służyło do informowania obserwatorów o zdarzeniach ruchu piłek
            }

            // Metoda dodająca określoną ilość piłek do modelu i uruchamiająca ich ruch
            public override void AddBallsAndStart(int ballsAmount)
            {
                logicApi.AddBallsAndStart(ballsAmount);
                for (int i = 1; i <= ballsAmount; i++)
                {
                    // Tworzenie nowej instancji BallInModel dla każdej piłki w modelu
                    BallInModel newBall = new BallInModel(logicApi.getBallPositionX(i), logicApi.getBallPositionY(i), logicApi.getBallRadius(i));
                    Balls.Add(newBall); // Dodawanie nowej piłki do listy piłek
                }
                // Wywołanie zdarzenia BallChanged dla każdej piłki w modelu
                foreach (BallInModel ball in Balls)
                {
                    BallChanged?.Invoke(this, new BallChaneEventArgs() { Ball = ball });
                }


            }
            // Metoda zwracająca IDisposable, która służy do subskrybowania obserwatora do eventObservable,
            // czyli obiektu typu IObservable<EventPattern<BallChaneEventArgs>>
            public override IDisposable Subscribe(IObserver<IBall> observer)
            {
                // Subskrybowanie obserwatora do eventObservable i zwrócenie IDisposable, który może zostać użyty do wyrejestrowania subskrypcji w przyszłości
                return eventObservable.Subscribe(x => observer.OnNext(x.EventArgs.Ball), ex => observer.OnError(ex), () => observer.OnCompleted());
            }
        }
    }
}