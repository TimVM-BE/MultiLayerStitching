using HotChocolate.Types;

namespace MultiLayerStitching.Third
{
    [ExtendObjectType("Query")]
    public class ThirdQueries
    {
        public string Hello(string input) => $"Hello {input}";
    }
}
