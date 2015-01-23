using System;

namespace Database.DatabaseModels
{
    [Flags]
    public enum PreferenceTypes
    {
        Genre = 0,
        Year = 1,
        Language = 2,
        Actor = 4
    }
}