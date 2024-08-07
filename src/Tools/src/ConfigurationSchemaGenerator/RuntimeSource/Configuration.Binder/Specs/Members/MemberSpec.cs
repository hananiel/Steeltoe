﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Steeltoe: This file was copied from the .NET Aspire Configuration Schema generator
// at https://github.com/dotnet/aspire/tree/cb7cc4d78f8dd2b4df1053a229493cdbf88f50df/src/Tools/ConfigurationSchemaGenerator.
#pragma warning disable

using System.Diagnostics;
using Microsoft.CodeAnalysis;
using SourceGenerators;

namespace Microsoft.Extensions.Configuration.Binder.SourceGeneration
{
    public abstract record MemberSpec
    {
        public MemberSpec(ISymbol member, TypeRef typeRef)
        {
            Debug.Assert(member is IPropertySymbol or IParameterSymbol);
            Name = member.Name;
            DefaultValueExpr = "default";
            TypeRef = typeRef;
        }

        public string Name { get; }
        public string DefaultValueExpr { get; protected set; }

        public TypeRef TypeRef { get; }
        public required string ConfigurationKeyName { get; init; }

        public abstract bool CanGet { get; }
        public abstract bool CanSet { get; }
    }
}
