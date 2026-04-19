@echo off
setlocal
pushd %~dp0

echo Starting CereaBridge setup...
dotnet run --project CereaBridge.csproj -- --setup

popd
endlocal
