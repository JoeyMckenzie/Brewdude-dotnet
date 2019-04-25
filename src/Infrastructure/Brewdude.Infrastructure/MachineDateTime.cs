namespace Brewdude.Infrastructure
{
    using System;
    using Brewdude.Common;
    using Brewdude.Common.Utilities;

    public class MachineDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;

        public int CurrentYear => DateTime.Now.Year;

        public int CurrentMonth => DateTime.Now.Month;

        public int CurrentDay => DateTime.Now.Day;
    }
}