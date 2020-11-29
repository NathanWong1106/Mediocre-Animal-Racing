namespace Racing.Util
{
    /// <summary>
    /// Group of conversion methods -- Add more here when necessary
    /// </summary>
    public static class Converter
    {
        public static float MillisecondsToSeconds(float milliseconds)
        {
            return milliseconds / 1000f;
        }

        public static float SecondsToMilliseconds(float seconds)
        {
            return seconds * 1000f;
        }
    }
}
