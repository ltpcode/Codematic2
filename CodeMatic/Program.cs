using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
namespace Codematic
{
    static class Program
    {     

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Subscribe to thread (unhandled) exception events
            ThreadExceptionHandler handler = new ThreadExceptionHandler();
            Application.ThreadException += new ThreadExceptionEventHandler(handler.Application_ThreadException);


            //Application.Run(new Form2());
            MainForm app = new MainForm();
            if (app.mutex != null)
            {               
                Application.Run(app);
            }
            else
            {
                MessageBox.Show(app, "动软代码生成器已经启动！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    /// 
    /// Handles a thread (unhandled) exception.
    /// 
    internal class ThreadExceptionHandler
    {
        // Handles the thread exception. 
        public void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                // Exit the program if the user clicks Abort.
                DialogResult result = ShowThreadExceptionDialog(e.Exception);
                if (result == DialogResult.Abort)
                    Application.Exit();
            }
            catch
            {                
                try
                {
                    MessageBox.Show("严重错误", "严重错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }
        }

        /// 
        /// Creates and displays the error message.
        /// 
        private DialogResult ShowThreadExceptionDialog(Exception ex)
        {
            string errorMessage = "错误信息：\n\n" +
                ex.Message + "\n\n" + ex.GetType() +
                "\n\nStack Trace:\n" +
                ex.StackTrace;                        
            SendErrInfo infoform = new SendErrInfo(errorMessage);
            return infoform.ShowDialog();
        }
    }
}