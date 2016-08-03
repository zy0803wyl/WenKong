using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WenKong
{
    public partial class Form1 : Form
    {
        private List<string> fileNames = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Import_Click(object sender, EventArgs e)
        {
            OpenFileDialog chooser = new OpenFileDialog();
            chooser.Title = "请选择温控数据txt文档";
            chooser.Filter = "文本文档|*.txt";
            if (chooser.ShowDialog() == DialogResult.OK)
            {
                txt_Import.Text = chooser.FileName;
                chooser.Dispose();
            }
        }

        private void btn_To_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog chooser = new FolderBrowserDialog();
            chooser.ShowNewFolderButton = true;
            if (chooser.ShowDialog() == DialogResult.OK)
            {
                txt_To.Text = chooser.SelectedPath;
                chooser.Dispose();
            }
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (File.Exists(txt_Import.Text))
            {
                if (Directory.Exists(txt_To.Text))
                {
                    progressBar1.Maximum = 100;
                    progressBar1.Minimum = 0;
                    //读取并处理数据
                    Start();
                }
                else
                {
                    MessageBox.Show("请选择空目录");
                    btn_To_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("请选择数据");
                btn_Import_Click(sender, e);
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.DialogResult dialogResult = MessageBox.Show("确认取消？", "警告", MessageBoxButtons.OKCancel);
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this.Close();
            }
        }

        private void Start()
        {
            SetState(false);
            if (txt_Msg.Text.Length > 0)
            {
                txt_Msg.Text = null;
            }
            Thread th = new Thread(WriteFileToCsv);
            th.IsBackground = true;
            th.Start();
            // Test();
        }

        private void WriteFileToCsv()
        {
            Dictionary<string, StringBuilder> dataDic = new Dictionary<string, StringBuilder>();
            using (StreamReader sr = new StreamReader(txt_Import.Text, Encoding.Default))
            {
                string temLine = null;
                int count = 0;
                int cishu = 0;
                IData data = null;
                SetMsg("开始写入数据。");

                #region 写入数据循环

                while ((temLine = sr.ReadLine()) != null)
                {
                    count++;
                    data = DataFactory(temLine);
                    //string line = string.Format("{0},{1},{2},{3},{4}", data.TYPE, data.ID, data.DATE, data.TIME, data.TEMP);
                    string line = data.GetData(temLine);
                    if (data.ID == "ID")
                    {
                        continue;
                    }
                    if (rbtn_Year.Checked == true)
                    {
                        //按年：2015_No.01
                        string fileName = data.DATE.Substring(0, 4) + "_" + data.ID;
                        if (!dataDic.Keys.Contains(fileName))
                        {
                            dataDic.Add(fileName, new StringBuilder(line).AppendLine());
                        }
                        else
                        {
                            dataDic[fileName] = dataDic[fileName].Append(line).AppendLine();
                        }
                    }
                    else
                    {
                        //按月：201501_No.01
                        string fileName = data.DATE.Substring(0, 6) + "_" + data.ID;
                        if (!dataDic.Keys.Contains(fileName))
                        {
                            dataDic.Add(fileName, new StringBuilder(line).AppendLine());
                        }
                        else
                        {
                            dataDic[fileName] = dataDic[fileName].Append(line).AppendLine();
                        }
                    }
                    if (count >= 5000000)
                    {
                        cishu++;
                        SetMsg("第 " + cishu.ToString() + " 次数据写入，请耐心等待");
                        //写入数据

                        #region 创建文件

                        foreach (string item in dataDic.Keys)
                        {
                            string fullPath = txt_To.Text + "\\" + item + ".csv";
                            CreatCsvFile(fullPath, data);
                        }

                        #endregion 创建文件

                        foreach (KeyValuePair<string, StringBuilder> item in dataDic)
                        {
                            WriteFile(item);
                        }
                        dataDic.Clear();
                        count = 0;
                    }
                }
                //读取文件之后剩余的行数
                if (dataDic.Count > 0)
                {
                    foreach (string item in dataDic.Keys)
                    {
                        string fullPath = txt_To.Text + "\\" + item + ".csv";
                        CreatCsvFile(fullPath, data);
                    }
                    foreach (KeyValuePair<string, StringBuilder> item in dataDic)
                    {
                        WriteFile(item);
                    }
                    dataDic.Clear();
                    count = 0;
                }

                #endregion 写入数据循环

                SetMsg("数据写入完成。");
            }
            //输出消息
            //修改开始按钮状态
            SetState(true);
            MessageBox.Show("数据拆分完成");
        }

        private IData DataFactory(string temLine)
        {
            string[] strs = temLine.Replace("\t\t", "\t").Split('\t');
            if (strs.Length > 6)
            {
                return new DataModelForNew();
            }
            else
            {
                return new DataModelForOld();
            }
        }

        private void CreatCsvFile(string fullPath, object data)
        {
            if (!File.Exists(fullPath))
            {
                //文件不存在
                using (StreamWriter sw = new StreamWriter(fullPath, true, Encoding.Default))
                {
                    // sw.WriteLine("TYPE,ID,DATE,TIME,TEMP");
                    PropertyInfo[] pros = data.GetType().GetProperties();
                    StringBuilder str = new StringBuilder();
                    foreach (PropertyInfo p in pros)
                    {
                        str.Append(p.Name + ",");
                    }
                    //移除多余的逗号
                    str.Remove(str.Length - 1, 1);
                    str.AppendLine();
                    sw.Write(str.ToString());
                }
            }
        }

        private void WriteFile(object data)
        {
            KeyValuePair<string, StringBuilder> dic = (KeyValuePair<string, StringBuilder>)data;
            //多线程写入文件——需要锁定当前读取的文件
            string fullPath = txt_To.Text + "\\" + dic.Key + ".csv";
            using (FileStream fs = new FileStream(fullPath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    sw.Write(dic.Value.ToString());
                }
            }
        }

        private void SetProsessBar(int value)
        {
            progressBar1.Value = value;
        }

        private void SetMsg(string msg)
        {
            if (txt_Msg.InvokeRequired)
            {
                StringBuilder strMsg = new StringBuilder();
                strMsg.Append(txt_Msg.Text);
                if (string.IsNullOrEmpty(strMsg.ToString()))
                {
                    strMsg.Append(msg);
                }
                else
                {
                    strMsg.AppendLine();
                    strMsg.Append(msg);
                }
                //lamda申明委托。匿名委托。
                Action ac = new Action(() =>
                {
                    //设置文本
                    txt_Msg.Text = "";
                    txt_Msg.Text = strMsg.ToString();
                    //滚动进度条
                    txt_Msg.SelectionLength = txt_Msg.Text.Length;
                    txt_Msg.ScrollToCaret();
                });
                this.Invoke(ac);
            }
            else
            {
                txt_Msg.Text = txt_Msg.Text + "\r\n" + msg;
            }
        }

        private delegate void SetBtn_StratStateDelegate(bool state);

        private void SetBtn_StratState(bool state)
        {
            if (this.btn_Start.InvokeRequired)
            {
                SetBtn_StratStateDelegate st = new SetBtn_StratStateDelegate(SetBtn_StratState);
                this.Invoke(st, state);
            }
            else
            {
                btn_Start.Enabled = state;
            }
        }

        private void SetState(bool state)
        {
            SetBtn_StratState(state);
        }

        private DataModelForOld GetData(string str)
        {
            //TYPE		ID		DATE		TIME		TEMP
            string[] datas = str.Replace("\t\t", "\t").Split('\t');
            DataModelForOld model = new DataModelForOld()
            {
            };
            return model;
        }

        private void Test()
        {
            string fileName = txt_Import.Text;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string temLine = null;
                    while ((temLine = sr.ReadLine()) != null)
                    {
                        temLine = temLine.Replace("\t\t", "\t");
                    }
                }
            }
        }
    }

    internal class DataModelForOld : IData
    {
        //TYPE		ID		DATE		TIME		TEMP
        //CHECK		NO.02		20150319	14:18:30	-070.0буC

        public string TYPE { get; set; }

        public string ID { get; set; }

        public string DATE { get; set; }

        public string TIME { get; set; }

        public string TEMP { get; set; }

        public string GetData(string str)
        {
            string[] datas = str.Replace("\t\t", "\t").Split('\t');
            this.TYPE = datas[0];
            this.ID = datas[1];
            this.DATE = datas[2];
            this.TIME = datas[3];
            this.TEMP = datas[4];
            return string.Format("{0},{1},{2},{3},{4}", this.TYPE, this.ID, this.DATE, this.TIME, this.TEMP);
        }
    }

    internal class DataModelForNew : IData
    {
        //TYPE		ID		ADDRESS		DESCRIBE	DATE		TIME		TEMP
        //Dopen		NO.003		01        	02        	20151223	17:24:54	-029.5буC

        public string TYPE { get; set; }

        public string ID { get; set; }

        public string ADDRESS { get; set; }
        public string DESCRIBE { get; set; }

        public string DATE { get; set; }

        public string TIME { get; set; }

        public string TEMP { get; set; }

        public string GetData(string str)
        {
            //TYPE		ID		ADDRESS		DESCRIBE	DATE		TIME		TEMP
            string[] datas = str.Replace("\t\t", "\t").Split('\t');
            this.TYPE = datas[0];
            this.ID = datas[1];
            this.ADDRESS = datas[2];
            this.DESCRIBE = datas[3];
            this.DATE = datas[4];
            this.TIME = datas[5];
            this.TEMP = datas[6];
            return string.Format("{0},{1},{2},{3},{4},{5},{6}", this.TYPE, this.ID, this.ADDRESS, this.DESCRIBE, this.DATE, this.TIME, this.TEMP);
        }
    }

    internal interface IData
    {
        //TYPE		ID		ADDRESS		DESCRIBE	DATE		TIME		TEMP
        //Dopen		NO.003		01        	02        	20151223	17:24:54	-029.5буC
        string TYPE { get; set; }

        string ID { get; set; }

        string DATE { get; set; }

        string TIME { get; set; }

        string TEMP { get; set; }

        //获取数据方法
        string GetData(string str);
    }
}