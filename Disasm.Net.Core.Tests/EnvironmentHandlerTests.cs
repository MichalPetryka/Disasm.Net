using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

using Xunit;
using Xunit.Abstractions;

namespace Disasm.Net.Core.Tests;

public class EnvironmentHandlerTests
{
	private readonly ITestOutputHelper _output;

	public EnvironmentHandlerTests(ITestOutputHelper output)
	{
		_output = output;
	}
	
	[Fact]
	public void CheckOsIdentifier()
	{
		string identifier = EnviromentHandler.OsIdentifier;
		if (OperatingSystem.IsWindows())
			Assert.Equal("win", identifier);
		else if (OperatingSystem.IsLinux())
			Assert.Equal("linux", identifier);
		else if (OperatingSystem.IsMacOS())
			Assert.Equal("osx", identifier);
		else
			throw new UnreachableException();
	}
	
	[Fact]
	public void CheckArchIdentifier()
	{
		string identifier = EnviromentHandler.ArchIdentifier;
		switch (RuntimeInformation.ProcessArchitecture)
		{
			case Architecture.X64:
				Assert.Equal("x64", identifier);
				break;
			case Architecture.Arm64:
				Assert.Equal("arm64", identifier);
				break;
			default:
				throw new UnreachableException();
		}
	}
	
	[Fact]
	public void CheckGetPlatformExecutableName()
	{
		const string executable = "Disasm.Net.Client";
		string name = EnviromentHandler.GetPlatformExecutableName(executable);
		_output.WriteLine(name);
		Assert.NotNull(name);
		Assert.NotEmpty(name);
		if (OperatingSystem.IsWindows())
			Assert.EndsWith(".exe", name, StringComparison.OrdinalIgnoreCase);
		else
			Assert.Equal(executable, name);
	}
	
	[Fact]
	public void CheckClientFiles()
	{
		string path = Path.Combine(EnviromentHandler.ClientFiles, EnviromentHandler.GetPlatformExecutableName("Disasm.Net.Client"));
		_output.WriteLine(path);
		Assert.True(File.Exists(path));
	}
}
