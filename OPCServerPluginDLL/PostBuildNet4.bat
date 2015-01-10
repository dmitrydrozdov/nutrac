if %1[==[ goto default

if exist %2DANSrvNet4.exe copy %2DANSrvNet4.exe %2%1
if not exist %2DANSrvNet4.exe copy %2..\..\DANSrvNet4.exe %2%1
if exist %2DANSrvNet4.exe.config copy %2DANSrvNet4.exe.config %2%1
if exist %2DANSrv.Items.xml copy %2DANSrv.Items.xml %2%1
if exist %2RegServer.exe copy %2RegServer.exe %2%1
if not exist %2RegServer.exe copy %2..\..\RegServer.exe %2%1
if exist %2UnregServer.exe copy %2UnregServer.exe %2%1
if not exist %2UnregServer.exe copy %2..\..\UnregServer.exe %2%1
%2%1RegServer.exe DANSrvNet4.exe
goto done

:default
if exist DANSrvNet4.exe copy DANSrvNet4.exe bin
if not exist DANSrvNet4.exe copy ..\..\DANSrvNet4.exe bin
if exist DANSrvNet4.exe.config copy DANSrvNet4.exe.config bin
if exist DANSrv.Items.xml copy DANSrv.Items.xml bin
if exist RegServer.exe copy RegServer.exe bin
if not exist RegServer.exe copy ..\..\RegServer.exe bin
if exist UnregServer.exe copy UnregServer.exe bin
if not exist UnregServer.exe copy ..\..\UnregServer.exe bin
bin\RegServer.exe DANSrvNet4.exe

:done