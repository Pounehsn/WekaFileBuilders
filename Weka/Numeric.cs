namespace Weka
{
    public class Numeric : WekaTypeBase
    {
        private Numeric() { }

        public static WekaTypeBase Instance = new Numeric();
        public override string ToString() => "numeric";
    }
}