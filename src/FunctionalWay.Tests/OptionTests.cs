using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalWay.Tests
{
    public class OptionTests
    {
        private string Greet(Option<string> name) 
            => name.Match(
                Some: n => $"hello, {n}",
                None: () => "sorry, who?");

        private Option<string> GetName(bool real)
            => real
                ? F.Some("Name")
                : F.None;

        private async Task<string> GetNameAsync() 
            => "My name is Trinity";

        [Fact]
        public void Match_should_call_appropriate_func()
        {
            Assert.Equal("hello, John", Greet(F.Some("John")));
            Assert.Equal("sorry, who?", Greet(F.None));
        }

        [Fact]
        public void Match_should_handle_action()
        {
            var result = GetName(false);
            result.Match(
                None: () => Console.Write("Nothing"),
                Some: Console.Write
            );
            
            Assert.Equal(F.None, GetName(false));
        }

//        [Fact]
//        public async Task  Match_should_be_able_to_call_async_func()
//        {
//            var result = GetName(false);
//            var name = await result.Match(
//                None: async () => await GetNameAsync(),
//                Some: (n) => n
//            );
//            
//            Assert.Equal("My name is Trinity", name);
//        }

        [Fact]
        public void Should_compare_cointained_value()
        {
            Assert.Equal(F.Some(20), F.Some(20));
        }


    }

    
}