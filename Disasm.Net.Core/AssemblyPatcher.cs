using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Disasm.Net.Core;

public static class AssemblyPatcher
{
	public static void Patch(string path, string references)
	{
		using DefaultAssemblyResolver resolver = new();

		resolver.AddSearchDirectory(references);

		using AssemblyDefinition assembly = ReadAssembly(path, true, resolver, out bool symbolsAvailable);

		foreach (ModuleDefinition module in assembly.Modules) 
			module.EntryPoint = null;

		assembly.Write(new WriterParameters { WriteSymbols = symbolsAvailable });
	}
	
	private static AssemblyDefinition ReadAssembly(string path, bool readWrite, IAssemblyResolver resolver, out bool symbolsAvailable)
	{
		try
		{
			symbolsAvailable = true;
			return AssemblyDefinition.ReadAssembly(path, new ReaderParameters
				{ ReadSymbols = true, ReadWrite = readWrite, AssemblyResolver = resolver });
		}
		catch (SymbolsNotFoundException)
		{
			symbolsAvailable = false;
			return AssemblyDefinition.ReadAssembly(path, new ReaderParameters
				{ ReadSymbols = false, ReadWrite = readWrite, AssemblyResolver = resolver });
		}
	}
}
