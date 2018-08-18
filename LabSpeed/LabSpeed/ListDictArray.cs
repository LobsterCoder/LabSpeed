using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabSpeed
{
    class ListDictArray
    {
        struct Point
        {
            public int x, y;
            public Point(int x, int y)
            {
                this.x = x; this.y = y;
            }
        }
        public static void Run()
        {
            ValueTypePerfTest();
            Console.ReadKey();
        }

        private static void ValueTypePerfTest()
        {
            const Int32 count = 1000000;

            using (new OperationTimer("List<Int32>"))
            {
                List<Int32> l = new List<Int32>();
                for (Int32 n = 0; n < count; n++)
                {
                    l.Add(n); // Без упаковки
                    Int32 x = l[n]; // Без распаковки
                }
                l = null; // Для удаления в процессе уборки мусора
            }

            using (new OperationTimer("List<Point>"))
            {
                List<Point> l = new List<Point>();
                for (Int32 n = 0; n < count; n++)
                {
                    l.Add(new Point(n,n));
                    Int32 x = l[n].x;
                }
                l = null; // Для удаления в процессе уборки мусора
            }

            using (new OperationTimer("Dictionnary<Int32, Int32>"))
            {
                Dictionary <Int32,Int32> a = new Dictionary <Int32,Int32>();
                for (Int32 n = 0; n < count; n++)
                {
                    a.Add(n, n); // Без упаковки
                    Int32 x = a[n]; // Без упаковки
                }
                a = null; // Для удаления в процессе уборки мусора
            }

            using (new OperationTimer("Dictionnary<Int32, Point>"))
            {
                Dictionary<Int32, Point> a = new Dictionary<Int32, Point>();
                for (Int32 n = 0; n < count; n++)
                {
                    a.Add(n, new Point(n,n));
                    Int32 x = a[n].x;
                }
                a = null; // Для удаления в процессе уборки мусора
            }

            using (new OperationTimer("Array[int32]"))
            {
                int[] l = new int[count];
                for (Int32 n = 0; n < count; n++)
                {
                    l[n] = n; 
                    Int32 x = l[n]; 
                }
                l = null; // Для удаления в процессе уборки мусора
            }

            using (new OperationTimer("Array[Point]"))
            {
                Point[] l = new Point[count];
                for (Int32 n = 0; n < count; n++)
                {
                    l[n] = new Point(n, n);
                    Int32 x = l[n].x;
                }
                l = null; // Для удаления в процессе уборки мусора
            }
        }
    }

    // Класс для оценки времени выполнения операций
    internal sealed class OperationTimer : IDisposable
    {
        private Stopwatch m_stopwatch;
        private String m_text;
        private Int32 m_collectionCount;
        public OperationTimer(String text)
        {
            PrepareForOperation();
            m_text = text;
            m_collectionCount = GC.CollectionCount(0);
            // Эта команда должна быть последней в этом методе
            // для максимально точной оценки быстродействия
            m_stopwatch = Stopwatch.StartNew();
        }
        public void Dispose()
        {
            Console.WriteLine("{0} (GCs={1,3}) {2}", (m_stopwatch.Elapsed), GC.CollectionCount(0) - m_collectionCount, m_text);
        }
        private static void PrepareForOperation()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
