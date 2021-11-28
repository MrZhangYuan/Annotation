using System;
using System.Collections.Generic;

namespace Annotation
{
    sealed class FrugalValueArray
    {
        internal struct Entry
        {
            public int Metadata;
            public AnnotationValue Value;

            public Entry(int metadata, AnnotationValue value)
            {
                Metadata = metadata;
                Value = value;
            }
        }

        private Entry[] _entries;
        private int _count;

        public FrugalValueArray(int capacity)
        {
            this._entries = new Entry[1];
            this._count = 0;
        }

        private void Increase(int incrlen)
        {
            Entry[] newList = new Entry[this._entries.Length + incrlen];
            this._entries.CopyTo(newList, 0);
            this._entries = newList;
        }

        public AnnotationValue Find(object sourceobj)
        {
            for (int i = 0; i < this._count; i++)
            {
                if (_entries[i].Value.CheckEquals(sourceobj))
                {
                    return _entries[i].Value;
                }
            }

            return null;
        }

        public void Add(AnnotationValue newvalue)
        {
            if (this._entries.Length == this._count)
            {
                this.Increase(1);
            }
            this._entries[this._count++] = new Entry(0, newvalue);
        }
    }

    class AnnotationValue
    {
        public struct AnnotationValueEntry
        {
            public PropertyKey Key;
            public object Value;
            public bool IsAutoDispose;

            public AnnotationValueEntry(
                PropertyKey key,
                object value,
                bool isautodispose)
            {
                Key = key;
                Value = value;
                IsAutoDispose = isautodispose;
            }
        }

        [Flags]
        enum SourceFlags : ushort
        {
            String,
            Reference,
            Value
        }

        [Flags]
        enum EntryFlags : ushort
        {
            AutoDisposable
        }

        private WeakReference _sourceReference = null;

        public object Source
        {
            get
            {
                if (this._sourceReference != null
                    && this._sourceReference.IsAlive)
                {
                    return this._sourceReference.Target;
                }

                return null;
            }
        }

        public List<AnnotationValueEntry> Entries { get; }


        private AnnotationValue(object obj)
        {
            this._sourceReference = new WeakReference(obj);
            this.Entries = new List<AnnotationValueEntry>(4);
        }

        public void AddOrUpdate(PropertyKey key, object objval)
        {
            //TODO 更新值时，若旧值为IDispose类型，如何选择释放时机
            //1：不参与管理该类型，调用者自己管理
            //2：Set提供重载，新增 对象不可达时 自动释放 IDispose 类型值
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (this.Entries[i].Key == key)
                {
                    this.Entries[i] = new AnnotationValueEntry(key, objval, true);
                    return;
                }
            }

            this.Entries.Add(new AnnotationValueEntry(key, objval, true));
        }

        public object Get(PropertyKey key)
        {
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (this.Entries[i].Key == key)
                {
                    return this.Entries[i].Value;
                }
            }
            return null;
        }

        public object ClearValue(PropertyKey key)
        {
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (this.Entries[i].Key == key)
                {
                    object value = this.Entries[i].Value;

                    this.Entries.RemoveAt(i);

                    return value;
                }
            }
            return null;
        }

        public bool CheckEquals(object targetobj)
        {
            return object.ReferenceEquals(this.Source, targetobj);
        }

        public static AnnotationValue Create(object sourceobj)
        {
            return new AnnotationValue(sourceobj);
        }
    }



    /*
    internal class PropertyValue
    {
        private readonly List<AnnotationPropertyEntry> _entries = new List<AnnotationPropertyEntry>(4);

        public PropertyValue()
        {

        }

        public AnnotationPropertyEntry FindOrCreateEntry(object sourceobj)
        {
            AnnotationPropertyEntry entry = null;

            for (int i = 0; i < this._entries.Count; i++)
            {
                var current = this._entries[i];

                if (current.SourceObject != null
                    && current.SourceObject.IsAlive
                    && object.ReferenceEquals(sourceobj, current.SourceObject.Target))
                {
                    entry = current;
                    break;
                }
            }

            if (entry == null)
            {
                entry = new AnnotationPropertyEntry
                {
                    SourceObject = new WeakReference(sourceobj),
                    SourceHashCode = sourceobj.GetHashCode()
                };
                this._entries.Add(entry);
            }

            return entry;
        }


        public AnnotationPropertyEntry FindEntry(object sourceobj)
        {
            AnnotationPropertyEntry entry = null;

            for (int i = 0; i < this._entries.Count; i++)
            {
                var current = this._entries[i];

                if (current.SourceObject != null
                    && current.SourceObject.IsAlive
                    && object.ReferenceEquals(sourceobj, current.SourceObject.Target))
                {
                    entry = current;
                    break;
                }
            }

            return entry;
        }
    }
    */
}
