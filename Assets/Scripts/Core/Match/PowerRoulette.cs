using static Core.Events;

namespace Core
{
    public interface IPowerRoulette
    {
        public void Launch();
    }

    public class PowerRoulette : IPowerRoulette
    {
        public PowerRouletteResult OnResult;

        public void Launch()
        {
            

            OnResult?.Invoke();
        }
    }

    public interface IPower
    {
        public void Perform();
    }

    public abstract class Power
    {
        string Name;

        public abstract void Perform();
    }
}