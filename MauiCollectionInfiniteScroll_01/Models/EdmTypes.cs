using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models.EdmTypes;

public static class EdmTypes
{
    public enum EdmType
    {
        EdmString,
        EdmBinary,
        EdmBoolean,
        EdmDateTime,
        EdmDouble,
        EdmGuid,
        EdmInt32,
        EdmInt64,
        NotSupported,
        NotTested
    };
}
