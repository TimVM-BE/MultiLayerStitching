using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiLayerStitching.First
{
    [ExtendObjectType("Query")]
    public class FirstQueries
    {
        public string Foo() => "Bar";
    }
}
