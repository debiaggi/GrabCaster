echo off
cd..
copy DefaultFiles\DevDefault.cfg Framework\bin\Debug\GrabCaster.cfg /y
copy DefaultFiles\DevDefault.cfg Framework\bin\Release\GrabCaster.cfg /y
cd %~dp0