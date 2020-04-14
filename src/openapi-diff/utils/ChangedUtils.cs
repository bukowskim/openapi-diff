﻿using openapi_diff.BusinessObjects;

namespace openapi_diff.utils
{
    public static class ChangedUtils
    {
        public static bool IsUnchanged(ChangedBO changed)
        {
            return changed == null || changed.IsUnchanged();
        }

        public static bool IsCompatible(ChangedBO changed)
        {
            return changed == null || changed.IsCompatible();
        }

        public static T IsChanged<T>(T changed)
        where T : ChangedBO
        {
            if (IsUnchanged(changed))
            {
                return null;
            }
            return changed;
        }
    }
}
