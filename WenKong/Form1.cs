using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Thread th = new Thread(WriteFileToCsv);
            th.IsBackground = true;
            th.Start();
        }

        private void WriteFileToCsv()
        {
            Dictionary<string, StringBuilder> dataDic = new Dictionary<string, StringBuilder>();
            using (StreamReader sr = new StreamReader(txt_Import.Text))
            {
                string temLine = null;
                int count = 0;
                int cishu = 0;
                SetMsg("开始写入数据。");

                #region 写入数据循环

                while ((temLine = sr.ReadLine()) != null)
                {
                    count++;
                    DataModel model = GetData(temLine);
                    if (model.ID == "ID")
                    {
                        continue;
                    }
                    string line = string.Format("{0},{1},{2},{3},{4}", model.TYPE, model.ID, model.DATE, model.TIME, model.TEMP);

                    if (rbtn_Year.Checked == true)
                    {
                        //按年：2015_No.01
                        string fileName = model.DATE.Substring(0, 4) + "_" + model.ID;
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
                        string fileName = model.DATE.Substring(0, 6) + "_" + model.ID;
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
                        //SetMsgDelegate sm = new SetMsgDelegate(SetMsg);
                        //写入数据

                        #region 创建文件

                        foreach (string item in dataDic.Keys)
                        {
                            string fullPath = txt_To.Text + "\\" + item + ".csv";
                            if (!File.Exists(fullPath))
                            {
                                //文件不存在
                                using (StreamWriter sw = new StreamWriter(fullPath, true, Encoding.UTF8))
                                {
                                    sw.WriteLine("TYPE,ID,DATE,TIME,TEMP");
                                }
                            }
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
                        if (!File.Exists(fullPath))
                        {
                            //文件不存在
                            using (StreamWriter sw = new StreamWriter(fullPath, true, Encoding.UTF8))
                            {
                                sw.WriteLine("TYPE,ID,DATE,TIME,TEMP");
                            }
                        }
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

        private void WriteFile(object data)
        {
            KeyValuePair<string, StringBuilder> dic = (KeyValuePair<string, StringBuilder>)data;
            //多线程写入文件——需要锁定当前读取的文件
            string fullPath = txt_To.Text + "\\" + dic.Key + ".csv";
            using (FileStream fs = new FileStream(fullPath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
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

        private DataModel GetData(string str)
        {
            //TYPE		ID		DATE		TIME		TEMP
            string[] datas = str.Replace("\t\t", "\t").Split('\t');
            DataModel model = new DataModel()
            {
                TYPE = datas[0],
                ID = datas[1],
                DATE = datas[2],
                TIME = datas[3],
                TEMP = datas[4]
            };
            return model;
        }
    }

    internal class DataModel
    {
        //TYPE		ID		DATE		TIME		TEMP
        //CHECK		NO.02		20150319	14:18:30	-070.0буC
        public string TYPE { get; set; }

        private string id;

        public string ID
        {
            get { return id.Replace(".", "_"); }
            set { id = value; }
        }

        public string DATE { get; set; }
        public string TIME { get; set; }
        private string temp;

        public string TEMP
        {
            get { return temp.Substring(0, 6); }
            set { temp = value; }
        }
    }
}