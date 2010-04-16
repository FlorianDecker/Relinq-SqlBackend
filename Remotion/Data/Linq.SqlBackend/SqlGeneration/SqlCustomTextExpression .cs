// This file is part of the re-motion Core Framework (www.re-motion.org)
// Copyright (C) 2005-2009 rubicon informationstechnologie gmbh, www.rubicon.eu
// 
// The re-motion Core Framework is free software; you can redistribute it 
// and/or modify it under the terms of the GNU Lesser General Public License 
// as published by the Free Software Foundation; either version 2.1 of the 
// License, or (at your option) any later version.
// 
// re-motion is distributed in the hope that it will be useful, 
// but WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the 
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with re-motion; if not, see http://www.gnu.org/licenses.
// 
using System;
using System.Linq.Expressions;
using Remotion.Data.Linq.Parsing;
using Remotion.Data.Linq.Utilities;

namespace Remotion.Data.Linq.SqlBackend.SqlGeneration
{
  /// <summary>
  /// <see cref="SqlCustomTextExpression"/> can be used to insert custom SQL text into the SQL generated by <see cref="SqlGeneratingExpressionVisitor"/>. 
  /// The custom text is inserted into the statement as is, it is not escaped. Therefore, the provider making use of <see cref="SqlCustomTextExpression"/> 
  /// has to make sure the custom text cannot lead to SQL injection attacks.
  /// </summary>
  // TODO Review 2564: Replace usages of this expression in the method call transformers with SqlLiteralExpressions where possible.
  public class SqlCustomTextExpression : SqlCustomTextGeneratorExpressionBase
  {
    private readonly string _sqlText;
    
    public SqlCustomTextExpression (string sqlText, Type expressionType) : base(expressionType)
    {
      ArgumentUtility.CheckNotNull ("sqlText", sqlText);

      _sqlText = sqlText;
    }

    public override void Generate (ISqlCommandBuilder commandBuilder, ExpressionTreeVisitor textGeneratingExpressionVisitor, ISqlGenerationStage stage)
    {
      ArgumentUtility.CheckNotNull ("commandBuilder", commandBuilder);
      ArgumentUtility.CheckNotNull ("textGeneratingExpressionVisitor", textGeneratingExpressionVisitor);
      ArgumentUtility.CheckNotNull ("stage", stage);

      commandBuilder.Append (_sqlText);
    }

    protected override Expression VisitChildren (ExpressionTreeVisitor visitor)
    {
      return this;
    }

  }
}