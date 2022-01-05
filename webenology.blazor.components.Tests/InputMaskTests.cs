using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webenology.blazor.components.Helpers;
using Xunit;

namespace webenology.blazor.components.Tests
{
    public class InputMaskTests
    {

        [Fact]
        public void it_should_do_simple_mask()
        {
            var name = "";

            var result = name.Mask("AA-A");

            Assert.Equal("__-_", result);
        }

        [Fact]
        public void it_should_do_simple_mask_two()
        {
            var name = "ab";

            var result = name.Mask("AA-A");

            Assert.Equal("ab-_", result);
        }

        [Fact]
        public void it_should_do_simple_mask_three()
        {
            var name = "abc";

            var result = name.Mask("##-A");

            Assert.Equal("__-c", result);
        }

        [Fact]
        public void it_should_do_simple_mask_of_numbers()
        {
            var name = "3146064496";

            var result = name.Mask("(###) ###-####");

            Assert.Equal("(314) 606-4496", result);
        }

        [Fact]
        public void it_should_do_simple_mask_of_numbers_and_letters()
        {
            var name = "314606449b";

            var result = name.Mask("(**A) ##*-A###");

            Assert.Equal("(31_) 606-_49_", result);
        }
    }
}
