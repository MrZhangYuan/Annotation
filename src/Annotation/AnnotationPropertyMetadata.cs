using System;

namespace Annotation
{
    public enum ValueScope
    {
        //ThreadLocal,
        //Type,
        Global = 0,
        Instance = 1
    }


    /// <summary>
    /// 再次设置或获取值时，源对象的比较方式
    /// 引用类型除String，默认Refrences，String使用 == （Equals）
    /// 
    /// </summary>
    //internal enum EqualType
    //{
    //    Default = 0,
    //    Refrences = 1,
    //    Equals = 2
    //}


    //public delegate void AnnotationPropertyChangedCallback(object obj, AnnotationPropertyChangedEventArgs e);
    //public class AnnotationPropertyChangedEventArgs : EventArgs
    //{

    //}

    public class AnnotationPropertyMetadata
    {
        //private static readonly Type[] _equesTypes = new Type[]
        //{
        //    typeof(byte),
        //    typeof(sbyte),
        //    typeof(char),
        //    typeof(short),
        //    typeof(ushort),
        //    typeof(int),
        //    typeof(uint),
        //    typeof(long),
        //    typeof(ulong),
        //    typeof(float),
        //    typeof(double),
        //    typeof(decimal)
        //};

        [Flags]
        internal enum MetadataFlags : uint
        {
            SealedID = 0x00000001,
            DefaultValueSeted = 0x00000002,
            ValueCreaterSeted = 0x00000004,

            // Unused                                    = 0x00000008,
            // Unused                                    = 0x00000010,
            // Unused                                    = 0x00000020, 
            // Unused                                    = 0x00000040,
            // Unused                                    = 0x00000080,

            ValueScope_Global = 0x00000100,
            ValueScope_Instance = 0x00000200,

            // Unused                                    = 0x00000400,
            // Unused                                    = 0x00000800,

            //EqualType_Default = 0x00001000,
            //EqualType_Equals = 0x00002000,
            //EqualType_Refrences = 0x00004000,

            // Unused                                    = 0x00008000,
            // Unused                                    = 0x00010000,
            // Unused                                    = 0x00020000,
            // Unused                                    = 0x00040000,
            // Unused                                    = 0x00080000,
            // Unused                                    = 0x00100000,
            // Unused                                    = 0x00200000,
            // Unused                                    = 0x00400000,
            // Unused                                    = 0x00800000,
            // Unused                                    = 0x01000000,
            // Unused                                    = 0x02000000,
            // Unused                                    = 0x04000000,
            // Unused                                    = 0x08000000,
            // Unused                                    = 0x10000000,
            // Unused                                    = 0x20000000,
            // Unused                                    = 0x40000000, 
            // Unused                                    = 0x80000000, 
        }

        internal MetadataFlags _flags;


        private object _defaultValue;

        public object DefaultValue
        {
            get { return _defaultValue; }
            //set { _defaultValue = value; }
        }


        private Func<AnnotationProperty, object, object> _valueCreater;

        public Func<AnnotationProperty, object, object> ValueCreater
        {
            get { return _valueCreater; }
            //set { _valueCreater = value; }
        }


        public AnnotationPropertyMetadata(object defaultValue)
            : this(defaultValue, ValueScope.Global)
        {

        }

        public AnnotationPropertyMetadata(object defaultValue, ValueScope valueScope)
            : this(defaultValue, null, valueScope)
        {
        }

        internal AnnotationPropertyMetadata(
            object defaultValue,
            Func<AnnotationProperty, object, object> valueCreater,
            ValueScope valueScope)
        {
            switch (valueScope)
            {
                case ValueScope.Global:
                    this.WriteFlag(MetadataFlags.ValueScope_Global,true);
                    break;

                case ValueScope.Instance:
                    this.WriteFlag(MetadataFlags.ValueScope_Instance, true);
                    break;

                default:
                    throw new NotSupportedException();
            }

            //switch (equalType)
            //{
            //    case EqualType.Default:
            //        this.WriteFlag(MetadataFlags.EqualType_Default, true);
            //        break;

            //    case EqualType.Refrences:
            //        this.WriteFlag(MetadataFlags.EqualType_Refrences, true);
            //        break;

            //    case EqualType.Equals:
            //        this.WriteFlag(MetadataFlags.EqualType_Equals, true);
            //        break;

            //    default:
            //        throw new NotSupportedException();
            //}


            if (Validator.NotNull(defaultValue))
            {
                this.WriteFlag(MetadataFlags.DefaultValueSeted, true);
            }

            if (Validator.NotNull(valueCreater))
            {
                this.WriteFlag(MetadataFlags.ValueCreaterSeted, true);
            }

            _defaultValue = defaultValue;
            _valueCreater = valueCreater;
        }


        internal void WriteFlag(MetadataFlags id, bool value)
        {
            if (value)
            {
                _flags |= id;
            }
            else
            {
                _flags &= (~id);
            }
        }

        internal bool ReadFlag(MetadataFlags id)
        {
            return (id & _flags) != 0;
        }

        internal object GetDefaultValue()
        {


            return DefaultValue;
        }

        //public bool CheckEquals(object source, object target)
        //{
        //    return object.ReferenceEquals(source, target);
        //}
    }
}
