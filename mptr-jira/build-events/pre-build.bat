:: taskkill always seems to return a non-zero exit code which the pre-build event must not do, 
:: so we run it in this batch file instead and always return exit code 0.
taskkill /IM PowerToys.exe 2>nul 1>nul
exit 0