namespace WenKong
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_Import = new System.Windows.Forms.TextBox();
            this.btn_Import = new System.Windows.Forms.Button();
            this.btn_To = new System.Windows.Forms.Button();
            this.txt_To = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rbtn_Year = new System.Windows.Forms.RadioButton();
            this.rbtn_Month = new System.Windows.Forms.RadioButton();
            this.btn_Start = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.txt_Msg = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(395, 48);
            this.label1.TabIndex = 0;
            this.label1.Text = "温控txt文档数据处理，按两种方式：\r\n\r\n1、按年：探头——>年份（探头号为文件名，年份为sheet）\r\n2、按月：探头——>年份——>月份（探头+年份为文件名" +
    "，月份为sheet）";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "温控数据文本：";
            // 
            // txt_Import
            // 
            this.txt_Import.Location = new System.Drawing.Point(96, 67);
            this.txt_Import.Name = "txt_Import";
            this.txt_Import.Size = new System.Drawing.Size(400, 21);
            this.txt_Import.TabIndex = 2;
            // 
            // btn_Import
            // 
            this.btn_Import.Location = new System.Drawing.Point(496, 66);
            this.btn_Import.Name = "btn_Import";
            this.btn_Import.Size = new System.Drawing.Size(75, 23);
            this.btn_Import.TabIndex = 3;
            this.btn_Import.Text = "浏览";
            this.btn_Import.UseVisualStyleBackColor = true;
            this.btn_Import.Click += new System.EventHandler(this.btn_Import_Click);
            // 
            // btn_To
            // 
            this.btn_To.Location = new System.Drawing.Point(495, 95);
            this.btn_To.Name = "btn_To";
            this.btn_To.Size = new System.Drawing.Size(75, 23);
            this.btn_To.TabIndex = 6;
            this.btn_To.Text = "浏览";
            this.btn_To.UseVisualStyleBackColor = true;
            this.btn_To.Click += new System.EventHandler(this.btn_To_Click);
            // 
            // txt_To
            // 
            this.txt_To.Location = new System.Drawing.Point(96, 96);
            this.txt_To.Name = "txt_To";
            this.txt_To.Size = new System.Drawing.Size(399, 21);
            this.txt_To.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "保存数据目录：";
            // 
            // rbtn_Year
            // 
            this.rbtn_Year.AutoSize = true;
            this.rbtn_Year.Checked = true;
            this.rbtn_Year.Location = new System.Drawing.Point(17, 132);
            this.rbtn_Year.Name = "rbtn_Year";
            this.rbtn_Year.Size = new System.Drawing.Size(47, 16);
            this.rbtn_Year.TabIndex = 7;
            this.rbtn_Year.TabStop = true;
            this.rbtn_Year.Text = "按年";
            this.rbtn_Year.UseVisualStyleBackColor = true;
            // 
            // rbtn_Month
            // 
            this.rbtn_Month.AutoSize = true;
            this.rbtn_Month.Location = new System.Drawing.Point(70, 132);
            this.rbtn_Month.Name = "rbtn_Month";
            this.rbtn_Month.Size = new System.Drawing.Size(47, 16);
            this.rbtn_Month.TabIndex = 8;
            this.rbtn_Month.TabStop = true;
            this.rbtn_Month.Text = "按月";
            this.rbtn_Month.UseVisualStyleBackColor = true;
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(123, 129);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(75, 23);
            this.btn_Start.TabIndex = 9;
            this.btn_Start.Text = "开始";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(204, 129);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 10;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(298, 129);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(272, 23);
            this.progressBar1.TabIndex = 11;
            this.progressBar1.Visible = false;
            // 
            // txt_Msg
            // 
            this.txt_Msg.Location = new System.Drawing.Point(17, 168);
            this.txt_Msg.Multiline = true;
            this.txt_Msg.Name = "txt_Msg";
            this.txt_Msg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_Msg.Size = new System.Drawing.Size(553, 81);
            this.txt_Msg.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 261);
            this.Controls.Add(this.txt_Msg);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Start);
            this.Controls.Add(this.rbtn_Month);
            this.Controls.Add(this.rbtn_Year);
            this.Controls.Add(this.btn_To);
            this.Controls.Add(this.txt_To);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_Import);
            this.Controls.Add(this.txt_Import);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "温控数据处理";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_Import;
        private System.Windows.Forms.Button btn_Import;
        private System.Windows.Forms.Button btn_To;
        private System.Windows.Forms.TextBox txt_To;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbtn_Year;
        private System.Windows.Forms.RadioButton rbtn_Month;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox txt_Msg;
    }
}

