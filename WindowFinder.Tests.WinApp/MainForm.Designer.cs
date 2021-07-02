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
            this.components = new System.ComponentModel.Container();
            this.gpbTrayMe = new System.Windows.Forms.GroupBox();
            this.txtWindowRect = new System.Windows.Forms.TextBox();
            this.lblWinRect = new System.Windows.Forms.Label();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radiobtnAimingFrame = new System.Windows.Forms.RadioButton();
            this.radiobtnInvertColor = new System.Windows.Forms.RadioButton();
            this.lblDpiAwareness = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.ckbScreenshot = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gpbTrayMe.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpbTrayMe
            // 
            this.gpbTrayMe.Controls.Add(this.txtWindowRect);
            this.gpbTrayMe.Controls.Add(this.lblWinRect);
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
            this.gpbTrayMe.Location = new System.Drawing.Point(8, 28);
            this.gpbTrayMe.Name = "gpbTrayMe";
            this.gpbTrayMe.Size = new System.Drawing.Size(260, 172);
            this.gpbTrayMe.TabIndex = 0;
            this.gpbTrayMe.TabStop = false;
            // 
            // txtWindowRect
            // 
            this.txtWindowRect.Location = new System.Drawing.Point(60, 144);
            this.txtWindowRect.Name = "txtWindowRect";
            this.txtWindowRect.ReadOnly = true;
            this.txtWindowRect.Size = new System.Drawing.Size(194, 20);
            this.txtWindowRect.TabIndex = 21;
            // 
            // lblWinRect
            // 
            this.lblWinRect.AutoSize = true;
            this.lblWinRect.Location = new System.Drawing.Point(8, 148);
            this.lblWinRect.Name = "lblWinRect";
            this.lblWinRect.Size = new System.Drawing.Size(52, 13);
            this.lblWinRect.TabIndex = 20;
            this.lblWinRect.Text = "WinRect:";
            // 
            // windowFinder
            // 
            this.windowFinder.isCaptureToClipboard = true;
            this.windowFinder.isFindOnlyTopLevel = false;
            this.windowFinder.Location = new System.Drawing.Point(100, 20);
            this.windowFinder.Name = "windowFinder";
            this.windowFinder.Size = new System.Drawing.Size(31, 28);
            this.windowFinder.TabIndex = 1;
            this.windowFinder.tgwHighlightMethod = WindowFinder.WindowFinder.HighlightMethod.AimingFrame;
            this.windowFinder.WindowHandleChanged += new System.EventHandler(this.windowFinder_WindowHandleChanged);
            // 
            // lblWindowHandle
            // 
            this.lblWindowHandle.Location = new System.Drawing.Point(8, 60);
            this.lblWindowHandle.Name = "lblWindowHandle";
            this.lblWindowHandle.Size = new System.Drawing.Size(48, 16);
            this.lblWindowHandle.TabIndex = 12;
            this.lblWindowHandle.Text = "H&andle:";
            // 
            // txtWindowHandle
            // 
            this.txtWindowHandle.Location = new System.Drawing.Point(60, 56);
            this.txtWindowHandle.MaxLength = 8;
            this.txtWindowHandle.Name = "txtWindowHandle";
            this.txtWindowHandle.Size = new System.Drawing.Size(72, 20);
            this.txtWindowHandle.TabIndex = 13;
            this.txtWindowHandle.TextChanged += new System.EventHandler(this.txtWindowHandle_TextChanged);
            // 
            // lblWindowText
            // 
            this.lblWindowText.Location = new System.Drawing.Point(8, 104);
            this.lblWindowText.Name = "lblWindowText";
            this.lblWindowText.Size = new System.Drawing.Size(48, 16);
            this.lblWindowText.TabIndex = 16;
            this.lblWindowText.Text = "&Text:";
            // 
            // txtWindowText
            // 
            this.txtWindowText.Location = new System.Drawing.Point(60, 100);
            this.txtWindowText.Name = "txtWindowText";
            this.txtWindowText.ReadOnly = true;
            this.txtWindowText.Size = new System.Drawing.Size(194, 20);
            this.txtWindowText.TabIndex = 17;
            // 
            // lblWindowCharset
            // 
            this.lblWindowCharset.Location = new System.Drawing.Point(8, 126);
            this.lblWindowCharset.Name = "lblWindowCharset";
            this.lblWindowCharset.Size = new System.Drawing.Size(48, 16);
            this.lblWindowCharset.TabIndex = 18;
            this.lblWindowCharset.Text = "C&harset:";
            // 
            // txtWindowCharset
            // 
            this.txtWindowCharset.Location = new System.Drawing.Point(60, 122);
            this.txtWindowCharset.Name = "txtWindowCharset";
            this.txtWindowCharset.ReadOnly = true;
            this.txtWindowCharset.Size = new System.Drawing.Size(194, 20);
            this.txtWindowCharset.TabIndex = 19;
            // 
            // lblFinderTool
            // 
            this.lblFinderTool.Location = new System.Drawing.Point(8, 24);
            this.lblFinderTool.Name = "lblFinderTool";
            this.lblFinderTool.Size = new System.Drawing.Size(84, 16);
            this.lblFinderTool.TabIndex = 0;
            this.lblFinderTool.Text = "Finder Tool:";
            // 
            // txtWindowClass
            // 
            this.txtWindowClass.Location = new System.Drawing.Point(60, 78);
            this.txtWindowClass.Name = "txtWindowClass";
            this.txtWindowClass.ReadOnly = true;
            this.txtWindowClass.Size = new System.Drawing.Size(194, 20);
            this.txtWindowClass.TabIndex = 15;
            // 
            // lblWindowClass
            // 
            this.lblWindowClass.Location = new System.Drawing.Point(8, 82);
            this.lblWindowClass.Name = "lblWindowClass";
            this.lblWindowClass.Size = new System.Drawing.Size(48, 16);
            this.lblWindowClass.TabIndex = 14;
            this.lblWindowClass.Text = "&Class";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(280, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(92, 32);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Cl&ose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radiobtnAimingFrame);
            this.groupBox1.Controls.Add(this.radiobtnInvertColor);
            this.groupBox1.Location = new System.Drawing.Point(280, 101);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(92, 70);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Highlight by";
            // 
            // radiobtnAimingFrame
            // 
            this.radiobtnAimingFrame.AutoSize = true;
            this.radiobtnAimingFrame.Location = new System.Drawing.Point(5, 41);
            this.radiobtnAimingFrame.Name = "radiobtnAimingFrame";
            this.radiobtnAimingFrame.Size = new System.Drawing.Size(85, 17);
            this.radiobtnAimingFrame.TabIndex = 9;
            this.radiobtnAimingFrame.Text = "&Aiming frame";
            this.radiobtnAimingFrame.UseVisualStyleBackColor = true;
            // 
            // radiobtnInvertColor
            // 
            this.radiobtnInvertColor.AutoSize = true;
            this.radiobtnInvertColor.Location = new System.Drawing.Point(5, 22);
            this.radiobtnInvertColor.Name = "radiobtnInvertColor";
            this.radiobtnInvertColor.Size = new System.Drawing.Size(78, 17);
            this.radiobtnInvertColor.TabIndex = 8;
            this.radiobtnInvertColor.TabStop = true;
            this.radiobtnInvertColor.Text = "&Invert color";
            this.radiobtnInvertColor.UseVisualStyleBackColor = true;
            this.radiobtnInvertColor.CheckedChanged += new System.EventHandler(this.radiobtnInvertColor_CheckedChanged);
            // 
            // lblDpiAwareness
            // 
            this.lblDpiAwareness.AutoSize = true;
            this.lblDpiAwareness.Location = new System.Drawing.Point(12, 12);
            this.lblDpiAwareness.Name = "lblDpiAwareness";
            this.lblDpiAwareness.Size = new System.Drawing.Size(85, 13);
            this.lblDpiAwareness.TabIndex = 10;
            this.lblDpiAwareness.Text = "?DPI awareness";
            // 
            // btnHelp
            // 
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHelp.Location = new System.Drawing.Point(248, 12);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(20, 22);
            this.btnHelp.TabIndex = 11;
            this.btnHelp.Text = "?";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // ckbScreenshot
            // 
            this.ckbScreenshot.AutoSize = true;
            this.ckbScreenshot.Location = new System.Drawing.Point(285, 177);
            this.ckbScreenshot.Name = "ckbScreenshot";
            this.ckbScreenshot.Size = new System.Drawing.Size(80, 17);
            this.ckbScreenshot.TabIndex = 4;
            this.ckbScreenshot.Text = "&Screenshot";
            this.toolTip1.SetToolTip(this.ckbScreenshot, "Send to clipboard the screenshot from target window location");
            this.ckbScreenshot.UseVisualStyleBackColor = true;
            this.ckbScreenshot.CheckedChanged += new System.EventHandler(this.ckbClipboard_CheckedChanged);
            this.ckbScreenshot.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ckbClipboard_MouseClick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(380, 207);
            this.Controls.Add(this.ckbScreenshot);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.lblDpiAwareness);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gpbTrayMe);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test WindowFinder Control";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.gpbTrayMe.ResumeLayout(false);
            this.gpbTrayMe.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radiobtnAimingFrame;
        private System.Windows.Forms.RadioButton radiobtnInvertColor;
        private System.Windows.Forms.Label lblDpiAwareness;
        private System.Windows.Forms.TextBox txtWindowRect;
        private System.Windows.Forms.Label lblWinRect;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.CheckBox ckbScreenshot;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.IContainer components;
    }
}
