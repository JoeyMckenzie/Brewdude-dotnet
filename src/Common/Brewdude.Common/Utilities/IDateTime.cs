using System;

namespace Brewdude.Common
{
    public interface IDateTime
    {
        DateTime Now { get; }
        int CurrentYear { get; }
        int CurrentMonth { get; }
        int CurrentDay { get; }
    }
}