﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Rubicon.Data.Linq.Parsing.Structure;

namespace Rubicon.Data.Linq.UnitTests.ParsingTest.StructureTest.WhereExpressionParserTest
{
  public class BodyHelper
  {
    private readonly IEnumerable<BodyExpressionDataBase> _bodyExpressions;

    public BodyHelper (IEnumerable<BodyExpressionDataBase> bodyExpressions)
    {
      _bodyExpressions = bodyExpressions;
    }

    public List<Expression> FromExpressions
    {
      get
      {
        List<Expression> fromExpressions = new List<Expression>();
        foreach (BodyExpressionDataBase expression in _bodyExpressions)
        {
          FromExpressionData fromExpressionData = expression as FromExpressionData;
          if (fromExpressionData != null)
            fromExpressions.Add (fromExpressionData.Expression);
        }
        return fromExpressions;
      }
    }

    public List<ParameterExpression> FromIdentifiers
    {
      get
      {
        List<ParameterExpression> fromIdentifiers = new List<ParameterExpression>();
        foreach (BodyExpressionDataBase expression in _bodyExpressions)
        {
          FromExpressionData fromExpressionData = expression as FromExpressionData;
          if (fromExpressionData != null)
            fromIdentifiers.Add (fromExpressionData.Identifier);
        }
        return fromIdentifiers;
      }
    }

    public List<LambdaExpression> WhereExpressions
    {
      get
      {
        List<LambdaExpression> fromExpressions = new List<LambdaExpression>();
        foreach (BodyExpressionDataBase expression in _bodyExpressions)
        {
          WhereExpressionData whereExpressionData = expression as WhereExpressionData;
          if (whereExpressionData != null)
            fromExpressions.Add (whereExpressionData.Expression);
        }
        return fromExpressions;
      }
    }

    public List<OrderExpressionData> OrderingExpressions
    {
      get
      {
        List<OrderExpressionData> orderbyExpressions = new List<OrderExpressionData> ();
        foreach (BodyExpressionDataBase expression in _bodyExpressions)
        {
          OrderExpressionData orderExpressionData = expression as OrderExpressionData;
          if (orderExpressionData != null)
            orderbyExpressions.Add (orderExpressionData);
        }
        return orderbyExpressions;
      }
    }

    


  }
}