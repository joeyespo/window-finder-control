using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace TestControl
{
	public class frmMain : System.Windows.Forms.Form
	{
    #region Class Variables
		
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
    /// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
    
    #endregion
		
    #region Class Construction
    
    public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}
		
    #region Windows Form Designer generated code
    
    /// <summary>
    /// Clean up any resources being used.
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
      this.gpbTrayMe.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                            this.windowFinder,
                                                                            this.lblWindowHandle,
                                                                            this.txtWindowHandle,
                                                                            this.lblWindowText,
                                                                            this.txtWindowText,
                                                                            this.lblWindowCharset,
                                                                            this.txtWindowCharset,
                                                                            this.lblFinderTool,
                                                                            this.txtWindowClass,
                                                                            this.lblWindowClass});
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
      this.txtWindowHandle.Text = "";
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
      this.txtWindowText.Text = "";
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
      this.txtWindowCharset.Text = "";
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
      this.txtWindowClass.Text = "";
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
      this.btnClose.Location = new System.Drawing.Point(280, 12);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(92, 32);
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "Cl&ose";
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // frmMain
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(380, 202);
      this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                  this.btnClose,
                                                                  this.gpbTrayMe});
      this.Name = "frmMain";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Test WindowFinder Control";
      this.gpbTrayMe.ResumeLayout(false);
      this.ResumeLayout(false);

    }
		
    #endregion
    
    #endregion
		
    /// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main () 
		{
			Application.Run(new frmMain());
		}
    
    private void btnClose_Click (object sender, System.EventArgs e)
    { this.Close(); }

    private void windowFinder_WindowHandleChanged (object sender, System.EventArgs e)
    {
      if (!handleEdit) txtWindowHandle.Text = windowFinder.WindowHandleText;
      txtWindowClass.Text = windowFinder.WindowClass;
      txtWindowText.Text = windowFinder.WindowText;
      txtWindowCharset.Text = windowFinder.WindowCharset;
    }
    
    bool handleEdit = false;
    private void txtWindowHandle_TextChanged (object sender, System.EventArgs e)
    {
      IntPtr handle;
      
      try
      { handle = (IntPtr)Convert.ToInt32(txtWindowHandle.Text, 16); }
      catch { handle = IntPtr.Zero; }
      
      handleEdit = true;
      try
      { windowFinder.SetWindowHandle(handle); }
      finally
      { handleEdit = false; }
    }
  }
}
