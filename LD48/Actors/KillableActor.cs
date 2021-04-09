namespace LD48.Actors
{
    public class KillableActor : GameActor
    {
        public int CurrentHP { get; set; }
        public int MaxHP { get; set; }

        public bool IsAlive => CurrentHP > 0;
    }
}