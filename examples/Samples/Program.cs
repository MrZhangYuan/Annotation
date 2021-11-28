using Annotation;
using System;
using System.Diagnostics;
using System.Threading;

namespace Samples
{
    class MyClass
    {
        public static AnnotationProperty HelloAnnotation = AnnotationProperty.Register(
            "Hello",
            typeof(string),
            typeof(MyClass),
            new AnnotationPropertyMetadata("Hello AnnotationProperty"));
    }

    class Program
    {
        static void Main(string[] args)
        {
            object[] obj = new object[10];

            for (int i = 0; i < obj.Length; i++)
            {
                obj[i] = new object();
            }



            while (true)
            {


                Stopwatch sw = Stopwatch.StartNew();

                for (int i = 0; i < obj.Length; i++)
                {
                    obj[i].Set(MyClass.HelloAnnotation, i + "");
                    i++;
                }


                //测试回收
                //obj[0] = null;
                //GC.Collect();
                //Thread.Sleep(100);

                //var dsds = typeof(MyClass);
                //var dddd = new MyClass().GetType();
                //var ddsss = object.ReferenceEquals(dsds, dddd);//true

                for (int i = 1; i < obj.Length; i++)
                {
                    var value = obj[i].Get<string>(MyClass.HelloAnnotation);
                    Console.WriteLine(value);
                }

                sw.Stop();
                Console.WriteLine(sw.Elapsed);

                Console.ReadKey();
            }

            Console.ReadKey();
        }
    }
}
