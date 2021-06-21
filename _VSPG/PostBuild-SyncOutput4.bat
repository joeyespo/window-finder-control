@echo off
setlocal enabledelayedexpansion
set batfilenam=%~n0%~x0
REM
set BuildConf=%1
set PlatformName=%2
set ExeDllDir=%3
set TargetName=%4

REM =========================================================================
REM This bat copies(=sync) project output(EXE/DLL etc) to your desired target dirs.
REM 
REM After duplicating your own PostBuild-SyncOutput4.bat, you should 
REM customize COPY_TO_DIRS's value to be your copying target dirs.
REM You can list multiple target dirs, separated by spaces.
REM For example, assign two remote machine folders for remote debugging:
REM 
set COPY_TO_DIRS=
    REM z:\bin y:\bin2
REM
REM =========================================================================

if "%COPY_TO_DIRS%" == "" (
	call :Echos COPY_TO_DIRS is empty, nothing to copy.
)

for %%d in (%COPY_TO_DIRS%) do (
	set d_final=%%d\__%BuildConf%\%PlatformName%
	if not exist !d_final! ( 
		mkdir !d_final! 
	)
	set copy_cmd=copy %ExeDllDir%\*.* !d_final!
	call :EchoExec !copy_cmd!
	!copy_cmd!
	if errorlevel 1 ( 
		call :Echos BAD. Error occurred when copying file to some COPY_TO_DIRS.
		exit /b 4 
	)
)


goto :END

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

:END
rem echo [%batfilenam%] END for %ProjectDir%
