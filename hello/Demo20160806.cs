using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo20160806
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] a = { 10, 21, 13, 34, 45, 16, 56, 67, 78, 93 };
            Console.WriteLine(a);
            Console.WriteLine("\r\n =======================\r\n");
            Console.WriteLine("1、定义Int数组长度是{0}",a.Length);
            Console.WriteLine("\r\n =======================\r\n");
            for (int i = 0; i < a.Length; i++)
            {
                Console.WriteLine("2、输出数组中的元素{0}",a[i].ToString());
            }
            Console.WriteLine("\r\n =======================\r\n");
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] % 2 > 0)
                {
                    Console.WriteLine("3、输出数组中的偶数元素{0}", a[i].ToString());
                }
            }
            Console.WriteLine("\r\n =======================\r\n");
            int temp = 0;
            for (int i = 0; i < a.Length; i++) 
            {
                for (int j = i + 1; j < a.Length; j++)
                {
                    if (a[i] > a[j])
                    {
                        temp = a[i];
                        a[i] = a[j];
                        a[j] = temp;
                   
                    }
                }
               
            }
            
            var str = "4、升序排列 : ";
            for (int i = 0; i < a.Length; i++)
            {
                str += a[i]+ ",";
            }
            Console.WriteLine(str);
            Console.WriteLine("\r\n =======================\r\n");
            
            List<int> list = a.ToList();
            list.RemoveAt(3);
            int[] newa = list.ToArray();
            Console.WriteLine("5、删除下标为3的元素",newa);
            Console.WriteLine("\r\n =======================\r\n");
            
            Console.ReadKey();



        }
    }
}
