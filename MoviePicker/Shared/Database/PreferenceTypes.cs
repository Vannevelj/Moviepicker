using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Database
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
