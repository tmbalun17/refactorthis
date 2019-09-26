using System;
using System.Collections.Generic;
using System.Text;


namespace Ardalis.GuardClauses
{
    public static class GuidGuards
    {
        public static void NullGuid(this IGuardClause guardClause, Guid Id)
        {
            if (Id == Guid.Empty)
                throw new ArgumentException("Id cannot be Null");
        }
    }
}
