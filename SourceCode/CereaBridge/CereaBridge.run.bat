@echo off
setlocal
pushd %~dp0

echo Starting CereaBridge...
dotnet run --project CereaBridge.csproj

popd
endlocal
