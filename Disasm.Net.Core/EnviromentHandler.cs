using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Disasm.Net.Core;

public static class EnviromentHandler
{
	public static string ClientFiles { get; } = GetClientPath();

#pragma warning disable IDE0046
	public static string OsIdentifier
	{
		get
		{
			if (OperatingSystem.IsWindows())
				return "win";
			if (OperatingSystem.IsLinux())
				return "linux";
			if (OperatingSystem.IsMacOS())
				return "osx";
			throw new PlatformNotSupportedException();
		}
	}

	public static string ArchIdentifier
	{
		get
		{
			// ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
			return RuntimeInformation.ProcessArchitecture switch
			{
				Architecture.X64 => "x64",
				Architecture.Arm64 => "arm64",
				_ => throw new PlatformNotSupportedException()
			};
		}
	}

	public static string GetPlatformExecutableName(string executable)
	{
		if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
			return executable;
		return OperatingSystem.IsWindows() ? executable + ".exe" : throw new PlatformNotSupportedException();
	}
#pragma warning restore IDE0046

	private static string GetClientPath()
	{
		string? files = Environment.GetEnvironmentVariable("DISASM_CLIENT_FILES");
		if (string.IsNullOrEmpty(files))
			files = Path.GetRelativePath(Directory.GetCurrentDirectory(),
				$"../../../../Disasm.Net.Client/bin/release/net8.0/{OsIdentifier}-{ArchIdentifier}/publish");
		return Path.GetFullPath(files);
	}
}
