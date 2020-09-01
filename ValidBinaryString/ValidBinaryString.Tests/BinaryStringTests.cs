using System;
using Xunit;
using Shouldly;

namespace ValidBinaryString.Tests
{
    public class BinaryStringTests
    {
        [Fact]
        public void Should_throw_exception_when_string_is_not_a_valid_binary()
        {
            var binary = new BinaryString();

            Should.Throw<ArgumentException>(() =>
            {
                binary.IsGood("this is not a valid binary string");
            });
        }

        [Fact]
        public void Should_be_false_when_binary_string_is_zero()
        {
            var binary = new BinaryString();
            bool isGood = binary.IsGood("0");
            isGood.ShouldBeFalse();
        }

        [Theory]
        [InlineData("1")]
        [InlineData("11")]
        [InlineData("110")]
        [InlineData("1110")]
        [InlineData("11100")]
        [InlineData("11010")]
        public void Should_be_false_when_number_zeros_are_different_than_number_ones(string input)
        {
            var binary = new BinaryString();
            bool isGood = binary.IsGood(input);
            isGood.ShouldBeFalse();
        }

        [Theory]
        [InlineData("1001")]
        [InlineData("110001")]
        [InlineData("101001")]
        [InlineData("10010011")]
        public void Should_be_false_when_number_ones_are_less_than_number_zeros_in_any_prefix(string input)
        {
            var binary = new BinaryString();
            bool isGood = binary.IsGood(input);
            isGood.ShouldBeFalse();
        }

        [Theory]
        [InlineData("10")]
        [InlineData("1010")]
        [InlineData("1100")]
        [InlineData("110010")]
        public void Should_be_true_when_zeros_ones_same_count_and_ones_never_less_than_zeros_in_any_prefix(string input)
        {
            var binary = new BinaryString();
            bool isGood = binary.IsGood(input);
            isGood.ShouldBeTrue();
        }
    }
}
