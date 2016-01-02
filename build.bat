@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=1.0.0
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

set nuget=
if "%nuget%" == "" (
	set nuget=nuget
)

%NuGet% restore ProgParty.Core.sln
%NuGet% restore "ProgParty.Core/packages.config"
%MsBuildExe% ProgParty.Core.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false

mkdir Build
mkdir Build\lib
mkdir Build\lib\net40

%nuget% pack "ProgParty.Core.nuspec" -NoPackageAnalysis -OutputDirectory -verbosity detailed -o Build -Version %version% -p Configuration="%config%"
