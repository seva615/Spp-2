using FakerLib.Fakers;
using FakerLib.Fakers.Impl;
using System;
using System.Collections.Generic;
using Xunit;

namespace FakerTests
{     
    public class TestFakers
    {

        [Fact]
        public void TestFaker()
        {
            var faker = new Faker();

            double value = faker.Create<double>();
            Assert.True(double.IsFinite(value));

            var list = faker.Create<List<List<DateTime>>>();
            Assert.NotNull(list);
            Assert.NotEmpty(list);


            double[] numbers = faker.Create<double[]>();
            Assert.NotNull(numbers);
            Assert.NotEmpty(numbers);

        }
    }
}
