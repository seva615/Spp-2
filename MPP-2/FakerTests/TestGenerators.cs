using ArrayGeneratorPlugin;
using DoubleGeneratorPlugin;
using FakerLib.Generator;
using FakerLib.Generator.Impl;
using System;
using System.Collections.Generic;
using Xunit;

namespace FakerTests
{
    public class TestGenerators
    {
        [Fact]
        public void TestDateGenerator()
        {
            IGenerator generator = new DateGenerator();
            Assert.True(generator.CanGenerate(typeof(DateTime)));
            Assert.False(generator.CanGenerate(typeof(int)));
        }

        [Fact]
        public void TestIntGenerator()
        {
            IGenerator generator = new IntGenerator();
            Assert.True(generator.CanGenerate(typeof(int)));
            Assert.False(generator.CanGenerate(typeof(double)));
        }

        [Fact]
        public void TestListGenerator()
        {
            IGenerator generator = new ListGenerator();
            Assert.True(generator.CanGenerate(typeof(List<List<DateGenerator>>)));
            Assert.False(generator.CanGenerate(typeof(DateTime)));
        }


        [Fact]
        public void TestDoubleGenerator()
        {
            IGenerator generator = new DoubleGenerator();
            Assert.True(generator.CanGenerate(typeof(double)));
            Assert.False(generator.CanGenerate(typeof(DateTime)));
        }


        [Fact]
        public void TestArrayGenerator()
        {
            IGenerator generator = new ArrayGenerator();
            Assert.True(generator.CanGenerate(typeof(DoubleGenerator[][])));
            Assert.False(generator.CanGenerate(typeof(DateTime)));
        }

    }
}
