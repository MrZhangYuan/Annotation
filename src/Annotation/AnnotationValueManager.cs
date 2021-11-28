using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Annotation
{
    internal static class AnnotationValueManager
    {
        private static readonly int _duTimeSeconds = 5 * 1000;
        private static readonly Timer _recycleTimer = new Timer(
                                                        AnnotationValueManager.RecycleValuesCallBack,
                                                        null,
                                                        _duTimeSeconds,
                                                        _duTimeSeconds);

        private static readonly ConcurrentDictionary<int, FrugalValueArray> _valueMap = new ConcurrentDictionary<int, FrugalValueArray>();

        static AnnotationValueManager()
        {

        }

        /// <summary>
        ///     定时清理已释放对象的属性值
        /// </summary>
        /// <param name="state"></param>
        private static void RecycleValuesCallBack(object state)
        {
            try
            {
                _recycleTimer.Change(Timeout.Infinite, Timeout.Infinite);


                //Console.WriteLine("清理开始：" + DateTime.Now);

                //Thread.Sleep(1000);

                ReleaseAutoDisposableObject();
                ReleaseRecycledObject();


                //Console.WriteLine("清理结束：" + DateTime.Now);
                //Console.WriteLine();
            }
            finally
            {
                _recycleTimer.Change(_duTimeSeconds, _duTimeSeconds);
            }
        }

        private static void ReleaseAutoDisposableObject()
        {

        }

        private static void ReleaseRecycledObject()
        {

        }



        internal static void SetValue(AnnotationProperty property, object obj, object objval, bool autodispose)
        {
            var values = _valueMap.GetOrAdd(
                        obj.GetHashCode(),
                        _p => new FrugalValueArray(1));

            var annvalue = values.Find(obj);
            if (annvalue == null)
            {
                annvalue = AnnotationValue.Create(obj);
                values.Add(annvalue);
            }
            annvalue.AddOrUpdate(property.PropertyKey, objval);
        }

        internal static object GetValue(AnnotationProperty property, object obj)
        {
            if (_valueMap.TryGetValue(obj.GetHashCode(), out FrugalValueArray values))
            {
                var annvalue = values.Find(obj);
                if (annvalue != null)
                {
                    return annvalue.Get(property.PropertyKey);
                }
            }

            return property.Metadata.GetDefaultValue();
        }


        //清空并返回当前值
        internal static object ClearValue(AnnotationProperty property, object obj)
        {
            if (_valueMap.TryGetValue(obj.GetHashCode(), out FrugalValueArray values))
            {
                var annvalue = values.Find(obj);
                if (annvalue != null)
                {
                    return annvalue.ClearValue(property.PropertyKey);
                }
            }

            return property.Metadata.GetDefaultValue();
        }





















        /*

        /// <summary>
        /// 注册的属性值数组，注册的属性始终存在
        /// </summary>
        private static PropertyValue[] _registedPropertyValues = new PropertyValue[256];


        /// <summary>
        /// 临时属性值字典
        /// 临时属性随着对象释放而删除
        /// </summary>
        private static readonly ConcurrentDictionary<PropertyKey, PropertyValue> _propertyValues = new ConcurrentDictionary<PropertyKey, PropertyValue>();

        private static void GrowUpIfNessery(int index)
        {
            if (index > _registedPropertyValues.Length)
            {
                int max = (int)(Math.Max(index, _registedPropertyValues.Length) * 1.3);
                _registedPropertyValues = new PropertyValue[max];
            }
        }

        public static PropertyValue GenerateValue(AnnotationProperty property)
        {
            GrowUpIfNessery(property.PropertyIndex);

            if (_registedPropertyValues[property.PropertyIndex] == null)
            {
                _registedPropertyValues[property.PropertyIndex] = new PropertyValue();
            }

            return _registedPropertyValues[property.PropertyIndex];
        }




        public static void SetValue(AnnotationProperty property, object sourceobj, object objvalue)
        {
            PropertyValue propval = GenerateValue(property);

            var entry = propval.FindOrCreateEntry(sourceobj);

            entry.Value = objvalue;
        }

        public static object GetValue(AnnotationProperty property, object sourceobj)
        {
            if (_registedPropertyValues.Length <= property.PropertyIndex
                || _registedPropertyValues[property.PropertyIndex] == null)
            {
                return null;
            }

            PropertyValue value = _registedPropertyValues[property.PropertyIndex];

            var entry = value.FindEntry(sourceobj);

            return entry?.Value;
        }


        */





    }
}
