using System;
using Xunit;
using DynamicObj;
using Microsoft.FSharp.Collections;
using System.Collections.Generic;
using System.Linq;

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

        public class MyDynamicObject : ImmutableDynamicObj
        {
            public MyDynamicObject(FSharpMap<string, object> map) : base(map) { }
            public MyDynamicObject() : base(new(Enumerable.Empty<Tuple<string, object>>())) { }
        }

        [Fact]
        public void Test2()
        {
            var obj1 =
                new MyDynamicObject()
                .With("aaa", 5);
            Assert.IsType<MyDynamicObject>(obj1);
            Assert.Equal(5, obj1["aaa"]);
        }
    }
}
