using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;

namespace SystemProgramming1
{
    /// <summary>
    /// Interaction logic for MultiWindow.xaml
    /// </summary>
    public partial class MultiWindow : Window
    {
        private Random random = new();
        private CancellationTokenSource cts;
        public MultiWindow()
        {
            InitializeComponent();
        }
        #region 1
        private void ButtonStart1_Click(object sender, RoutedEventArgs e)
        {
            sum = 100;
            progressBar1.Value = 0;
            for(int i = 0;i < 12;i++)
            {
                new Thread(plusPercent).Start();
            }
        }

        private void ButtonStop1_Click(object sender, RoutedEventArgs e)
        {

        }
        private double sum;
        private void plusPercent()
        {
            double val = sum;
            Thread.Sleep(random.Next(250, 300));
            double percent = 10;
            val *= 1 + percent / 100;
            sum = val;
            Dispatcher.Invoke(() => {
                ConsoleBlock.Text += sum + "\n";
                progressBar1.Value += 100.0 / 12;
            });
        }
        #endregion
        #region 2
        private void ButtonStart2_Click(object sender, RoutedEventArgs e)
        {
            sum2 = 100;
            progressBar2.Value = 0;
            for (int i = 0; i < 12; i++)
            {
                new Thread(plusPercent2).Start();
            }
        }

        private void ButtonStop2_Click(object sender, RoutedEventArgs e)
        {

        }
        private double sum2;
        private readonly object locker2 = new();
        private void plusPercent2()
        {
            double val;
            lock (locker2)
            {
                val = sum2;
                Thread.Sleep(random.Next(250, 300));
                double percent = 10;
                val *= 1 + percent / 100;
                sum2 = val;
            }
            Dispatcher.Invoke(() => {
                ConsoleBlock.Text += sum2 + "\n";
                progressBar2.Value += 100.0 / 12;
            });
        }
        #endregion
        #region 3
        private void ButtonStart3_Click(object sender, RoutedEventArgs e)
        {
            sum3 = 100;
            progressBar3.Value = 0;
            for (int i = 0; i < 12; i++)
            {
                new Thread(plusPercent3).Start(i + 1);
            }
        }

        private void ButtonStop3_Click(object sender, RoutedEventArgs e)
        {

        }
        private double sum3;
        private readonly object locker3 = new();
        private void plusPercent3(object? month)
        {
            if (month is not int) return;

            double val;

            Thread.Sleep(random.Next(250, 300));
            double percent = 10 + (int)month;
            double factor = 1 + percent / 100;
    
            lock (locker3)
            {
                val = sum3;
                val *= factor;  
                sum3 = val;
            
            }
            Dispatcher.Invoke(() => {
                ConsoleBlock.Text += month + "---" + val + "\n";
                progressBar3.Value += 100.0 / 12;
            });
        }
        #endregion
        //public int Count = 1;
        //private void ButtonStart4_Click(object sender, RoutedEventArgs e)
        //{
        //    Count++;
        //    sum4 = 100;
        //    progressBar4.Value = 0;
        //    cts = new();
        //    for (int i = 0; i < 12; i++)
        //    {
        //        new Thread(plusPercent4).Start(cts.Token);
        //    }
        //}

        //private void ButtonStop4_Click(object sender, RoutedEventArgs e)
        //{
        //    cts?.Cancel();
        //    Count--;
        //    if(Count == 2)
        //    {

        //    }
        //}
        //private double sum4;
        //private readonly object locker4 = new();
        //private void plusPercent4(object? token)
        //{
        //    if (token is not CancellationToken) return;
        //    CancellationToken cancellationToken =(CancellationToken)token;

        //    double val;
        //    lock (locker4)
        //    {

        //        if (cancellationToken.IsCancellationRequested) return;
        //        val = sum4;
        //        Thread.Sleep(random.Next(250, 300));
        //        double percent = 10;
        //        val *= 1 + percent / 100;
        //        sum4 = val;
        //    }
        //    Dispatcher.Invoke(() => {
        //        ConsoleBlock.Text += val + "\n";
        //        progressBar4.Value += 100.0 / 12;
        //    });
        //}
        private bool isThreadRunning = false;
        private Thread thread;

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            if (!isThreadRunning)
            {
                isThreadRunning = true;
                thread = new Thread(plusPercent4);
                thread.Start();
                ButtonStart4.IsEnabled = false;
            }
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            if (isThreadRunning)
            {
                thread.Abort();
                isThreadRunning = false;
                ButtonStart4.IsEnabled = true;
                ButtonStart4.Content = "Resume";
            }
            else
            {
                isThreadRunning = true;
                thread = new Thread(plusPercent4);
                thread.Start();
                ButtonStart4.Content = "Start";
            }
        }

        private void plusPercent4()
        {
            try
            {
                // Some long running operation here
            }
            catch (ThreadAbortException)
            {
                // Thread was aborted
                Dispatcher.Invoke(() => {
                    ButtonStart4.Content = "Resume";
                    ButtonStart4.IsEnabled = true;
                });
                Thread.ResetAbort();
            }
            isThreadRunning = false;
            Dispatcher.Invoke(() => {
                ButtonStart4.IsEnabled = true;
                ButtonStart4.Content = "Start";
            });
        }
    }
}
