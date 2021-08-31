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

        public class MyDynamicObjectWithField : ImmutableDynamicObj
        {
            public int Aaa { get; init; }
        }

        [Fact]
        public void TestFieldsPreservedForInheritor()
        {
            var obj1 =
                new MyDynamicObjectWithField() { Aaa = 100500 }
                .With("aaa", 5)
                .Without("aaa");
            Assert.Equal(100500, obj1.Aaa);
        }
    }
}
