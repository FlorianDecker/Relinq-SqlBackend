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
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Remotion.Data.Linq.Parsing;
using Remotion.Data.Linq.SqlBackend.SqlGeneration;
using Remotion.Data.Linq.UnitTests.Linq.Core.Clauses.Expressions;
using Rhino.Mocks;

namespace Remotion.Data.Linq.UnitTests.Linq.SqlBackend.SqlGeneration
{
  [TestFixture]
  public class SqlCustomTextExpressionTest
  {
    private SqlCustomTextExpression _sqlCustomTextExpression;

    [SetUp]
    public void SetUp ()
    {
      _sqlCustomTextExpression = new SqlCustomTextExpression ("test", typeof (string));
    }


    [Test]
    public void Generate ()
    {
      var commandBuilder = new SqlCommandBuilder();
      var visitor = MockRepository.GenerateMock<ExpressionTreeVisitor>();
      var stage = MockRepository.GenerateMock<ISqlGenerationStage>();

      _sqlCustomTextExpression.Generate (commandBuilder, visitor, stage);

      Assert.That (commandBuilder.GetCommandText(), Is.EqualTo ("test"));
    }

    [Test]
    public void VisitChildren_ReturnsThis ()
    {
      var visitorMock = MockRepository.GenerateStrictMock<ExpressionTreeVisitor> ();
      visitorMock.Replay ();

      var result = ExtensionExpressionTestHelper.CallVisitChildren (_sqlCustomTextExpression, visitorMock);

      visitorMock.VerifyAllExpectations ();
      Assert.That (result, Is.SameAs (_sqlCustomTextExpression));
    }
  }
}