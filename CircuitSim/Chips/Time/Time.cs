namespace CircuitSim.Chips.Time
{
    public class Ticks : Component
    {
        private static uint count = 0;

        public Outputs<long> Outputs;
        private long _ticks;

        public Ticks() : base($"Ticks{count++}")
        {
            Outputs = new Outputs<long>(this);
        }

        public override void Compute()
        {
            _ticks = System.DateTime.Now.Ticks;
        }

        public override void Set()
        {
            Outputs.Out.Value = _ticks;
        }

        public override void Detach()
        {
            Outputs.Detach();
        }

    }


}
