using System;
using Xunit;
using DynamicObj;

namespace CSharpTests
{
    public class InteropWorks
    {
        [Fact]
        public void Test1()
        {
            var obj1 =
                new ImmutableDynamicObj()
                .With("aa", 4)
                .With("bb", 10)
                .Without("aa")
                ;
            var obj2 = 
                new ImmutableDynamicObj()
                .With("bb", 10)
                ;
            Assert.Equal(obj1, obj2);
        }
    }
}
