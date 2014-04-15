// Copyright (c) rubicon IT GmbH, www.rubicon.eu
//
// See the NOTICE file distributed with this work for additional information
// regarding copyright ownership.  rubicon licenses this file to you under 
// the Apache License, Version 2.0 (the "License"); you may not use this 
// file except in compliance with the License.  You may obtain a copy of the 
// License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT 
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the 
// License for the specific language governing permissions and limitations
// under the License.
// 

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace System
{
  // TODO RM-6132: See which methods can be replaced by existing functionality, e.g. ReflectionUtility.GetMethodInfo (...) or by passing parameter types.
  internal static class TypeExtensions
  {
    public static bool IsAssignableFrom (this Type type, Type c)
    {
      return c != null && type.GetTypeInfo().IsAssignableFrom (c.GetTypeInfo());
    }

    public static Type[] GetGenericArguments (this Type type)
    {
      var typeInfo = type.GetTypeInfo();

      return typeInfo.IsGenericTypeDefinition
          ? typeInfo.GenericTypeParameters
          : typeInfo.GenericTypeArguments;
    }
  }
}
