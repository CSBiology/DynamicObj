@echo off
cls

set PYTHONIOENCODING=utf-8

dotnet run --project ./build/build.fsproj %*