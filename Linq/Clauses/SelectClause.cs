using System;
using Rubicon.Data.Linq.Clauses;
using Rubicon.Data.Linq.DataObjectModel;
using Rubicon.Utilities;
using System.Linq.Expressions;

namespace Rubicon.Data.Linq.Clauses
{
  public class SelectClause : ISelectGroupClause
  {
    private readonly LambdaExpression _projectionExpression;

    public SelectClause (IClause previousClause, LambdaExpression projectionExpression)
    {
      ArgumentUtility.CheckNotNull ("previousClause", previousClause);

      PreviousClause = previousClause;
      _projectionExpression = projectionExpression;
    }

    public IClause PreviousClause { get; private set; }

    public LambdaExpression ProjectionExpression
    {
      get { return _projectionExpression; }
    }

    public virtual void Accept (IQueryVisitor visitor)
    {
      ArgumentUtility.CheckNotNull ("visitor", visitor);
      visitor.VisitSelectClause (this);
    }
  }
}