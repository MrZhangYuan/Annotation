using Annotation;
using System;
using System.Collections.Generic;

namespace System
{
    public static class ObjectAnnotationEx
    {
        //public static object Set<T>(this T obj, string propname, object value) where T : class
        //{
        //    AnnotationProperty ap = AnnotationProperty.GetProperty(
        //            new PropertyKey(
        //                propname,
        //                typeof(AnnotationProperty.DefaultPropertyOwner)
        //            )
        //        );

        //    return obj.Set(ap, value);
        //}

        //public static T Get<T>(this object obj, string propname, T defval = default(T))
        //{
        //    AnnotationProperty ap = AnnotationProperty.GetProperty(
        //            new PropertyKey(
        //                propname,
        //                typeof(AnnotationProperty.DefaultPropertyOwner)
        //            )
        //        );

        //    return obj.Get<T>(ap, defval);
        //}

        public static object Set<T>(this T obj, AnnotationProperty ap, object value) where T : class
        {
            ap.SetValue(obj, value);

            return obj;
        }

        public static object Set<T>(this T obj, AnnotationProperty ap, object value, bool autodispose) where T : class
        {
            ap.SetValue(obj, value);

            return obj;
        }

        public static T Get<T>(this object obj, AnnotationProperty ap, T defval = default(T))
        {
            if (ap.GetValue(obj) is T tval
                && tval != null)
            {
                return tval;
            }

            return defval;
        }


        public static IEnumerable<object> GetAll(this object obj)
        {

            return null;
        }
    }


}
