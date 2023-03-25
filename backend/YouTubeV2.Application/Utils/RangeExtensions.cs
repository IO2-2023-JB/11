namespace YouTubeV2.Application.Utils
{
    public static class RangeExtensions
    {
        public static Range FromString(string input)
        {
            string[] range = input.Split('-');
            if (range.Length != 2)
                throw new ArgumentException($"Expected input argument format is int1-int2. Recieved input argument: {input}");
            if (!int.TryParse(range[0], out int start))
                throw new ArgumentException($"Expected input argument format is int1-int2. In recieved input ({input}) int1 can not be converted to an integer");
            if (!int.TryParse(range[1], out int end))
                throw new ArgumentException($"Expected input argument format is int1-int2. In recieved input ({input}) int2 can not be converted to an integer");
            return new Range(start, end);
        }
    }
}
