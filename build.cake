var TARGET = Argument ("target", Argument ("t", "Default"));
var VERSION = EnvironmentVariable ("APPVEYOR_BUILD_VERSION") ?? Argument("version", "0.0.9999");
var CONFIG = Argument("configuration", EnvironmentVariable ("CONFIGURATION") ?? "Release");
var SLN = "./src/Settings.sln";
var NUNIT_RESULT_PARSER = "./nunit-summary.exe";

Task("Libraries").Does(()=>
{
	NuGetRestore (SLN);
	MSBuild (SLN, c => {
		c.Configuration = CONFIG;
		c.MSBuildPlatform = Cake.Common.Tools.MSBuild.MSBuildPlatform.x86;
	});
});

Task ("NuGet")
	.IsDependentOn ("Libraries")
	.Does (() =>
{
    if(!DirectoryExists("./Build/nuget/"))
        CreateDirectory("./Build/nuget");
        
	NuGetPack ("./nuget/Plugin.nuspec", new NuGetPackSettings { 
		Version = VERSION,
		OutputDirectory = "./Build/nuget/",
		BasePath = "./"
	});	
});

//Build the component, which build samples, nugets, and libraries
Task ("Default").IsDependentOn("NuGet");

Task ("RunDroidTests")
	.IsDependentOn("DownloadUnitTestTools")
	.Does(()=>
{
	var outputPath =  MakeAbsolute(Directory("./tests/Plugin.Settings.NUnitTest.Android/bin/Debug"));
	MSBuild("./tests/Plugin.Settings.NUnitTest.Android/Plugin.Settings.NUnitTest.Android.csproj", 
		new MSBuildSettings()
			.WithProperty("MyBuildOutputPath", outputPath.ToString())
			.WithTarget("SignAndroidPackage")
			.WithTarget("RunUnitTests"));
	var exe = MakeAbsolute(File(NUNIT_RESULT_PARSER));
	StartProcess(exe.ToString(), outputPath.CombineWithFilePath("Result_Tests-Debug.xml").ToString());
});

Task("DownloadUnitTestTools")
	.Does(()=>{
	var exe = MakeAbsolute(File(NUNIT_RESULT_PARSER));
	if(!FileExists(exe.ToString()))
		DownloadFile("https://github.com/prashantvc/nunit-summary/releases/download/0.4/nunit-summary.exe","nunit-summary.exe");
	else
		Information("nunit-summary.exe exists");
	
});

Task ("Clean")
	.Does (() =>
{
	CleanDirectory ("./component/tools/");
	CleanDirectories ("./Build/");
	CleanDirectories ("./**/bin");
	CleanDirectories ("./**/obj");
});

RunTarget (TARGET);
