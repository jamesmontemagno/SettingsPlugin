var TARGET = Argument ("target", Argument ("t", "Default"));
var VERSION = EnvironmentVariable ("APPVEYOR_BUILD_VERSION") ?? Argument("version", "0.0.9999");
var CONFIG = Argument("configuration", EnvironmentVariable ("CONFIGURATION") ?? "Release");
var SLN = "./src/Settings.sln";
var NUNIT_RESULT_PARSER = "tools/nunit-summary/nunit-summary.exe";

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
	.Does(()=>
{
	var outputPath =  MakeAbsolute(Directory("./tests/Plugin.Settings.NUnitTest.Android/bin/Debug"));
	MSBuild("./tests/Plugin.Settings.NUnitTest.Android/Plugin.Settings.NUnitTest.Android.csproj", 
		new MSBuildSettings()
			.WithProperty("MyBuildOutputPath", outputPath.ToString())
			.WithTarget("SignAndroidPackage")
			.WithTarget("RunUnitTests"));
	
	StartProcess(NUNIT_RESULT_PARSER, outputPath.CombineWithFilePath("Result_Tests-Debug.xml").ToString());
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
