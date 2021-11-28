using System;

namespace Annotation
{
    internal static class Validator
    {
        public static void ThrowIfNull(object obj, string argname)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(argname);
            }
        }


        public static bool NotNull(object obj)
        {
            return obj != null;
        }


        public static bool NotAllNull(params object[] objs)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
