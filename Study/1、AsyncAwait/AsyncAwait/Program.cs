using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncAwait
{
    /// <summary>
    /// Async/Await
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("欢迎来到.net高级班vip课程，今晚学习Async/Await");
                AwaitAsyncShow.TestShow();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}
