namespace TestControl
{
    partial class MainForm
    {
        /// <summary>
        /// Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.Form"/>.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(components != null)
                {
                    components.Dispose();
                }
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
            this.gpbTrayMe = new System.Windows.Forms.GroupBox();
            this.windowFinder = new WindowFinder.WindowFinder();
            this.lblWindowHandle = new System.Windows.Forms.Label();
            this.txtWindowHandle = new System.Windows.Forms.TextBox();
            this.lblWindowText = new System.Windows.Forms.Label();
            this.txtWindowText = new System.Windows.Forms.TextBox();
            this.lblWindowCharset = new System.Windows.Forms.Label();
            this.txtWindowCharset = new System.Windows.Forms.TextBox();
            this.lblFinderTool = new System.Windows.Forms.Label();
            this.txtWindowClass = new System.Windows.Forms.TextBox();
            this.lblWindowClass = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.gpbTrayMe.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpbTrayMe
            // 
            this.gpbTrayMe.Controls.Add(this.windowFinder);
            this.gpbTrayMe.Controls.Add(this.lblWindowHandle);
            this.gpbTrayMe.Controls.Add(this.txtWindowHandle);
            this.gpbTrayMe.Controls.Add(this.lblWindowText);
            this.gpbTrayMe.Controls.Add(this.txtWindowText);
            this.gpbTrayMe.Controls.Add(this.lblWindowCharset);
            this.gpbTrayMe.Controls.Add(this.txtWindowCharset);
            this.gpbTrayMe.Controls.Add(this.lblFinderTool);
            this.gpbTrayMe.Controls.Add(this.txtWindowClass);
            this.gpbTrayMe.Controls.Add(this.lblWindowClass);
            this.gpbTrayMe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gpbTrayMe.Location = new System.Drawing.Point(8, 8);
            this.gpbTrayMe.Name = "gpbTrayMe";
            this.gpbTrayMe.Size = new System.Drawing.Size(260, 184);
            this.gpbTrayMe.TabIndex = 0;
            this.gpbTrayMe.TabStop = false;
            // 
            // windowFinder
            // 
            this.windowFinder.Location = new System.Drawing.Point(108, 32);
            this.windowFinder.Name = "windowFinder";
            this.windowFinder.Size = new System.Drawing.Size(31, 28);
            this.windowFinder.TabIndex = 1;
            this.windowFinder.WindowHandleChanged += new System.EventHandler(this.windowFinder_WindowHandleChanged);
            // 
            // lblWindowHandle
            // 
            this.lblWindowHandle.Location = new System.Drawing.Point(16, 72);
            this.lblWindowHandle.Name = "lblWindowHandle";
            this.lblWindowHandle.Size = new System.Drawing.Size(48, 16);
            this.lblWindowHandle.TabIndex = 2;
            this.lblWindowHandle.Text = "H&andle:";
            // 
            // txtWindowHandle
            // 
            this.txtWindowHandle.Location = new System.Drawing.Point(68, 68);
            this.txtWindowHandle.MaxLength = 8;
            this.txtWindowHandle.Name = "txtWindowHandle";
            this.txtWindowHandle.Size = new System.Drawing.Size(72, 20);
            this.txtWindowHandle.TabIndex = 3;
            this.txtWindowHandle.TextChanged += new System.EventHandler(this.txtWindowHandle_TextChanged);
            // 
            // lblWindowText
            // 
            this.lblWindowText.Location = new System.Drawing.Point(16, 120);
            this.lblWindowText.Name = "lblWindowText";
            this.lblWindowText.Size = new System.Drawing.Size(48, 16);
            this.lblWindowText.TabIndex = 6;
            this.lblWindowText.Text = "&Text:";
            // 
            // txtWindowText
            // 
            this.txtWindowText.Location = new System.Drawing.Point(68, 116);
            this.txtWindowText.Name = "txtWindowText";
            this.txtWindowText.ReadOnly = true;
            this.txtWindowText.Size = new System.Drawing.Size(176, 20);
            this.txtWindowText.TabIndex = 7;
            // 
            // lblWindowCharset
            // 
            this.lblWindowCharset.Location = new System.Drawing.Point(16, 144);
            this.lblWindowCharset.Name = "lblWindowCharset";
            this.lblWindowCharset.Size = new System.Drawing.Size(48, 16);
            this.lblWindowCharset.TabIndex = 8;
            this.lblWindowCharset.Text = "C&harset:";
            // 
            // txtWindowCharset
            // 
            this.txtWindowCharset.Location = new System.Drawing.Point(68, 140);
            this.txtWindowCharset.Name = "txtWindowCharset";
            this.txtWindowCharset.ReadOnly = true;
            this.txtWindowCharset.Size = new System.Drawing.Size(176, 20);
            this.txtWindowCharset.TabIndex = 9;
            // 
            // lblFinderTool
            // 
            this.lblFinderTool.Location = new System.Drawing.Point(16, 36);
            this.lblFinderTool.Name = "lblFinderTool";
            this.lblFinderTool.Size = new System.Drawing.Size(84, 16);
            this.lblFinderTool.TabIndex = 0;
            this.lblFinderTool.Text = "Finder Tool:";
            // 
            // txtWindowClass
            // 
            this.txtWindowClass.Location = new System.Drawing.Point(68, 92);
            this.txtWindowClass.Name = "txtWindowClass";
            this.txtWindowClass.ReadOnly = true;
            this.txtWindowClass.Size = new System.Drawing.Size(176, 20);
            this.txtWindowClass.TabIndex = 5;
            // 
            // lblWindowClass
            // 
            this.lblWindowClass.Location = new System.Drawing.Point(16, 96);
            this.lblWindowClass.Name = "lblWindowClass";
            this.lblWindowClass.Size = new System.Drawing.Size(48, 16);
            this.lblWindowClass.TabIndex = 4;
            this.lblWindowClass.Text = "&Class";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(280, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(92, 32);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Cl&ose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(380, 202);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gpbTrayMe);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test WindowFinder Control";
            this.gpbTrayMe.ResumeLayout(false);
            this.gpbTrayMe.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.GroupBox gpbTrayMe;
        private System.Windows.Forms.Label lblFinderTool;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblWindowHandle;
        private System.Windows.Forms.TextBox txtWindowHandle;
        private System.Windows.Forms.Label lblWindowText;
        private System.Windows.Forms.TextBox txtWindowText;
        private System.Windows.Forms.Label lblWindowCharset;
        private System.Windows.Forms.TextBox txtWindowCharset;
        private System.Windows.Forms.TextBox txtWindowClass;
        private System.Windows.Forms.Label lblWindowClass;
        private WindowFinder.WindowFinder windowFinder;
    }
}
