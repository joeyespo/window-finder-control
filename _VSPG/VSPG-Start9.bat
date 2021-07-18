@echo off
REM Usage: This .bat is to be called from Visual Studio project Pre-build-commands and/or Post-build-commands,
REM so that we can write complex batch  programs from dedicated .bat files, instead of tucking them in 
REM those crowded .vcxproj or .csproj .
REM
REM Just use the following sample:
REM
REM $(ProjectDir)_VSPG\VSPG-Start9.bat $(ProjectDir)_VSPG\VSPG-PostBuild7.bat $(ProjectDir)Program.cs $(SolutionDir) $(ProjectDir) $(Configuration) $(PlatformName) $(TargetDir) $(TargetFileName) $(TargetName)
REM
REM Two things to tune:
REM [1] 1st parameter, 
REM     for Pre-build event, use
REM         $(ProjectDir)_VSPG\VSPG-PreBuild7.bat
REM     for Post-build event, use
REM         $(ProjectDir)_VSPG\VSPG-PostBuild7.bat
REM [2] 2nd parameter,
REM     You have to assign an existing "feedback" source-file(Program.cs for C#, or main.cpp for C++).
REM     On this .bat file execution failure, this .bat will touch that feedback file so that the failure is not
REM     slipped away. I mean, if previous Build fails and you execute Build again from Visual Studio, 
REM     the Build action will really take effect, instead of reporting a bogus up-to-date status.
REM
REM set batfilenam to .bat filename(no directory prefix)
set batfilenam=%~n0%~x0
REM  
set SubworkBat=%1
shift
set FeedbackFile=%1
shift
call :EchoVar SubworkBat
call :EchoVar FeedbackFile

if not exist %FeedbackFile% (
	call :Echos [ERROR] Not-existing feedback file: %FeedbackFile%
	exit /b 4
)

set ALL_PARAMS=%1 %2 %3 %4 %5 %6 %7
if exist %SubworkBat% (
  cmd /c %SubworkBat% %ALL_PARAMS%
) else (
  call :Echos [ERROR] SubworkBat NOT found: %SubworkBat%
  call :SetErrorlevel 4
)
if errorlevel 1 ( call :Touch %FeedbackFile% && exit /b 4 )

exit /b 0


REM =============================
REM ====== Functions Below ======
REM =============================

:Echos
  echo [%batfilenam%] %*
exit /b

:EchoExec
  echo [%batfilenam%] EXEC: %*
exit /b

:EchoVar
  REM Env-var double expansion trick from: https://stackoverflow.com/a/1202562/151453
  set _Varname=%1
  for /F %%i in ('echo %%%_Varname%%%') do echo [%batfilenam%] %_Varname% = %%i
exit /b

:SetErrorlevel
  REM Usage example:
  REM call :SetErrorlevel 4
exit /b %1

:Touch
	REM Touch updates a file's modification time to current.
	REM NOTE: No way to check for success/fail here. So, call it only when 
	REM you have decided to fail the whole bat.
	
	copy /b "%~1"+,, "%~1" >NUL 2>&1
exit /b

goto :END


:END
rem echo [%batfilenam%] END for %ProjectDir%
