using System;
using Brewdude.Common;

namespace Brewdude.Application.Tests.Infrastructure
{
    public class DateTimeTest : IDateTime
    {
        public DateTime Now => DateTime.Now;
        public int CurrentYear => DateTime.Now.Year;
        public int CurrentMonth => DateTime.Now.Month;
        public int CurrentDay => DateTime.Now.Day;
    }
}