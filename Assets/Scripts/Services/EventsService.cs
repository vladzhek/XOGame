using System;

namespace Services
{
    public class EventsService
    {
        public event Action OnEndGame;

        public void OnEndGameInvoke()
        {
            OnEndGame?.Invoke();
        }
    }
}