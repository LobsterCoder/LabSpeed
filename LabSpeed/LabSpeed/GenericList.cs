using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabSpeed
{
    class GenericList
    {
        public static void Run()
        {
            ValueTypePerfTest();
            ReferenceTypePerfTest();
            Console.ReadKey();
        }

        private static void ValueTypePerfTest()
        {
            const Int32 count = 10000000;
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
            using (new OperationTimer("ArrayList of Int32"))
            {
                ArrayList a = new ArrayList();
                for (Int32 n = 0; n < count; n++)
                {
                    a.Add(n); // Упаковка
                    Int32 x = (Int32)a[n]; // Распаковка
                }
                a = null; // Для удаления в процессе уборки мусора
            }
        }

        private static void ReferenceTypePerfTest()
        {
            const Int32 count = 10000000;
            using (new OperationTimer("List<String>"))
            {
                List<String> l = new List<String>();
                for (Int32 n = 0; n < count; n++)
                {
                    l.Add("X"); // Копирование ссылки
                    String x = l[n]; // Копирование ссылки
                }
                l = null; // Для удаления в процессе уборки мусора
            }
            using (new OperationTimer("ArrayList of String"))
            {
                ArrayList a = new ArrayList();
                for (Int32 n = 0; n < count; n++)
                {
                    a.Add("X"); // Копирование ссылки
                    String x = (String)a[n]; // Проверка преобразования
                } // и копирование ссылки
                a = null; // Для удаления в процессе уборки мусора
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
                Console.WriteLine("{0} (GCs={1,3}) {2}", (m_stopwatch.Elapsed), GC.CollectionCount(0)- m_collectionCount,  m_text);
            }
            private static void PrepareForOperation()
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
    }
}

