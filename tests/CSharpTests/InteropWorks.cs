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
                .AddItem("aa", 4)
                .AddItem("bb", 10)
                .RemoveItem("aa")
                ;
            var obj2 = 
                new ImmutableDynamicObj()
                .AddItem("bb", 10)
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
                .AddItem("aaa", 5);
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
                .AddItem("aaa", 5)
                .RemoveItem("aaa");
            Assert.Equal(100500, obj1.Aaa);
        }
    }
}
