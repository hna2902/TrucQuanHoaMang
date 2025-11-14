namespace TrucQuanHoaMang
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GroupBox_CreateRandom = new System.Windows.Forms.GroupBox();
            this.btnCreate_Random = new System.Windows.Forms.Button();
            this.input_SizeRandom = new System.Windows.Forms.NumericUpDown();
            this.Label_RandomSize = new System.Windows.Forms.Label();
            this.GroupBox_Act = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.input_PositionAct = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.input_ValueAct = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.GroupBox_CreateManual = new System.Windows.Forms.GroupBox();
            this.btnCreate_Array = new System.Windows.Forms.Button();
            this.btnCreate_Index = new System.Windows.Forms.Button();
            this.input_PositionManual = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.input_ValueManual = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.GroupBox_Sort = new System.Windows.Forms.GroupBox();
            this.btn_ContinueSort = new System.Windows.Forms.Button();
            this.btn_Back = new System.Windows.Forms.Button();
            this.btn_StopSort = new System.Windows.Forms.Button();
            this.Label_ManualSort = new System.Windows.Forms.Label();
            this.Label_AutoSort = new System.Windows.Forms.Label();
            this.tbSpeed = new System.Windows.Forms.TrackBar();
            this.Label_Speed = new System.Windows.Forms.Label();
            this.btn_AutoSort = new System.Windows.Forms.Button();
            this.Algorithm_Sort = new System.Windows.Forms.ComboBox();
            this.panelVisualizer = new System.Windows.Forms.Panel();
            this.panel = new System.Windows.Forms.Panel();
            this.GroupBox_Algorithm = new System.Windows.Forms.GroupBox();
            this.btnClearArray = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.GroupBox_CreateRandom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.input_SizeRandom)).BeginInit();
            this.GroupBox_Act.SuspendLayout();
            this.GroupBox_CreateManual.SuspendLayout();
            this.GroupBox_Sort.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbSpeed)).BeginInit();
            this.panel.SuspendLayout();
            this.GroupBox_Algorithm.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox_CreateRandom
            // 
            this.GroupBox_CreateRandom.Controls.Add(this.btnCreate_Random);
            this.GroupBox_CreateRandom.Controls.Add(this.input_SizeRandom);
            this.GroupBox_CreateRandom.Controls.Add(this.Label_RandomSize);
            this.GroupBox_CreateRandom.Location = new System.Drawing.Point(3, 3);
            this.GroupBox_CreateRandom.Name = "GroupBox_CreateRandom";
            this.GroupBox_CreateRandom.Size = new System.Drawing.Size(200, 131);
            this.GroupBox_CreateRandom.TabIndex = 0;
            this.GroupBox_CreateRandom.TabStop = false;
            this.GroupBox_CreateRandom.Text = "Khởi tạo mảng ngẫu nhiên";
            // 
            // btnCreate_Random
            // 
            this.btnCreate_Random.Location = new System.Drawing.Point(119, 102);
            this.btnCreate_Random.Name = "btnCreate_Random";
            this.btnCreate_Random.Size = new System.Drawing.Size(75, 23);
            this.btnCreate_Random.TabIndex = 2;
            this.btnCreate_Random.Text = "Tạo Mảng";
            this.btnCreate_Random.UseVisualStyleBackColor = true;
            this.btnCreate_Random.Click += new System.EventHandler(this.btnCreate_Random_Click);
            // 
            // input_SizeRandom
            // 
            this.input_SizeRandom.Location = new System.Drawing.Point(75, 48);
            this.input_SizeRandom.Name = "input_SizeRandom";
            this.input_SizeRandom.Size = new System.Drawing.Size(120, 20);
            this.input_SizeRandom.TabIndex = 1;
            // 
            // Label_RandomSize
            // 
            this.Label_RandomSize.AutoSize = true;
            this.Label_RandomSize.Location = new System.Drawing.Point(9, 54);
            this.Label_RandomSize.Name = "Label_RandomSize";
            this.Label_RandomSize.Size = new System.Drawing.Size(60, 13);
            this.Label_RandomSize.TabIndex = 0;
            this.Label_RandomSize.Text = "Kích thước";
            // 
            // GroupBox_Act
            // 
            this.GroupBox_Act.Controls.Add(this.btnSearch);
            this.GroupBox_Act.Controls.Add(this.btnDelete);
            this.GroupBox_Act.Controls.Add(this.btnInsert);
            this.GroupBox_Act.Controls.Add(this.input_PositionAct);
            this.GroupBox_Act.Controls.Add(this.label3);
            this.GroupBox_Act.Controls.Add(this.input_ValueAct);
            this.GroupBox_Act.Controls.Add(this.label2);
            this.GroupBox_Act.Location = new System.Drawing.Point(415, 3);
            this.GroupBox_Act.Name = "GroupBox_Act";
            this.GroupBox_Act.Size = new System.Drawing.Size(200, 131);
            this.GroupBox_Act.TabIndex = 1;
            this.GroupBox_Act.TabStop = false;
            this.GroupBox_Act.Text = "Thao tác trên phần tử";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(136, 102);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(59, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Tìm";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(71, 102);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(59, 23);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(6, 102);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(59, 23);
            this.btnInsert.TabIndex = 4;
            this.btnInsert.Text = "Thêm";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // input_PositionAct
            // 
            this.input_PositionAct.Location = new System.Drawing.Point(61, 66);
            this.input_PositionAct.Name = "input_PositionAct";
            this.input_PositionAct.Size = new System.Drawing.Size(100, 20);
            this.input_PositionAct.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Vị trí";
            // 
            // input_ValueAct
            // 
            this.input_ValueAct.Location = new System.Drawing.Point(61, 40);
            this.input_ValueAct.Name = "input_ValueAct";
            this.input_ValueAct.Size = new System.Drawing.Size(100, 20);
            this.input_ValueAct.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Giá trị";
            // 
            // GroupBox_CreateManual
            // 
            this.GroupBox_CreateManual.Controls.Add(this.btnCreate_Array);
            this.GroupBox_CreateManual.Controls.Add(this.btnCreate_Index);
            this.GroupBox_CreateManual.Controls.Add(this.input_PositionManual);
            this.GroupBox_CreateManual.Controls.Add(this.label4);
            this.GroupBox_CreateManual.Controls.Add(this.input_ValueManual);
            this.GroupBox_CreateManual.Controls.Add(this.label5);
            this.GroupBox_CreateManual.Location = new System.Drawing.Point(209, 3);
            this.GroupBox_CreateManual.Name = "GroupBox_CreateManual";
            this.GroupBox_CreateManual.Size = new System.Drawing.Size(200, 131);
            this.GroupBox_CreateManual.TabIndex = 7;
            this.GroupBox_CreateManual.TabStop = false;
            this.GroupBox_CreateManual.Text = "Khởi tạo mảng";
            // 
            // btnCreate_Array
            // 
            this.btnCreate_Array.Location = new System.Drawing.Point(135, 102);
            this.btnCreate_Array.Name = "btnCreate_Array";
            this.btnCreate_Array.Size = new System.Drawing.Size(59, 23);
            this.btnCreate_Array.TabIndex = 5;
            this.btnCreate_Array.Text = "Hoàn tất";
            this.btnCreate_Array.UseVisualStyleBackColor = true;
            this.btnCreate_Array.Click += new System.EventHandler(this.btnCreate_Array_Click);
            // 
            // btnCreate_Index
            // 
            this.btnCreate_Index.Location = new System.Drawing.Point(70, 102);
            this.btnCreate_Index.Name = "btnCreate_Index";
            this.btnCreate_Index.Size = new System.Drawing.Size(59, 23);
            this.btnCreate_Index.TabIndex = 4;
            this.btnCreate_Index.Text = "Thêm";
            this.btnCreate_Index.UseVisualStyleBackColor = true;
            this.btnCreate_Index.Click += new System.EventHandler(this.btnCreate_Index_Click);
            // 
            // input_PositionManual
            // 
            this.input_PositionManual.Location = new System.Drawing.Point(70, 66);
            this.input_PositionManual.Name = "input_PositionManual";
            this.input_PositionManual.Size = new System.Drawing.Size(100, 20);
            this.input_PositionManual.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Vị trí";
            // 
            // input_ValueManual
            // 
            this.input_ValueManual.Location = new System.Drawing.Point(70, 40);
            this.input_ValueManual.Name = "input_ValueManual";
            this.input_ValueManual.Size = new System.Drawing.Size(100, 20);
            this.input_ValueManual.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Giá trị";
            // 
            // GroupBox_Sort
            // 
            this.GroupBox_Sort.Controls.Add(this.btn_ContinueSort);
            this.GroupBox_Sort.Controls.Add(this.btn_Back);
            this.GroupBox_Sort.Controls.Add(this.btn_StopSort);
            this.GroupBox_Sort.Controls.Add(this.Label_ManualSort);
            this.GroupBox_Sort.Controls.Add(this.Label_AutoSort);
            this.GroupBox_Sort.Controls.Add(this.tbSpeed);
            this.GroupBox_Sort.Controls.Add(this.Label_Speed);
            this.GroupBox_Sort.Controls.Add(this.btn_AutoSort);
            this.GroupBox_Sort.Location = new System.Drawing.Point(3, 200);
            this.GroupBox_Sort.Name = "GroupBox_Sort";
            this.GroupBox_Sort.Size = new System.Drawing.Size(612, 136);
            this.GroupBox_Sort.TabIndex = 8;
            this.GroupBox_Sort.TabStop = false;
            this.GroupBox_Sort.Text = "Duyệt mảng";
            // 
            // btn_ContinueSort
            // 
            this.btn_ContinueSort.Location = new System.Drawing.Point(341, 106);
            this.btn_ContinueSort.Name = "btn_ContinueSort";
            this.btn_ContinueSort.Size = new System.Drawing.Size(207, 23);
            this.btn_ContinueSort.TabIndex = 9;
            this.btn_ContinueSort.Text = "Tiếp tục";
            this.btn_ContinueSort.UseVisualStyleBackColor = true;
            // 
            // btn_Back
            // 
            this.btn_Back.Location = new System.Drawing.Point(118, 106);
            this.btn_Back.Name = "btn_Back";
            this.btn_Back.Size = new System.Drawing.Size(207, 23);
            this.btn_Back.TabIndex = 8;
            this.btn_Back.Text = "Quay lại";
            this.btn_Back.UseVisualStyleBackColor = true;
            // 
            // btn_StopSort
            // 
            this.btn_StopSort.Location = new System.Drawing.Point(341, 24);
            this.btn_StopSort.Name = "btn_StopSort";
            this.btn_StopSort.Size = new System.Drawing.Size(206, 23);
            this.btn_StopSort.TabIndex = 7;
            this.btn_StopSort.Text = "Ngưng";
            this.btn_StopSort.UseVisualStyleBackColor = true;
            this.btn_StopSort.Click += new System.EventHandler(this.btn_StopSort_Click_1);
            // 
            // Label_ManualSort
            // 
            this.Label_ManualSort.AutoSize = true;
            this.Label_ManualSort.Location = new System.Drawing.Point(9, 111);
            this.Label_ManualSort.Name = "Label_ManualSort";
            this.Label_ManualSort.Size = new System.Drawing.Size(80, 13);
            this.Label_ManualSort.TabIndex = 6;
            this.Label_ManualSort.Text = "Duyệt thủ công";
            // 
            // Label_AutoSort
            // 
            this.Label_AutoSort.AutoSize = true;
            this.Label_AutoSort.Location = new System.Drawing.Point(9, 29);
            this.Label_AutoSort.Name = "Label_AutoSort";
            this.Label_AutoSort.Size = new System.Drawing.Size(75, 13);
            this.Label_AutoSort.TabIndex = 5;
            this.Label_AutoSort.Text = "Duyệt tự động";
            // 
            // tbSpeed
            // 
            this.tbSpeed.Location = new System.Drawing.Point(119, 68);
            this.tbSpeed.Minimum = 1;
            this.tbSpeed.Name = "tbSpeed";
            this.tbSpeed.Size = new System.Drawing.Size(206, 45);
            this.tbSpeed.TabIndex = 4;
            this.tbSpeed.Value = 1;
            // 
            // Label_Speed
            // 
            this.Label_Speed.AutoSize = true;
            this.Label_Speed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Label_Speed.Location = new System.Drawing.Point(8, 68);
            this.Label_Speed.Name = "Label_Speed";
            this.Label_Speed.Size = new System.Drawing.Size(71, 13);
            this.Label_Speed.TabIndex = 3;
            this.Label_Speed.Text = "Tốc độ duyệt";
            // 
            // btn_AutoSort
            // 
            this.btn_AutoSort.Location = new System.Drawing.Point(119, 24);
            this.btn_AutoSort.Name = "btn_AutoSort";
            this.btn_AutoSort.Size = new System.Drawing.Size(206, 23);
            this.btn_AutoSort.TabIndex = 2;
            this.btn_AutoSort.Text = "Duyệt";
            this.btn_AutoSort.UseVisualStyleBackColor = true;
            // 
            // Algorithm_Sort
            // 
            this.Algorithm_Sort.FormattingEnabled = true;
            this.Algorithm_Sort.Items.AddRange(new object[] {
            "Bubble Sort",
            "Insertion Sort"});
            this.Algorithm_Sort.Location = new System.Drawing.Point(119, 19);
            this.Algorithm_Sort.Name = "Algorithm_Sort";
            this.Algorithm_Sort.Size = new System.Drawing.Size(206, 21);
            this.Algorithm_Sort.TabIndex = 1;
            // 
            // panelVisualizer
            // 
            this.panelVisualizer.Location = new System.Drawing.Point(3, 342);
            this.panelVisualizer.Name = "panelVisualizer";
            this.panelVisualizer.Size = new System.Drawing.Size(612, 82);
            this.panelVisualizer.TabIndex = 9;
            // 
            // panel
            // 
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Controls.Add(this.GroupBox_Algorithm);
            this.panel.Controls.Add(this.btnClearArray);
            this.panel.Controls.Add(this.statusStrip1);
            this.panel.Controls.Add(this.panelVisualizer);
            this.panel.Controls.Add(this.GroupBox_Sort);
            this.panel.Controls.Add(this.GroupBox_CreateManual);
            this.panel.Controls.Add(this.GroupBox_Act);
            this.panel.Controls.Add(this.GroupBox_CreateRandom);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(620, 455);
            this.panel.TabIndex = 0;
            // 
            // GroupBox_Algorithm
            // 
            this.GroupBox_Algorithm.Controls.Add(this.Algorithm_Sort);
            this.GroupBox_Algorithm.Location = new System.Drawing.Point(3, 134);
            this.GroupBox_Algorithm.Name = "GroupBox_Algorithm";
            this.GroupBox_Algorithm.Size = new System.Drawing.Size(611, 65);
            this.GroupBox_Algorithm.TabIndex = 12;
            this.GroupBox_Algorithm.TabStop = false;
            this.GroupBox_Algorithm.Text = "Thuật toán";
            // 
            // btnClearArray
            // 
            this.btnClearArray.Location = new System.Drawing.Point(3, 427);
            this.btnClearArray.Name = "btnClearArray";
            this.btnClearArray.Size = new System.Drawing.Size(92, 21);
            this.btnClearArray.TabIndex = 11;
            this.btnClearArray.Text = "Xóa Mảng";
            this.btnClearArray.UseVisualStyleBackColor = true;
            this.btnClearArray.Click += new System.EventHandler(this.btnClearArray_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(489, 432);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(56, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Ready";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 455);
            this.Controls.Add(this.panel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.GroupBox_CreateRandom.ResumeLayout(false);
            this.GroupBox_CreateRandom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.input_SizeRandom)).EndInit();
            this.GroupBox_Act.ResumeLayout(false);
            this.GroupBox_Act.PerformLayout();
            this.GroupBox_CreateManual.ResumeLayout(false);
            this.GroupBox_CreateManual.PerformLayout();
            this.GroupBox_Sort.ResumeLayout(false);
            this.GroupBox_Sort.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbSpeed)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.GroupBox_Algorithm.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GroupBox_CreateRandom;
        private System.Windows.Forms.Button btnCreate_Random;
        private System.Windows.Forms.NumericUpDown input_SizeRandom;
        private System.Windows.Forms.Label Label_RandomSize;
        private System.Windows.Forms.GroupBox GroupBox_Act;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.TextBox input_PositionAct;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox input_ValueAct;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox GroupBox_CreateManual;
        private System.Windows.Forms.Button btnCreate_Array;
        private System.Windows.Forms.Button btnCreate_Index;
        private System.Windows.Forms.TextBox input_PositionManual;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox input_ValueManual;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox GroupBox_Sort;
        private System.Windows.Forms.TrackBar tbSpeed;
        private System.Windows.Forms.Label Label_Speed;
        private System.Windows.Forms.Button btn_AutoSort;
        private System.Windows.Forms.ComboBox Algorithm_Sort;
        private System.Windows.Forms.Panel panelVisualizer;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Button btnClearArray;
        private System.Windows.Forms.GroupBox GroupBox_Algorithm;
        private System.Windows.Forms.Label Label_AutoSort;
        private System.Windows.Forms.Button btn_StopSort;
        private System.Windows.Forms.Label Label_ManualSort;
        private System.Windows.Forms.Button btn_ContinueSort;
        private System.Windows.Forms.Button btn_Back;
    }
}

