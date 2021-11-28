using System;

namespace Annotation
{
    internal class PropertyKey
    {
        public PropertyKey(string propname, Type ownertype)
        {
            PropertyName = propname;
            OwnerType = ownertype;

            this._hashCode = this.PropertyName.GetHashCode()
                ^ this.OwnerType.GetHashCode();
        }

        public string PropertyName { get; }
        public Type OwnerType { get; }

        private int _hashCode;

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public override bool Equals(object o)
        {
            if ((o != null) && (o is PropertyKey))
            {
                return Equals((PropertyKey)o);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(PropertyKey key)
        {
            return PropertyName.Equals(key.PropertyName) 
                && (OwnerType == key.OwnerType);
        }
    }
}
