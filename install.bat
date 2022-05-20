@echo off
set "powertoys_path=C:\Program Files\PowerToys"
set /p powertoys_path= "PowerToys install path [%powertoys_path%]: "
mklink /D "%powertoys_path%\modules\launcher\Plugins\mptr-bookmark" "%cd%\mptr-bookmark\bin\Debug\net6.0-windows"
echo Restarting PowerToys...
taskkill /IM PowerToys.exe 2>nul 1>nul
powershell start-process 'C:\Program Files\PowerToys\PowerToys.exe'
echo Done.