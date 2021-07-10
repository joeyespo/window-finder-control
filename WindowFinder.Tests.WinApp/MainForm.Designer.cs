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
            this.lblScreenMousePos = new System.Windows.Forms.Label();
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
            this.radiobtnSnapFrame = new System.Windows.Forms.RadioButton();
            this.radiobtnInvertColor = new System.Windows.Forms.RadioButton();
            this.lblDpiAwareness = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ckbMyProcess = new TestControl.MyCheckBox();
            this.ckbMyThread = new TestControl.MyCheckBox();
            this.ckbScreenshot = new TestControl.MyCheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gpbTrayMe.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpbTrayMe
            // 
            this.gpbTrayMe.Controls.Add(this.lblScreenMousePos);
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
            this.gpbTrayMe.Size = new System.Drawing.Size(260, 167);
            this.gpbTrayMe.TabIndex = 0;
            this.gpbTrayMe.TabStop = false;
            // 
            // lblScreenMousePos
            // 
            this.lblScreenMousePos.AutoSize = true;
            this.lblScreenMousePos.Location = new System.Drawing.Point(8, 33);
            this.lblScreenMousePos.Name = "lblScreenMousePos";
            this.lblScreenMousePos.Size = new System.Drawing.Size(63, 13);
            this.lblScreenMousePos.TabIndex = 22;
            this.lblScreenMousePos.Text = "?MousePos";
            // 
            // txtWindowRect
            // 
            this.txtWindowRect.Location = new System.Drawing.Point(60, 139);
            this.txtWindowRect.Name = "txtWindowRect";
            this.txtWindowRect.ReadOnly = true;
            this.txtWindowRect.Size = new System.Drawing.Size(194, 20);
            this.txtWindowRect.TabIndex = 21;
            // 
            // lblWinRect
            // 
            this.lblWinRect.AutoSize = true;
            this.lblWinRect.Location = new System.Drawing.Point(8, 143);
            this.lblWinRect.Name = "lblWinRect";
            this.lblWinRect.Size = new System.Drawing.Size(52, 13);
            this.lblWinRect.TabIndex = 20;
            this.lblWinRect.Text = "WinRect:";
            // 
            // windowFinder
            // 
            this.windowFinder.isDoScreenshot = true;
            this.windowFinder.isFindOnlyTopLevel = false;
            this.windowFinder.isIncludeMyProcess = true;
            this.windowFinder.isIncludeMyThread = true;
            this.windowFinder.Location = new System.Drawing.Point(105, 15);
            this.windowFinder.Name = "windowFinder";
            this.windowFinder.Size = new System.Drawing.Size(31, 28);
            this.windowFinder.TabIndex = 1;
            this.windowFinder.tgwHighlightMethod = WindowFinder.WindowFinder.HighlightMethod.SnapFrame;
            this.windowFinder.WindowHandleChanged += new System.EventHandler(this.windowFinder_WindowHandleChanged);
            this.windowFinder.MouseDraggingChanged += new System.EventHandler<WindowFinder.MouseDraggingEventArgs>(this.windowFinder_MouseDraggingChanged);
            // 
            // lblWindowHandle
            // 
            this.lblWindowHandle.Location = new System.Drawing.Point(8, 55);
            this.lblWindowHandle.Name = "lblWindowHandle";
            this.lblWindowHandle.Size = new System.Drawing.Size(48, 16);
            this.lblWindowHandle.TabIndex = 12;
            this.lblWindowHandle.Text = "H&andle:";
            // 
            // txtWindowHandle
            // 
            this.txtWindowHandle.Location = new System.Drawing.Point(60, 51);
            this.txtWindowHandle.MaxLength = 8;
            this.txtWindowHandle.Name = "txtWindowHandle";
            this.txtWindowHandle.Size = new System.Drawing.Size(76, 20);
            this.txtWindowHandle.TabIndex = 13;
            this.txtWindowHandle.TextChanged += new System.EventHandler(this.txtWindowHandle_TextChanged);
            // 
            // lblWindowText
            // 
            this.lblWindowText.Location = new System.Drawing.Point(8, 99);
            this.lblWindowText.Name = "lblWindowText";
            this.lblWindowText.Size = new System.Drawing.Size(48, 16);
            this.lblWindowText.TabIndex = 16;
            this.lblWindowText.Text = "&Text:";
            // 
            // txtWindowText
            // 
            this.txtWindowText.Location = new System.Drawing.Point(60, 95);
            this.txtWindowText.Name = "txtWindowText";
            this.txtWindowText.ReadOnly = true;
            this.txtWindowText.Size = new System.Drawing.Size(194, 20);
            this.txtWindowText.TabIndex = 17;
            // 
            // lblWindowCharset
            // 
            this.lblWindowCharset.Location = new System.Drawing.Point(8, 121);
            this.lblWindowCharset.Name = "lblWindowCharset";
            this.lblWindowCharset.Size = new System.Drawing.Size(48, 16);
            this.lblWindowCharset.TabIndex = 18;
            this.lblWindowCharset.Text = "C&harset:";
            // 
            // txtWindowCharset
            // 
            this.txtWindowCharset.Location = new System.Drawing.Point(60, 117);
            this.txtWindowCharset.Name = "txtWindowCharset";
            this.txtWindowCharset.ReadOnly = true;
            this.txtWindowCharset.Size = new System.Drawing.Size(194, 20);
            this.txtWindowCharset.TabIndex = 19;
            // 
            // lblFinderTool
            // 
            this.lblFinderTool.AutoSize = true;
            this.lblFinderTool.Location = new System.Drawing.Point(8, 14);
            this.lblFinderTool.Name = "lblFinderTool";
            this.lblFinderTool.Size = new System.Drawing.Size(63, 13);
            this.lblFinderTool.TabIndex = 0;
            this.lblFinderTool.Text = "Finder Tool:";
            // 
            // txtWindowClass
            // 
            this.txtWindowClass.Location = new System.Drawing.Point(60, 73);
            this.txtWindowClass.Name = "txtWindowClass";
            this.txtWindowClass.ReadOnly = true;
            this.txtWindowClass.Size = new System.Drawing.Size(194, 20);
            this.txtWindowClass.TabIndex = 15;
            // 
            // lblWindowClass
            // 
            this.lblWindowClass.Location = new System.Drawing.Point(8, 77);
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
            this.groupBox1.Controls.Add(this.radiobtnSnapFrame);
            this.groupBox1.Controls.Add(this.radiobtnInvertColor);
            this.groupBox1.Location = new System.Drawing.Point(280, 115);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(92, 54);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Highlight by";
            // 
            // radiobtnSnapFrame
            // 
            this.radiobtnSnapFrame.AutoSize = true;
            this.radiobtnSnapFrame.Location = new System.Drawing.Point(5, 34);
            this.radiobtnSnapFrame.Name = "radiobtnSnapFrame";
            this.radiobtnSnapFrame.Size = new System.Drawing.Size(79, 17);
            this.radiobtnSnapFrame.TabIndex = 9;
            this.radiobtnSnapFrame.Text = "Snap-frame";
            this.toolTip1.SetToolTip(this.radiobtnSnapFrame, "Overlay a (red) aiming-frame window on the target window, and keep the former sam" +
        "e-size as latter.\r\nThis works well regardless of DWM-rendering.");
            this.radiobtnSnapFrame.UseVisualStyleBackColor = true;
            // 
            // radiobtnInvertColor
            // 
            this.radiobtnInvertColor.AutoSize = true;
            this.radiobtnInvertColor.Location = new System.Drawing.Point(5, 15);
            this.radiobtnInvertColor.Name = "radiobtnInvertColor";
            this.radiobtnInvertColor.Size = new System.Drawing.Size(78, 17);
            this.radiobtnInvertColor.TabIndex = 8;
            this.radiobtnInvertColor.TabStop = true;
            this.radiobtnInvertColor.Text = "&Invert color";
            this.toolTip1.SetToolTip(this.radiobtnInvertColor, "WinXP era highlight method, drawing an invert-colored frame around target window." +
        " \r\nIt loses visual effect with DWM-rendered top-level window since Vista.");
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
            // ckbMyProcess
            // 
            this.ckbMyProcess.AutoSize = true;
            this.ckbMyProcess.Location = new System.Drawing.Point(6, 16);
            this.ckbMyProcess.Name = "ckbMyProcess";
            this.ckbMyProcess.Size = new System.Drawing.Size(80, 17);
            this.ckbMyProcess.TabIndex = 12;
            this.ckbMyProcess.Text = "My process";
            this.toolTip1.SetToolTip(this.ckbMyProcess, "Find windows/controls from the same process of this program.");
            this.ckbMyProcess.UseVisualStyleBackColor = true;
            this.ckbMyProcess.CheckedChanged_ByHuman += new System.EventHandler(this.ckbMyProcess_CheckedChanged_ByHuman);
            // 
            // ckbMyThread
            // 
            this.ckbMyThread.AutoSize = true;
            this.ckbMyThread.Location = new System.Drawing.Point(6, 34);
            this.ckbMyThread.Name = "ckbMyThread";
            this.ckbMyThread.Size = new System.Drawing.Size(73, 17);
            this.ckbMyThread.TabIndex = 13;
            this.ckbMyThread.Text = "My thread";
            this.toolTip1.SetToolTip(this.ckbMyThread, "Find windows/controls from the same thread by the calling code.");
            this.ckbMyThread.UseVisualStyleBackColor = true;
            this.ckbMyThread.CheckedChanged_ByHuman += new System.EventHandler(this.ckbMyThread_CheckedChanged_ByHuman);
            // 
            // ckbScreenshot
            // 
            this.ckbScreenshot.AutoSize = true;
            this.ckbScreenshot.Location = new System.Drawing.Point(285, 175);
            this.ckbScreenshot.Name = "ckbScreenshot";
            this.ckbScreenshot.Size = new System.Drawing.Size(80, 17);
            this.ckbScreenshot.TabIndex = 4;
            this.ckbScreenshot.Text = "&Screenshot";
            this.toolTip1.SetToolTip(this.ckbScreenshot, "Send to clipboard the screenshot from target window location");
            this.ckbScreenshot.UseVisualStyleBackColor = true;
            this.ckbScreenshot.CheckedChanged_ByHuman += new System.EventHandler(this.ckbScreenshot_CheckedChanged_ByHuman);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ckbMyProcess);
            this.groupBox2.Controls.Add(this.ckbMyThread);
            this.groupBox2.Location = new System.Drawing.Point(280, 55);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(92, 54);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Include";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(384, 202);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ckbScreenshot);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.lblDpiAwareness);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gpbTrayMe);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wcFinder TestApp";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.gpbTrayMe.ResumeLayout(false);
            this.gpbTrayMe.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.RadioButton radiobtnSnapFrame;
        private System.Windows.Forms.RadioButton radiobtnInvertColor;
        private System.Windows.Forms.Label lblDpiAwareness;
        private System.Windows.Forms.TextBox txtWindowRect;
        private System.Windows.Forms.Label lblWinRect;
        private System.Windows.Forms.Button btnHelp;
        private MyCheckBox ckbScreenshot;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.GroupBox groupBox2;
        private MyCheckBox ckbMyProcess;
        private MyCheckBox ckbMyThread;
        private System.Windows.Forms.Label lblScreenMousePos;
    }
}
