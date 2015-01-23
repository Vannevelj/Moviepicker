using System;

namespace MoviePickerApi.ApiModels.Database
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