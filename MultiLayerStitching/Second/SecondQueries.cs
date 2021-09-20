using HotChocolate.Types;

namespace MultiLayerStitching.Second
{
    [ExtendObjectType("Query")]
    public class SecondQueries
    {
        public string Bar => "Foo";
    }
}
