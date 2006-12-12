using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

internal class resfinder
{}

namespace WindowFinder
{
  [ToolboxBitmap(typeof(resfinder), "WindowFinder.WindowFinder.ico")]
  [Designer(typeof(WindowFinderDesigner))]
  public class WindowFinder : System.Windows.Forms.UserControl
	{
    bool bTargeting = false;
    Cursor cursorTarget = null;
    Bitmap bitmapFind = null;
    Bitmap bitmapFinda = null;
    IntPtr hCurrentTarget = IntPtr.Zero;
    IntPtr windowHandle = IntPtr.Zero;
    string windowHandleText = "";
    string windowClass = "";
    string windowText = "";
    bool isWindowUnicode = false;
    string isWindowUnicodeText = "";
    private System.Windows.Forms.PictureBox picTarget;
    
    #region Class Variables

    private System.ComponentModel.IContainer components;
    
    #endregion
		
    #region Class Construction
    
    public WindowFinder ()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
      picTarget.Size = new Size(31, 28);
      this.SetStyle(ControlStyles.FixedWidth, true);
      this.SetStyle(ControlStyles.FixedHeight, true);
      this.SetStyle(ControlStyles.StandardClick, false);
      this.SetStyle(ControlStyles.StandardDoubleClick, false);
      this.Size = picTarget.Size;
    }
		
    #region Component Designer generated code
		
    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        if( components != null )
          components.Dispose();
      }
      base.Dispose( disposing );
    }
		
    /// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      this.picTarget = new System.Windows.Forms.PictureBox();
      this.SuspendLayout();
      // 
      // picTarget
      // 
      this.picTarget.BackColor = System.Drawing.Color.White;
      this.picTarget.Name = "picTarget";
      this.picTarget.Size = new System.Drawing.Size(92, 104);
      this.picTarget.TabIndex = 0;
      this.picTarget.TabStop = false;
      this.picTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picTarget_MouseUp);
      this.picTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picTarget_MouseMove);
      this.picTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picTarget_MouseDown);
      // 
      // WindowFinder
      // 
      this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                  this.picTarget});
      this.Name = "WindowFinder";
      this.Size = new System.Drawing.Size(272, 216);
      this.Load += new System.EventHandler(this.WindowFinder_Load);
      this.ResumeLayout(false);

    }
		
    #endregion
    
    #endregion
    
    public IntPtr WindowHandle
    { get { return windowHandle; } }
    
    public string WindowHandleText
    { get { return windowHandleText; } }
    
    public string WindowClass
    { get { return windowClass; } }
    
    public string WindowText
    { get { return windowText; } }
    
    public bool IsWindowUnicode
    { get { return isWindowUnicode; } }
    
    public string IsWindowUnicodeText
    { get { return isWindowUnicodeText; } }
    
    public void SetWindowHandle (IntPtr handle)
    {
      if ( (Win32.IsWindow(handle) == 0) || (Win32Ex.IsRelativeWindow(handle, this.Handle, true)) ) 
      {
        windowHandle = IntPtr.Zero;
        windowHandleText = "";
        windowClass = "";
        windowText = "";
        isWindowUnicode = false;
        isWindowUnicodeText = "";
        return;
      }
      
      // Set window information
      windowHandle = handle;
      windowHandleText = Convert.ToString(handle.ToInt32(), 16).ToUpper().PadLeft(8, '0');
      windowClass = Win32Ex.GetClassName(handle);
      windowText = Win32Ex.GetWindowText(handle);
      isWindowUnicode = Win32.IsWindowUnicode(handle) != 0;
      isWindowUnicodeText = (( isWindowUnicode )?( "Unicode" ):( "Ansi" ));
    }
    
    private void WindowFinder_Load (object sender, System.EventArgs e)
    {
      this.Size = picTarget.Size;
      try
      {
        // Load cursors
        Assembly assembly = Assembly.GetExecutingAssembly();
        cursorTarget = new Cursor(assembly.GetManifestResourceStream("WindowFinder.curTarget.cur"));
        bitmapFind = new Bitmap(assembly.GetManifestResourceStream("WindowFinder.bmpFind.bmp"));
        bitmapFinda = new Bitmap(assembly.GetManifestResourceStream("WindowFinder.bmpFinda.bmp"));
      }
      catch (Exception x )
      {
        // Show error
        MessageBox.Show(this, "Failed to load resources.\n\n" + x.ToString(), "WindowFinder");
        
        // Attempt to use backup cursor
        if (cursorTarget == null) cursorTarget = Cursors.Cross;
      }
      
      
      // Set default values
      picTarget.Image = bitmapFind;
    }
    
    private void picTarget_MouseDown (object sender, System.Windows.Forms.MouseEventArgs e)
    {
      // Set capture image and cursor
      picTarget.Image = bitmapFinda;
      picTarget.Cursor = cursorTarget;
      
      // Set capture
      Win32.SetCapture(picTarget.Handle);
      
      // Begin targeting
      bTargeting = true;
      hCurrentTarget = IntPtr.Zero;
      
      // Show info   TODO: Put into function for mousemove & mousedown
      SetWindowHandle(picTarget.Handle);
    }
    private void picTarget_MouseMove (object sender, System.Windows.Forms.MouseEventArgs e)
    {
      IntPtr hWnd;
      IntPtr hTemp;
      Win32.POINTAPI pt;
      
      // TODO: Draw border around EVERY window
      
      pt.x = e.X; pt.y = e.Y;
      Win32.ClientToScreen(picTarget.Handle, ref pt);
      
      // Make sure targeting before highlighting windows
      if (!bTargeting) return;
      
      // Get screen coords from client coords and window handle
      hWnd = Win32Ex.WindowFromPoint(picTarget.Handle, e.X, e.Y);
      
      // Get real window
      if (hWnd != IntPtr.Zero) 
      {
        hTemp = IntPtr.Zero;
        
        while (true)
        {
          Win32.MapWindowPoints(hTemp, hWnd, ref pt, 1);
          hTemp = (IntPtr)Win32.ChildWindowFromPoint(hWnd, pt.x, pt.y);
          if (hTemp == IntPtr.Zero) break;
          if (hWnd == hTemp) break;
          hWnd = hTemp;
        }
        
        /* TODO: Work with ALL windows
        Win32.ScreenToClient(hWnd, ref pt);
        Win32.MapWindowPoints(IntPtr.Zero, hWnd, ref pt, 2);
        if ((hTemp = (IntPtr)Win32.ChildWindowFromPoint(hWnd, pt.x, pt.y)) != IntPtr.Zero) 
        {
          hWnd = hTemp;
        }
        // */
      }
      
      // Get owner
      while ((hTemp = Win32.GetParent(hWnd)) != IntPtr.Zero) hWnd = hTemp;
      
      // Show info
      SetWindowHandle(hWnd);
      
      // Highlight valid window
      highlightValidWindow(hWnd, this.Handle);
    }
    private void picTarget_MouseUp (object sender, System.Windows.Forms.MouseEventArgs e)
    {
      IntPtr hWnd;
      IntPtr hTemp;
      
      // End targeting
      bTargeting = false;
      
      // Unhighlight window
      if (hCurrentTarget != IntPtr.Zero) Win32Ex.HighlightWindow(hCurrentTarget);
      hCurrentTarget = IntPtr.Zero;
      
      // Reset capture image and cursor
      picTarget.Cursor = Cursors.Default;
      picTarget.Image = bitmapFind;
      
      // Get screen coords from client coords and window handle
      hWnd = Win32Ex.WindowFromPoint(picTarget.Handle, e.X, e.Y);
      
      // Get owner
      while ((hTemp = Win32.GetParent(hWnd)) != IntPtr.Zero) hWnd = hTemp;
      
      SetWindowHandle(hWnd);
      
      // Release capture
      Win32.SetCapture(IntPtr.Zero);
    }
    
    void highlightValidWindow (IntPtr hWnd, IntPtr hOwner)
    {
      // Check for valid highlight
      if (hCurrentTarget == hWnd) return;
      
      // Check for relative
      if (Win32Ex.IsRelativeWindow(hWnd, hOwner, true))
      {
        // Unhighlight last window
        if (hCurrentTarget != IntPtr.Zero)
        { Win32Ex.HighlightWindow(hCurrentTarget); hCurrentTarget = IntPtr.Zero; }
        
        return;
      }
      
      // Unhighlight last window
      Win32Ex.HighlightWindow(hCurrentTarget);
      
      // Set as current target
      hCurrentTarget = hWnd;
      
      // Highlight window
      Win32Ex.HighlightWindow(hWnd);
    }
  }
}
