using System;
using System.Collections;
using System.Threading;

namespace Annotation
{
    public sealed class AnnotationProperty
    {
        internal class DefaultPropertyOwner
        {

        }


        private static readonly Hashtable _allRegistedProperies = new Hashtable();

        public string PropertyName { get; }
        public Type PropertyType { get; }
        public Type OwnerType { get; }
        public AnnotationPropertyMetadata Metadata { get; }
        internal PropertyKey PropertyKey { get; }

        private AnnotationProperty(
            string propertyName,
            Type propertyType,
            Type ownerType,
            AnnotationPropertyMetadata metadata)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
            OwnerType = ownerType ?? throw new ArgumentNullException(nameof(ownerType));
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));

            this.PropertyKey = new PropertyKey(
                this.PropertyName,
                this.OwnerType);
        }


        public static AnnotationProperty Register(string propname, Type proptype, Type ownertype)
        {
            return RegisterCore(propname, proptype, ownertype, null);
        }

        public static AnnotationProperty Register(string propname, Type proptype, Type ownertype, AnnotationPropertyMetadata metadata)
        {
            return RegisterCore(propname, proptype, ownertype, metadata);
        }

        public static AnnotationProperty RegisterCore(string propname, Type proptype, Type ownertype, AnnotationPropertyMetadata metadata)
        {
            var key = new PropertyKey(propname, ownertype);

            if (_allRegistedProperies.Contains(key))
            {
                throw new Exception("已注册");
            }

            if (metadata == null)
            {
                metadata = new AnnotationPropertyMetadata(
                            null,
                            AnnotationProperty.DefaultValueFactory,
                            ValueScope.Global);
            }

            AnnotationProperty property = new AnnotationProperty(propname, proptype, ownertype, metadata);

            _allRegistedProperies[key] = property;

            return property;
        }

        private static object DefaultValueFactory(AnnotationProperty property, object sourceojb)
        {
            return null;
        }



        internal static AnnotationProperty GetProperty(PropertyKey propkey)
        {
            throw new NotImplementedException();
        }

        public void SetValue(object obj, object objval)
        {
            AnnotationValueManager.SetValue(this, obj, objval, false);
        }

        public void SetValue(object obj, object objval, bool autodispose)
        {
            AnnotationValueManager.SetValue(this, obj, objval, autodispose);
        }

        public object GetValue(object obj)
        {
            return AnnotationValueManager.GetValue(this, obj);
        }
    }
}
