using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Disasm.Net.Client;

public static class Program
{
	public static int Main(string[] args)
	{
		if (args is not { Length: 1 } || string.IsNullOrWhiteSpace(args[0]))
		{
			Console.WriteLine("Assembly path required");
			return 1;
		}

		int errorCode = 0;

		foreach (Module module in Assembly.LoadFrom(args[0]).Modules)
		{
			foreach (Type type in module.GetTypes())
			{
				try
				{
					if (type.IsGenericTypeDefinition)
					{
						bool notFound = true;
						foreach (GenericParametersAttribute generics in type.GetCustomAttributes<GenericParametersAttribute>())
						{
							ProcessType(type.MakeGenericType(generics.Types));
							notFound = false;
						}
						if (notFound)
							Console.WriteLine($"Skipping generic type {type.FullName}");
					}
					else
					{
						ProcessType(type);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					errorCode = 2;
				}
			}
		}
		return errorCode;
	}

	private static void ProcessType(Type type)
	{
		foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
		{
			if (!method.IsGenericMethodDefinition)
				RuntimeHelpers.PrepareMethod(method.MethodHandle);
			else
			{
				bool notFound = true;
				foreach (GenericParametersAttribute generics in method.GetCustomAttributes<GenericParametersAttribute>())
				{
					RuntimeHelpers.PrepareMethod(method.MethodHandle, generics.Types.Select(t => t.TypeHandle).ToArray());
					notFound = false;
				}
				if (notFound)
					Console.WriteLine($"Skipping generic method {method}");
			}
		}
	}
}