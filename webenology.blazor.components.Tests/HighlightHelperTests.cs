using System;

using webenology.blazor.components.Helpers;

using Xunit;

namespace webenology.blazor.components.Tests
{
    public class HighlightHelperTests
    {
        [Fact]
        public void it_should_highlight_1()
        {
            var item = "My Item";

            var results = item.Highlight("te");

            Assert.Equal("My I<mark>te</mark>m", results);
        }


        [Fact]
        public void it_should_highlight_2()
        {
            var item = "My Item";

            var results = item.Highlight("tem");

            Assert.Equal("My I<mark>tem</mark>", results);
        }

        [Fact]
        public void it_should_highlight_3()
        {
            var item = "My Item";

            var results = item.Highlight("my em");

            Assert.Equal("<mark>My</mark> It<mark>em</mark>", results);
        }

        [Fact]
        public void it_should_highlight_4()
        {
            var item = "Hello Hello Hello";

            var results = item.Highlight("he lo");

            Assert.Equal("<mark>He</mark>l<mark>lo</mark> <mark>He</mark>l<mark>lo</mark> <mark>He</mark>l<mark>lo</mark>", results);
        }

        [Fact]
        public void it_should_highlight_5()
        {
            var item = "1-Db Goddess and Thyro-Drive";

            var results = item.Highlight("thyro godd ");

            Assert.Equal("1-Db <mark>Godd</mark>ess and <mark>Thyro</mark>-Drive", results);
        }

        [Fact]
        public void it_should_highlight_6()
        {
            var item = "99.7 Shaylee";

            var results = item.Highlight("ee ylee");

            Assert.Equal("99.7 Sha<mark>ylee</mark>", results);
        }

        [Fact]
        public void it_should_highlight_7()
        {
            var item = "99.7 Shaylee";

            var results = item.Highlight("ee yl");

            Assert.Equal("99.7 Sha<mark>ylee</mark>", results);
        }

        [Fact]
        public void it_should_highlight_8()
        {
            var item = "99.7 Shaylee";

            var results = item.Highlight("sh ay le");

            Assert.Equal("99.7 <mark>Shayle</mark>e", results);
        }

        [Fact]
        public void it_should_highlight_9()
        {
            var item = "99.7 Shaylee";

            var results = item.Highlight("ay sh ay le");

            Assert.Equal("99.7 <mark>Shayle</mark>e", results);
        }

        [Fact]
        public void it_should_highlight_10()
        {
            var item = "99.7 Shaylee";

            var results = item.Highlight("laa");

            Assert.Equal("99.7 Shay<mark>lee</mark>", results);
        }

        [Fact]
        public void it_should_highlight_11()
        {
            var item = "this is a very very long post and hopefully I can get some highlighting on this";

            var results = item.Highlight("this very pod and hope can ge high on t");

            Assert.Equal("<mark>this</mark> is a <mark>very</mark> <mark>very</mark> l<mark>on</mark>g pos<mark>t</mark> <mark>and</mark> <mark>hope</mark>fully I <mark>can</mark> <mark>get</mark> some <mark>high</mark>ligh<mark>t</mark>ing <mark>on</mark> <mark>this</mark>", results);
        }

        [Fact]
        public void it_should_highlight_12()
        {
            var item = "something";

            var results = item.Highlight("godd");

            Assert.Equal("something", results);
        }

        [Fact]
        public void it_should_not_fail_on_null_search_term()
        {
            var item = "99.7 Shaylee";

            var results = item.Highlight(null);

            Assert.Equal("99.7 Shaylee", results);
        }

        [Fact]
        public void it_should_not_fail_on_empty_search_term()
        {
            var item = "99.7 Shaylee";

            var results = item.Highlight("");

            Assert.Equal("99.7 Shaylee", results);
        }

        [Fact]
        public void it_should_not_fail_on_empty_item_and_null_search()
        {
            var item = "";

            var results = item.Highlight(null);

            Assert.Equal("", results);
        }

        [Fact]
        public void it_should_not_fail_on_empty_item_and_empty_search()
        {
            var item = "";

            var results = item.Highlight("");

            Assert.Equal("", results);
        }


      


    }
}
