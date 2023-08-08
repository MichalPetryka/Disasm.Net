using System;

namespace Disasm.Net;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method, AllowMultiple = true)]
public sealed class GenericParametersAttribute : Attribute
{
	public Type[] Types { get; }

	public GenericParametersAttribute(params Type[] types) => Types = types;
}
