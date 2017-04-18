namespace EnsembleDecisionTree
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
            this.txt_file_name = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.set_view = new System.Windows.Forms.DataGridView();
            this.DecisionTree = new System.Windows.Forms.TreeView();
            this.txt_discrete = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_cont = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_class = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.set_view)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_file_name
            // 
            this.txt_file_name.Location = new System.Drawing.Point(84, 47);
            this.txt_file_name.Name = "txt_file_name";
            this.txt_file_name.Size = new System.Drawing.Size(197, 20);
            this.txt_file_name.TabIndex = 0;
            this.txt_file_name.TextChanged += new System.EventHandler(this.txt_file_name_TextChanged);
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(84, 98);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(125, 23);
            this.btn_ok.TabIndex = 1;
            this.btn_ok.Text = "ok";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "File Name";
            // 
            // set_view
            // 
            this.set_view.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.set_view.Location = new System.Drawing.Point(12, 225);
            this.set_view.Name = "set_view";
            this.set_view.Size = new System.Drawing.Size(451, 329);
            this.set_view.TabIndex = 3;
            // 
            // DecisionTree
            // 
            this.DecisionTree.Location = new System.Drawing.Point(502, 225);
            this.DecisionTree.Name = "DecisionTree";
            this.DecisionTree.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.DecisionTree.Size = new System.Drawing.Size(495, 329);
            this.DecisionTree.TabIndex = 4;
            this.DecisionTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.DecisionTree_AfterSelect);
            // 
            // txt_discrete
            // 
            this.txt_discrete.Location = new System.Drawing.Point(741, 16);
            this.txt_discrete.Name = "txt_discrete";
            this.txt_discrete.Size = new System.Drawing.Size(149, 20);
            this.txt_discrete.TabIndex = 5;
            this.txt_discrete.TextChanged += new System.EventHandler(this.txt_discrete_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(581, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "No of Discrete attribute";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(581, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "No of Continuous attribute";
            // 
            // txt_cont
            // 
            this.txt_cont.Location = new System.Drawing.Point(741, 42);
            this.txt_cont.Name = "txt_cont";
            this.txt_cont.Size = new System.Drawing.Size(149, 20);
            this.txt_cont.TabIndex = 7;
            this.txt_cont.TextChanged += new System.EventHandler(this.txt_cont_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(581, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "No of Class:";
            // 
            // txt_class
            // 
            this.txt_class.Location = new System.Drawing.Point(741, 68);
            this.txt_class.Name = "txt_class";
            this.txt_class.Size = new System.Drawing.Size(149, 20);
            this.txt_class.TabIndex = 9;
            this.txt_class.TextChanged += new System.EventHandler(this.txt_class_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(754, 127);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "create";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(179, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Training Set";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(711, 199);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Decision Tree";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1034, 599);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_class);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_cont);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_discrete);
            this.Controls.Add(this.DecisionTree);
            this.Controls.Add(this.set_view);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.txt_file_name);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.set_view)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_file_name;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView set_view;
        private System.Windows.Forms.TreeView DecisionTree;
        private System.Windows.Forms.TextBox txt_discrete;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_cont;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_class;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

