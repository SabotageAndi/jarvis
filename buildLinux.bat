del /F /Q linux\output\*
mkdir linux\output\client
mkdir linux\output\server
mkdir linux\output\worker
mkdir linux\output\eventhandler
mkdir linux\output\addins

xcopy /Y jarvis\Client\Worker\worker\bin\Debug\*.* linux\output\worker
del /F /Q linux\output\worker\*.config

xcopy /Y jarvis\Client\EventHandler\EventHandler\bin\Debug\*.* linux\output\eventhandler
del /F /Q linux\output\eventhandler\*.config

xcopy /Y jarvis\Client\Linux\linux\bin\Debug\*.* linux\output\client
del /F /Q linux\output\client\*.config

xcopy /Y jarvis\Server\server\bin\Debug\*.* linux\output\server
del /F /Q linux\output\server\*.config

xcopy /Y jarvis\Addins\compiledClient\* linux\output\addins

xcopy /Y linux\ninject\Ninject.* linux\output\worker
xcopy /Y linux\ninject\Ninject.* linux\output\eventhandler
xcopy /Y linux\ninject\Ninject.* linux\output\client
xcopy /Y linux\ninject\Ninject.* linux\output\server

xcopy /Y linux\npgsql\Npgsql2.0.11.93\Mono2.0\bin\Npgsql.* linux\output\server
rem xcopy /Y linux\npgsql\Npgsql2.0.11.93\Mono2.0\bin\Mono.Security.* linux\output\server

del /F /Q linux\output\server\Mono.Security.*

xcopy /Y linux\servicestack\* linux\output\worker
xcopy /Y linux\servicestack\* linux\output\eventhandler
xcopy /Y linux\servicestack\* linux\output\client
xcopy /Y linux\servicestack\* linux\output\server

del /F /Q linux\output\worker\*.txt
del /F /Q linux\output\eventhandler\*.txt
del /F /Q linux\output\client\*.txt
del /F /Q linux\output\server\*.txt