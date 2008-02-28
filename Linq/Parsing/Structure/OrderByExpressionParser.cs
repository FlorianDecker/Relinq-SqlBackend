using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Rubicon.Data.Linq.Clauses;
using Rubicon.Data.Linq.Parsing.Structure;
using Rubicon.Utilities;

namespace Rubicon.Data.Linq.Parsing.Structure
{
  public class OrderByExpressionParser
  {
    private readonly ParseResultCollector _resultCollector;
    private readonly bool _isTopLevel;

    public OrderByExpressionParser (ParseResultCollector resultCollector, MethodCallExpression orderExpression, bool isTopLevel)
    {
      ArgumentUtility.CheckNotNull ("orderExpression", orderExpression);
      ArgumentUtility.CheckNotNull ("resultCollector", resultCollector);

      _resultCollector = resultCollector;
      _isTopLevel = isTopLevel;

      if (orderExpression.Arguments.Count != 2)
        throw ParserUtility.CreateParserException ("OrderBy call with two arguments", orderExpression, "OrderBy expressions",
            _resultCollector.ExpressionTreeRoot);

      SourceExpression = orderExpression;

      switch (ParserUtility.CheckMethodCallExpression (orderExpression, _resultCollector.ExpressionTreeRoot,
          "OrderBy", "OrderByDescending", "ThenBy", "ThenByDescending"))
      {
        case "OrderBy":
          ParseOrderBy (_resultCollector.ExpressionTreeRoot, OrderDirection.Asc, true);
          break;
        case "ThenBy":
          ParseOrderBy (_resultCollector.ExpressionTreeRoot, OrderDirection.Asc, false);
          break;
        case "OrderByDescending":
          ParseOrderBy (_resultCollector.ExpressionTreeRoot, OrderDirection.Desc, true);
          break;
        case "ThenByDescending":
          ParseOrderBy (_resultCollector.ExpressionTreeRoot, OrderDirection.Desc, false);
          break;
      }
    }

    public MethodCallExpression SourceExpression { get; private set; }

    private void ParseOrderBy (Expression expressionTreeRoot, OrderDirection direction, bool orderBy)
    {
      UnaryExpression unaryExpression = ParserUtility.GetTypedExpression<UnaryExpression> (SourceExpression.Arguments[1],
          "second argument of OrderBy expression", expressionTreeRoot);
      LambdaExpression ueLambda = ParserUtility.GetTypedExpression<LambdaExpression> (unaryExpression.Operand,
          "second argument of OrderBy expression", expressionTreeRoot);

      new SourceExpressionParser (_resultCollector, SourceExpression.Arguments[0], false,
          ueLambda.Parameters[0], "first argument of OrderBy expression");
            
      _resultCollector.AddBodyExpression (new OrderExpression (orderBy, direction, ueLambda));
      if (_isTopLevel)
        _resultCollector.AddProjectionExpression (null);
    }
  }
}