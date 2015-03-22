using System;

namespace Models.Users.Authorization
{
    [Flags]
    public enum ApplicationType
    {
        Other = 0,
        JavaScript = 1,
        NativeConfidential = 2
    }
}