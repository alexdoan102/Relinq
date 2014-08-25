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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Utilities;

namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  /// <summary>
  /// Represents a <see cref="MethodCallExpression"/> for <see cref="Queryable.Count{TSource}(System.Linq.IQueryable{TSource})"/>,
  /// <see cref="Queryable.Count{TSource}(System.Linq.IQueryable{TSource},System.Linq.Expressions.Expression{System.Func{TSource,bool}})"/>,
  /// for the Count properties of <see cref="List{T}"/>, <see cref="T:System.Collections.ArrayList"/>, <see cref="ICollection{T}"/>, 
  /// and <see cref="ICollection"/>, and for the <see cref="Array.Length"/> property of arrays.
  /// It is generated by <see cref="ExpressionTreeParser"/> when an <see cref="Expression"/> tree is parsed.
  /// When this node is used, it marks the beginning (i.e. the last node) of an <see cref="IExpressionNode"/> chain that represents a query.
  /// </summary>
  public class CountExpressionNode : ResultOperatorExpressionNodeBase
  {
    public static readonly MethodInfo[] SupportedMethods;

    static CountExpressionNode ()
    {
      var supportedMethods = new List<MethodInfo>
                             {
                                                               GetSupportedMethod (() => Queryable.Count<object> (null)),
                                                               GetSupportedMethod (() => Queryable.Count<object> (null, null)),
                                                               GetSupportedMethod (() => Enumerable.Count<object> (null)),
                                                               GetSupportedMethod (() => Enumerable.Count<object> (null, null)),
// ReSharper disable PossibleNullReferenceException
                                                               GetSupportedMethod (() => ((List<int>) null).Count),
                                                               GetSupportedMethod (() => ((ICollection<int>) null).Count),
                                                               GetSupportedMethod (() => ((ICollection) null).Count),
                                                               GetSupportedMethod (() => (((Array) null).Length)),
// ReSharper restore PossibleNullReferenceException
                         };

      var arrayListCountExpression = GetArrayListCountExpression();
      if (arrayListCountExpression != null)
        supportedMethods.Add (GetSupportedMethod (arrayListCountExpression));

      SupportedMethods = supportedMethods.ToArray();
    }

    private static Expression<Func<int>> GetArrayListCountExpression ()
    {
      var arrayListType = Type.GetType ("System.Collections.ArrayList", false);
      if (arrayListType == null)
        return null;

      var property = arrayListType.GetRuntimeProperty ("Count");
      Assertion.IsNotNull (property, "Property 'Count' was not found on type 'System.Collections.ArrayList'.");

      //() => ((ArrayList) null).Count;
      return Expression.Lambda<Func<int>>(Expression.MakeMemberAccess (Expression.Constant (null, arrayListType), property));
    }

    public CountExpressionNode (MethodCallExpressionParseInfo parseInfo, LambdaExpression optionalPredicate)
        : base (parseInfo, optionalPredicate, null)
    {
    }

    public override Expression Resolve (
        ParameterExpression inputParameter, Expression expressionToBeResolved, ClauseGenerationContext clauseGenerationContext)
    {
      // no data streams out from this node, so we cannot resolve any expressions
      throw CreateResolveNotSupportedException();
    }

    protected override ResultOperatorBase CreateResultOperator (ClauseGenerationContext clauseGenerationContext)
    {
      return new CountResultOperator();
    }
  }
}