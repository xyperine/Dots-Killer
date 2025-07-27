namespace DotsKiller.UI.Automatons
{
    public interface IAutomatonFormatter
    {
        public string TickspeedFormatted { get; }
        public string TickspeedUnitsFormatted { get; }
        public string TickspeedSeparator { get; }
        public string ActionsPerTickFormatted { get; }
        public string ActionsPerTickUnitsFormatted { get; }
        public string ActionsPerTickSeparator { get; }
    }
}