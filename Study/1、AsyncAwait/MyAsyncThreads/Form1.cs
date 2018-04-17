using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyAsyncThreads
{
    /// <summary>
    /// 1 作业讲解，事件回顾
    /// 2 进程-线程-多线程，同步和异步
    /// 3 异步使用和回调
    /// 4 异步参数
    /// 5 异步等待
    /// 6 异步返回值
    /// 
    /// 1 多线程的特点：不卡主线程、速度快、无序性
    /// 2 thread：线程等待，回调，前台线程/后台线程， 
    /// 3 threadpool：线程池使用，设置线程池，ManualResetEvent
    /// 4 Task初步接触
    /// 
    /// 1 task：waitall  waitany  continueWhenAny  continueWhenAll  
    /// 2 并行运算Parallel
    /// 3 异常处理、线程取消、多线程的临时变量和lock
    /// 4 作业部署
    /// </summary>
    public partial class frmAsyncThreads : Form
    {
        public frmAsyncThreads()
        {
            Console.WriteLine("欢迎来到.net高级班vip课程，今天是Eleven老师带来的异步多线程内容");
            InitializeComponent();
        }

        #region btnSync_Click
        /// <summary>
        /// 同步方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSync_Click(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            //Thread
            Console.WriteLine($"****************btnSync_Click Start {Thread.CurrentThread.ManagedThreadId} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}***************");

            for (int i = 0; i < 5; i++)
            {
                string name = string.Format($"btnSync_Click_{i}");
                this.DoSomethingLong(name);
            }
            watch.Stop();
            Console.WriteLine($"****************btnSync_Click   End {Thread.CurrentThread.ManagedThreadId} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}  {watch.ElapsedMilliseconds}***************");
        }
        #endregion

        #region btnAsync_Click
        private delegate void DoLongHandler(string name);

        /// <summary>
        /// 异步方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAsync_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"****************btnAsync_Click Start {Thread.CurrentThread.ManagedThreadId}***************");
            {
                Action<string> act = this.DoSomethingLong;
                //act += this.DoSomethingLong;//多播委托
                //act.Invoke("btnAsync_Click");
                //act("btnAsync_Click");
                //() => { }

                IAsyncResult iAsyncResult = null;
                AsyncCallback callback = t =>
                 {
                     Console.WriteLine(t);
                     Console.WriteLine($"string.ReferenceEquals(t, iAsyncResult)={string.ReferenceEquals(t, iAsyncResult)}");
                     Console.WriteLine($"This is Callback {Thread.CurrentThread.ManagedThreadId}");
                 };
                //callback.Invoke()

                iAsyncResult = act.BeginInvoke("btnAsync_Click_async", callback, "wing");//委托的异步调用
                //callback.Invoke(iAsyncResult);

                Console.WriteLine("DoSomething Else/.....");
                Console.WriteLine("DoSomething Else/.....");
                Console.WriteLine("DoSomething Else/.....");
                Console.WriteLine("DoSomething Else/.....");

                //while (!iAsyncResult.IsCompleted)//边等待边操作  提示用户
                //{
                //    Thread.Sleep(100);//有误差，最多100ms
                //    Console.WriteLine("*******等待等待*********");
                //}

                //iAsyncResult.AsyncWaitHandle.WaitOne();//等到异步完成 没有损耗
                //iAsyncResult.AsyncWaitHandle.WaitOne(-1);//等到异步完成 没有损耗

                //iAsyncResult.AsyncWaitHandle.WaitOne(2000);//最多只等2000ms，否则不等了


                act.EndInvoke(iAsyncResult);
            }

            {
                Func<string, int> func = t =>
                   {
                       Thread.Sleep(2000);
                       return DateTime.Now.Year;
                   };

                IAsyncResult iAsyncResult = func.BeginInvoke("消逝青春", t =>
                {
                    int iResult = func.EndInvoke(t);

                }, null);

                int iResultOut = func.EndInvoke(iAsyncResult);
            }

            Console.WriteLine("异步执行完成了才去做的事儿");
            Console.WriteLine($"****************btnAsync_Click   End {Thread.CurrentThread.ManagedThreadId}***************");
        }
        #endregion

        #region btnAsyncThreads_Click
        /// <summary>
        /// 异步多线程三大特点:
        /// 1 同步卡界面，UI线程被占用；异步多线程不卡界面，UI线程空闲，计算任务交给子线程
        /// 2 同步方法慢，因为只有一个线程干活；异步多线程方法快，因为多个线程并发计算,
        ///   这里也会消耗更多的资源，不是线程的线性关系，不是线程越多越好(1资源有限 2线程调度耗资源 3不稳定)
        /// 3 异步多线程是无序的，不可预测的：启动顺序不确定、消耗时间不确定、结束顺序不确定
        ///   我们不要试图控制执行的顺序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAsyncThreads_Click(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Console.WriteLine($"****************btnAsyncThreads_Click Start {Thread.CurrentThread.ManagedThreadId} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}***************");

            Action<String> act = this.DoSomethingLong;
            for (int i = 0; i < 5; i++)
            {
                string name = string.Format($"btnAsyncThreads_Click_{i}");
                act.BeginInvoke(name, null, null);
            }

            watch.Stop();
            Console.WriteLine($"****************btnAsyncThreads_Click   End {Thread.CurrentThread.ManagedThreadId} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}  {watch.ElapsedMilliseconds}***************");

        }
        #endregion

        #region btnThread_Click
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnThread_Click(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            //Thread
            Console.WriteLine($"****************btnThread_Click Start {Thread.CurrentThread.ManagedThreadId} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}***************");

            Func<int> oldFunc = () =>
              {
                  Console.WriteLine("oldFunc Sleep Start");
                  Thread.Sleep(10000);
                  Console.WriteLine("oldFunc Sleep End。。");
                  return DateTime.Now.Millisecond;
              };

            Func<int> newFunc = this.ThreadWithReturn<int>(oldFunc);//begin invoke

            Console.WriteLine("Else1............................");
            Thread.Sleep(1000);
            Console.WriteLine("Else2............................");
            Thread.Sleep(1000);
            Console.WriteLine("Else3............................");

            Console.WriteLine(newFunc.Invoke());//等待 endivoke

            //int iResult = this.ThreadWithReturn<int>(oldFunc)();


            //List<Thread> threadList = new List<Thread>();

            //for (int i = 0; i < 5; i++)
            //{
            //    string name = string.Format($"btnSync_Click_{i}");
            //    ThreadStart start = () => this.DoSomethingLong(name);
            //    Thread thread = new Thread(start);//默认是前台线程,启动后计算完才能退出
            //    thread.IsBackground = true;//设置成后台线程，会立即退出
            //    thread.Start();
            //    threadList.Add(thread);
            //    //别用
            //    //thread.Suspend();//暂停
            //    //thread.Resume();//恢复
            //    //thread.Abort();//销毁线程
            //    //停止线程  靠的不是外部力量，而是线程自身，外部修改信号量，线程检测信号量
            //    //thread.Join();//线程等待
            //}

            //while (threadList.Count(t => t.ThreadState != System.Threading.ThreadState.Stopped) > 0)
            //{
            //    Thread.Sleep(500);
            //    Console.WriteLine("waingting....");
            //}
            ////foreach (var thread in threadList)
            ////{
            ////    thread.Join();//表示把thread线程任务join到当前线程，也就是当前线程等着thread任务完成
            ////}
            watch.Stop();//能正确统计全部耗时
            Console.WriteLine($"****************btnThread_Click   End {Thread.CurrentThread.ManagedThreadId} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}  {watch.ElapsedMilliseconds}***************");
        }
        //thread 没有回调
        /// <summary>
        /// 回调封装
        /// </summary>
        /// <param name="start"></param>
        /// <param name="callback">回调</param>
        private void ThreadWithCallback(ThreadStart start, Action callback)
        {
            ThreadStart newStart = () =>
            {
                start.Invoke();
                callback.Invoke();
            };
            Thread thread = new Thread(newStart);
            thread.Start();
        }
        //thread 没有返回值
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        private Func<T> ThreadWithReturn<T>(Func<T> func)
        {
            T t = default(T);
            ThreadStart newStart = () =>
            {
                t = func.Invoke();

            };
            Thread thread = new Thread(newStart);
            thread.Start();

            return new Func<T>(() =>
           {
               thread.Join();
               return t;
           });
        }
        #endregion

        #region btnThreadPool_Click
        /// <summary>
        /// 2.0 线程池  享元模式   单例模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnThreadPool_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"****************btnThreadPool_Click Start {Thread.CurrentThread.ManagedThreadId}***************");
            {
                ManualResetEvent mre = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(t =>
                {
                    this.DoSomethingLong("btnThreadPool_Click");
                    mre.Set();
                });
                mre.WaitOne();
            }

            #region ManualResetEvent
            {
                ManualResetEvent mre = new ManualResetEvent(false);//false 关闭
                new Action(() =>
                {
                    Thread.Sleep(5000);
                    Console.WriteLine("委托的异步调用");
                    mre.Set();//打开
                }).BeginInvoke(null, null);

                mre.WaitOne();
                Console.WriteLine("12345");
                mre.Reset();//关闭
                new Action(() =>
                {
                    Thread.Sleep(5000);
                    Console.WriteLine("委托的异步调用2");
                    mre.Set();//打开
                }).BeginInvoke(null, null);
                mre.WaitOne();
                Console.WriteLine("23456");
            }
            #endregion

            #region PoolSet
            ////ThreadPool.SetMaxThreads(8, 8);//最小也是核数
            ////ThreadPool.SetMinThreads(8, 8);
            //int workerThreads = 0;
            //int ioThreads = 0;
            //ThreadPool.GetMaxThreads(out workerThreads, out ioThreads);
            //Console.WriteLine(String.Format("Max worker threads: {0};    Max I/O threads: {1}", workerThreads, ioThreads));

            //ThreadPool.GetMinThreads(out workerThreads, out ioThreads);
            //Console.WriteLine(String.Format("Min worker threads: {0};    Min I/O threads: {1}", workerThreads, ioThreads));

            //ThreadPool.GetAvailableThreads(out workerThreads, out ioThreads);
            //Console.WriteLine(String.Format("Available worker threads: {0};    Available I/O threads: {1}", workerThreads, ioThreads));
            #endregion
            Console.WriteLine($"****************btnThreadPool_Click   End {Thread.CurrentThread.ManagedThreadId}***************");

        }
        #endregion

        #region btnTask_Click
        /// <summary>
        /// TASK：基于线程池线程
        /// waitall  waitany  continueWhenAny  continueWhenAll  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTask_Click(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Console.WriteLine($"****************btnTask_Click Start {Thread.CurrentThread.ManagedThreadId} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}***************");

            List<Task> taskList = new List<Task>();

            for (int i = 0; i < 5; i++)
            {
                string name = string.Format($"btnTask_Click_{i}");
                Task task = Task.Factory.StartNew(() => this.DoSomethingLong(name));
                //Task taskContinue = task.ContinueWith(t =>
                // {
                //     Console.WriteLine(t.IsCompleted);
                //     Console.WriteLine($"ContinueWhenAny {Thread.CurrentThread.ManagedThreadId}");
                // }).ContinueWith(t =>
                // {
                //     Console.WriteLine(t.IsCompleted);
                //     Console.WriteLine($"ContinueWhenAny {Thread.CurrentThread.ManagedThreadId}");
                // });

                taskList.Add(task);
            }

            taskList.Add(Task.Factory.ContinueWhenAny(taskList.ToArray(), t =>
                {
                    Console.WriteLine(t.IsCompleted);
                    Console.WriteLine($"ContinueWhenAny {Thread.CurrentThread.ManagedThreadId}");
                }));

            taskList.Add(Task.Factory.ContinueWhenAll(taskList.ToArray(), tList =>
            {
                Console.WriteLine(tList[0].IsCompleted);
                Console.WriteLine($"ContinueWhenAll {Thread.CurrentThread.ManagedThreadId}");
            }));
            //回调形式的，全部任务完成后执行的后续动作

            Console.WriteLine("before WaitAny");
            Task.WaitAny(taskList.ToArray());//当前线程等待某个任务的完成  主线程
            Console.WriteLine("after WaitAny");


            Console.WriteLine("before WaitAll");
            Task.WaitAll(taskList.ToArray());//当前线程等待全部任务的完成  主线程
            Console.WriteLine("after WaitAll");

            watch.Stop();
            Console.WriteLine($"****************btnTask_Click   End {Thread.CurrentThread.ManagedThreadId} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}  {watch.ElapsedMilliseconds}***************");
        }
        #endregion

        #region btnParallel_Click
        /// <summary>
        /// 并行编程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnParallel_Click(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Console.WriteLine($"****************btnParallel_Click Start {Thread.CurrentThread.ManagedThreadId} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}***************");

            //Parallel.Invoke(() =>//==Task+waitall
            //this.DoSomethingLong("btnParallel_Click_0")
            // , () => this.DoSomethingLong("btnParallel_Click_1")
            // , () => this.DoSomethingLong("btnParallel_Click_2")
            // , () => this.DoSomethingLong("btnParallel_Click_3")
            // , () => this.DoSomethingLong("btnParallel_Click_4")
            // , () => this.DoSomethingLong("btnParallel_Click_5"));

            //Parallel.For(0, 5, t => this.DoSomethingLong($"btnParallel_Click_{t}"));

            //Parallel.ForEach(new int[] { 1, 2, 3, 4, 5, 6 }, t => this.DoSomethingLong($"btnParallel_Click_{t}"));

            //new Action(() =>
            //{
            //ParallelOptions option = new ParallelOptions()
            //{
            //    MaxDegreeOfParallelism = 3//最大并发数
            //};
            //Parallel.ForEach(new int[] { 1, 2, 3, 4, 5, 6 }, option, t =>
            //{
            //    this.DoSomethingLong($"btnParallel_Click_{t}");
            //});
            //}).BeginInvoke(null, null);

            ParallelOptions option = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 3//最大并发数
            };
            Parallel.ForEach(new int[] { 1, 2, 3, 4, 5, 6 }, option, (t, state) =>
            {
                this.DoSomethingLong($"btnParallel_Click_{t}");
                //state.Break();//这一次结束
                //return;

                state.Stop();//整个Parallel结束
                return;
                //不能共存
            });


            watch.Stop();
            Console.WriteLine($"****************btnParallel_Click   End {Thread.CurrentThread.ManagedThreadId} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}  {watch.ElapsedMilliseconds}***************");

        }

        #endregion

        #region btnThreadCore_Click
        private static object btnThreadCore_Click_Lock = new object();
        private int TotalCount = 0;//
        private List<int> IntList = new List<int>();

        /// <summary>
        /// 线程安全的集合
        /// </summary>
        //System.Collections.Concurrent.ConcurrentDictionary

        /// <summary>
        /// 异常处理、线程取消、多线程的临时变量和线程安全lock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnThreadCore_Click(object sender, EventArgs e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Console.WriteLine();
            Console.WriteLine($"***********************btnThreadCore_Click Start 主线程id {Thread.CurrentThread.ManagedThreadId}**********************************");
            try
            {
                TaskFactory taskFactory = new TaskFactory();
                List<Task> taskList = new List<Task>();
                #region 异常处理
                ////在线程Action加上try catch，日志记录，不抛异常
                //for (int i = 0; i < 20; i++)
                //{
                //    string name = string.Format($"btnThreadCore_Click_{i}");
                //    Action<object> act = t =>
                //    {
                //        try
                //        {
                //            Thread.Sleep(2000);
                //            if (t.ToString().Equals("btnThreadCore_Click_11"))
                //            {
                //                throw new Exception(string.Format($"{t} 执行失败"));
                //            }
                //            if (t.ToString().Equals("btnThreadCore_Click_12"))
                //            {
                //                throw new Exception(string.Format($"{t} 执行失败"));
                //            }
                //            Console.WriteLine("{0} 执行成功", t);
                //        }
                //        catch (Exception ex)
                //        {
                //            Console.WriteLine(ex.Message);
                //        }
                //    };
                //    taskList.Add(taskFactory.StartNew(act, name));
                //}
                ////异常被吞掉了，加上waitall才能抓取到异常
                //Task.WaitAll(taskList.ToArray());
                #endregion

                #region 线程取消
                ////线程间都是通过共有变量：都能访问局部变量/全局变量/数据库的一个值/硬盘文件
                ////线程不能被外部停止，只能自身停止自身；或者在任务启动前停止，会抛出异常的
                //CancellationTokenSource cts = new CancellationTokenSource();
                //for (int i = 0; i < 40; i++)
                //{
                //    string name = string.Format("btnThreadCore_Click{0}", i);
                //    Action<object> act = t =>
                //    {
                //        try
                //        {
                //            //if (cts.IsCancellationRequested)
                //            //{
                //            //    Console.WriteLine("{0} 取消一个任务的执行", t);
                //            //}
                //            Thread.Sleep(2000);
                //            if (t.ToString().Equals("btnThreadCore_Click11"))
                //            {
                //                throw new Exception(string.Format("{0} 执行失败", t));
                //            }
                //            if (t.ToString().Equals("btnThreadCore_Click12"))
                //            {
                //                throw new Exception(string.Format("{0} 执行失败", t));
                //            }
                //            if (cts.IsCancellationRequested)//检查信号量
                //            {
                //                Console.WriteLine("{0} 放弃执行", t);
                //            }
                //            else
                //            {
                //                Console.WriteLine("{0} 执行成功", t);
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            cts.Cancel();//表示修改了信号量  让大家取消执行
                //            Console.WriteLine(ex.Message);
                //        }
                //    };
                //    taskList.Add(taskFactory.StartNew(act, name, cts.Token));//没有启动的任务  在Cancel后放弃启动
                //}
                //Task.WaitAll(taskList.ToArray());
                #endregion

                #region 多线程临时变量
                //for (int i = 0; i < 5; i++)
                //{
                //    int k = i;
                //    new Action(() =>
                //    {
                //        Thread.Sleep(100);
                //        //Console.WriteLine(i);
                //        Console.WriteLine(k);
                //    }).BeginInvoke(null, null);
                //}
                #endregion

                #region 线程安全 lock
                //共有变量：都能访问局部变量/全局变量/数据库的一个值/硬盘文件
                for (int i = 0; i < 10000; i++)
                {
                    int newI = i;
                    taskList.Add(taskFactory.StartNew(() =>
                    {
                        lock (btnThreadCore_Click_Lock)//lock后的方法块，任意时刻只有一个线程可以进入
                        {//这里就是单线程
                            this.TotalCount += 1;
                            IntList.Add(newI);
                        }
                    }));
                }
                Task.WaitAll(taskList.ToArray());

                Console.WriteLine(this.TotalCount);
                Console.WriteLine(IntList.Count());
                #endregion
            }
            catch (AggregateException aex)
            {
                foreach (var item in aex.InnerExceptions)
                {
                    Console.WriteLine(item.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            watch.Stop();
            Console.WriteLine("**********************btnThreadCore_Click   End 主线程id {0} {1}************************************", Thread.CurrentThread.ManagedThreadId, watch.ElapsedMilliseconds);
            Console.WriteLine();
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 一个比较耗时耗资源的私有方法
        /// </summary>
        /// <param name="name"></param>
        private void DoSomethingLong(string name)
        {
            Console.WriteLine($"****************DoSomethingLong Start {name} {Thread.CurrentThread.ManagedThreadId} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}***************");
            long lResult = 0;
            for (int i = 0; i < 2000000000; i++)
            {
                lResult += i;
            }
            //Thread.Sleep(2000);

            Console.WriteLine($"****************DoSomethingLong   End  {name} {Thread.CurrentThread.ManagedThreadId} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}***************");
        }
        #endregion
    }
}
