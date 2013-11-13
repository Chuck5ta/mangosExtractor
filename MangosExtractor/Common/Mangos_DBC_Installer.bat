@echo off
:quick
rem Quick install section
rem This will automatically use the variables below to install the world and scriptdev2 databases without prompting then optimize them and exit
rem To use: Set your environment variables below and change 'set quick=off' to 'set quick=on' 
set quick=off
if %quick% == off goto standard
echo (( DBC Quick Installer ))
rem -- Change the values below to match your server --
set svr=localhost
set user=root
set pass=
set port=3306
set wdb=dbc
rem -- Don't change past this point --
set yesno=y
goto install

:standard
rem Standard install section
color 3
echo .
echo 8888888b.  888888b.    .d8888b.  
echo 888  "Y88b 888  "88b  d88P  Y88b 
echo 888    888 888  .88P  Y88b       
echo 888    888 8888888K.  Y88b       
echo 888    888 888  "Y88b Y88b       
echo 888    888 888    888 Y88b       
echo 888  .d88P 888   d88P Y88b  d88P 
echo 8888888P"  8888888P"   "Y8888P"  
echo.
echo Credits to: Factionwars, Nemok, BrainDedd and Antz
echo.
set /p svr=What is your MySQL host name?    [localhost]  : 
if %svr%. == . set svr=localhost
set /p user=What is your MySQL user name?   [root]       : 
if %user%. == . set user=root
set /p pass=What is your MySQL password?    [ ]          : 
if %pass%. == . set pass=
set /p port=What is your MySQL port?        [3306]       : 
if %port%. == . set port=3306
set /p wdb=What is your DBC database name?  [dbc]        : 
if %wdb%. == . set wdb=DBC

:install
set dbpath=dbfilesclient
set mysql=.

:checkpaths
if not exist %dbpath% then goto patherror
if not exist %mysql%\mysql.exe then goto patherror
goto world

:patherror
echo Cannot find required files, please ensure you have done a fully
echo recursive checkout from the SVN.
pause
goto :eof

:world
if %quick% == off echo.
if %quick% == off echo This will wipe out your current World database and replace it.
if %quick% == off set /p yesno=Do you wish to continue? (y/n) 
if %quick% == off if %yesno% neq y if %yesno% neq Y goto sd2

echo.
echo Importing World database

for %%i in (%dbpath%\*.sql) do if %%i neq %dbpath%\characters.sql if %%i neq %dbpath%\realmd.sql echo %%i & %mysql%\mysql -q -s -h %svr% --user=%user% --password=%pass% --port=%port% %wdb% < %%i

:done
echo.
echo Done :)
echo.
pause