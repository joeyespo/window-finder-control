These BtnLook exe are sample programs run with various DPI-awareness. They will act as sample target-windows for wcFinder.

The original BtnLook code is from [Charles Petzold - *Programming Windows 5th-edition (1998)*](https://www.amazon.com/Programming-Windows-Developer-Reference-Charles-ebook/dp/B00JDMP71S/), chapter 9. I compile it to get EXE copies, and attach different manifest(dpiAware-SysDpi.manifest, dpiAware-PerMon.manifest) to them so to make them different DPI-awareness. MSDN: [Setting the default DPI awareness for a process](https://docs.microsoft.com/en-us/windows/win32/hidpi/setting-the-default-dpi-awareness-for-a-process).

![The "same" program with different DPI-awareness context on Win10.1909](3-dpi-awareness.png)
