using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Threading;
using UpdateApp.UpdateService;
namespace UpdateApp
{
	/// <summary>
	/// Form1 的摘要说明。
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label StatusLabel1;
		private System.Windows.Forms.ProgressBar progressBar1;
        delegate void SetStatusCallback(string text);
        delegate void SetProBar1MaxCallback(int val);
        delegate void SetProBar1ValCallback(int val);
		Thread mythread;
		Mutex mutex;
        private PictureBox pictureBox1;
        string logfilename = Application.StartupPath + "\\logInfo.txt";

		#region 
		public Form1()
		{			
			InitializeComponent();
			mutex = new Mutex(false, "UpdateApp_MUTEX");
			if (!mutex.WaitOne(0, false)) 
			{
				mutex.Close();
				mutex = null;
			}	
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.StatusLabel1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // StatusLabel1
            // 
            this.StatusLabel1.AutoSize = true;
            this.StatusLabel1.Location = new System.Drawing.Point(21, 73);
            this.StatusLabel1.Name = "StatusLabel1";
            this.StatusLabel1.Size = new System.Drawing.Size(155, 12);
            this.StatusLabel1.TabIndex = 0;
            this.StatusLabel1.Text = "连接服务器，检测更新中...";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(0, 110);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(292, 19);
            this.progressBar1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(19, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(292, 131);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.StatusLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统更新";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main() 
		{			
			Application.EnableVisualStyles();
			Application.DoEvents(); 
						
			Form1 app = new Form1();
			if (app.mutex != null) 
			{
				Application.Run(app);				
			}
			else
			{
				DialogResult dia=MessageBox.Show(app,"程序已经在运行！终止当前应用吗？","系统提示",MessageBoxButtons.YesNo,MessageBoxIcon.Information);
				if(dia==DialogResult.Yes)
				{
					Application.Exit();
				}
			}
		}

        /// <summary>
        /// 循环网址进度最大值
        /// </summary>
        /// <param name="val"></param>
        public void SetprogressBar1Max(int val)
        {
            if (this.progressBar1.InvokeRequired)
            {
                SetProBar1MaxCallback d = new SetProBar1MaxCallback(SetprogressBar1Max);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.progressBar1.Maximum = val;

            }
        }
        /// <summary>
        /// 循环网址进度
        /// </summary>
        /// <param name="val"></param>
        public void SetprogressBar1Val(int val)
        {
            if (this.progressBar1.InvokeRequired)
            {
                SetProBar1ValCallback d = new SetProBar1ValCallback(SetprogressBar1Val);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.progressBar1.Value = val;

            }
        }

		#endregion

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="loginfo"></param>
        public void WriteLog(string loginfo)
        {            
            StreamWriter sw = new StreamWriter(logfilename, true);
            sw.WriteLine(DateTime.Now.ToString() + ":" + loginfo);
            sw.Close();
        }

		private void Form1_Load(object sender, System.EventArgs e)
		{
			if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
			{
				this.Close();
			}
			try
			{
                mythread = new Thread(new ThreadStart(ProcUpdate));
                mythread.Start();
                //ProcUpdate();
			}
			catch (System.Exception er)
			{
				MessageBox.Show(er.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		#region 进行升级下载文件

		void ProcUpdate()
		{
			this.StatusLabel1.Text = "连接服务器，检测更新中...";
            bool iserr = false;
            //计算总行数
            int linecount = 0;
            int linesucc = 0;//成功数 
            bool restartInstall = false;
			try
			{                
				UpServer upser = new UpServer();				
				if (upser.GetFileList() != "")
				{
					string currpath = Application.StartupPath;
					string serverurl = upser.GetServerUrl();
					string folder = upser.GetNewFilePath();
					
                    //下载[新文件列表]文件
					SetMessage("下载文件列表...");
					string fileUrl = serverurl + "/" + upser.GetFileList();
					string filelistname = currpath + "\\Updatelist.ini";
					DownFile(fileUrl, filelistname);
                                        
                    StreamReader srline = File.OpenText(filelistname);
                    while (srline.ReadLine()!=null)
                    {
                        linecount++;
                    }
                    srline.Close();
                    SetprogressBar1Max(linecount);

					//开始按列表文件 开始 下载程序文件					
					StreamReader sr = File.OpenText(filelistname);
                    int currline = 1;
					String strfile;
					while ((strfile = sr.ReadLine()) != null)
					{
                        try
                        {
                            SetMessage("正在下载 " + strfile);
                            string fileR = serverurl + "/" + folder + "/" + strfile;
                            string fileL = currpath + "\\" + strfile; 

                            if (strfile.EndsWith(".msi"))
                            {
                                DirectoryInfo target = new DirectoryInfo("c:\\Codematic");
                                if (!target.Exists)
                                {
                                    target.Create();
                                }
                                fileL = "c:\\Codematic" + "\\" + strfile; ;
                                restartInstall = true;                                
                            }                            
                            DownFile(fileR, fileL);
                            linesucc++;
                        }
                        catch (System.Exception ex)
                        {
                            iserr = true;
                            WriteLog(strfile+":"+ex.Message);
                        }
                        SetprogressBar1Val(currline);
                        currline++;
					}                    
                    SetMessage("下载完成。");
					sr.Close();					
				}				
			}
			catch (System.Exception ex)
			{
				string s = ex.Message;
                iserr = true;
                WriteLog(s);
			}
			finally
			{}
            //更新失败
            if (iserr)
            {
                try
                {
                    string errinfo = "程序更新失败，建议直接在官方网站下载最新版本安装。";
                    int fileerr = linecount - linesucc;
                    if (fileerr > 0)
                    {
                        errinfo = "程序更新失败"+fileerr.ToString()+"个文件未更新成功，建议直接在官方网站下载最新版本安装。";
                    }
                    DialogResult dia = MessageBox.Show(this, errinfo, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dia == DialogResult.Yes)
                    {
                        //Process proc = new Process();
                        Process.Start("IExplore.exe", "http://www.maticsoft.com/download.aspx");
                    }

                }
                catch
                {
                    MessageBox.Show("请访问：http://www.maticsoft.com", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (restartInstall)
            {
                ReStartInstall();
            }
            else
            {
                StartApp();
            }
            
		}

        //文件下载
        public void DownFile(string fileurl, string localFilename)
        {
            WebClient myclient = new WebClient();
            myclient.DownloadFile(fileurl, localFilename);
            myclient.Dispose();
        }
        //文件信息
        private void SetMessage(string msg)
        {
            if (this.StatusLabel1.InvokeRequired)
            {
                SetStatusCallback d = new SetStatusCallback(SetMessage);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.StatusLabel1.Text = msg;

            }
        }
		#endregion

        #region 下载完毕，启动应用程序
        /// <summary>
        /// 启动程序
        /// </summary>
        private void StartApp()
		{
            this.Hide();
			DialogResult dia=MessageBox.Show(this,"程序更新成功，请关闭当前窗口重新打开应用程序。\n你想现在打开吗？","系统提示",MessageBoxButtons.YesNo,MessageBoxIcon.Information);
			if(dia==DialogResult.Yes)
			{
				string strapp = Application.StartupPath + @"\Codematic.exe";
				if (File.Exists(strapp))
				{
					Process.Start(strapp);
				}
			}
			Application.Exit();
        }
        /// <summary>
        /// 重新安装
        /// </summary>
        private void ReStartInstall()
        {
            //this.Hide();
            DialogResult dia = MessageBox.Show(this, "程序下载成功，请关闭当前窗口重新安装最新程序。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (dia == DialogResult.OK)
            {
                string DownPath = @"c:\Codematic";
                Process.Start("explorer.exe", DownPath);
            }
            Application.Exit();
        }
        #endregion

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            //if ((mythread.ThreadState == System.Threading.ThreadState.Running) || (mythread.ThreadState == System.Threading.ThreadState.WaitSleepJoin))
            //{
            //    e.Cancel = true;
            //}
		}
	}
}
