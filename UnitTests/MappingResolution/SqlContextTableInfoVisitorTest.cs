// This file is part of the re-linq project (relinq.codeplex.com)
// Copyright (c) rubicon IT GmbH, www.rubicon.eu
// 
// re-linq is free software; you can redistribute it and/or modify it under 
// the terms of the GNU Lesser General Public License as published by the 
// Free Software Foundation; either version 2.1 of the License, 
// or (at your option) any later version.
// 
// re-linq is distributed in the hope that it will be useful, 
// but WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the 
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with re-linq; if not, see http://www.gnu.org/licenses.
// 

using System;
using NUnit.Framework;
using Remotion.Linq.SqlBackend.MappingResolution;
using Remotion.Linq.SqlBackend.SqlStatementModel.Resolved;
using Remotion.Linq.SqlBackend.UnitTests.SqlStatementModel;
using Remotion.Linq.SqlBackend.UnitTests.TestDomain;
using Rhino.Mocks;

namespace Remotion.Linq.SqlBackend.UnitTests.MappingResolution
{
  [TestFixture]
  public class SqlContextTableInfoVisitorTest
  {
    private IMappingResolutionStage _stageMock;
    private IMappingResolutionContext _mappingresolutionContext;

    [SetUp]
    public void SetUp ()
    {
      _stageMock = MockRepository.GenerateMock<IMappingResolutionStage> ();
      _mappingresolutionContext = new MappingResolutionContext();
    }

    [Test]
    public void ApplyContext_ResolvedSimpleTableInfo ()
    {
      var tableInfo = new ResolvedSimpleTableInfo (typeof (Cook), "CookTable", "c");

      var result = SqlContextTableInfoVisitor.ApplyContext (tableInfo, SqlExpressionContext.ValueRequired, _stageMock, _mappingresolutionContext);

      Assert.That (result, Is.SameAs (tableInfo));
    }

    [Test]
    public void ApplyContext_ResolvedSubStatementTableInfo_SameTableInfo ()
    {
      var subStatement = SqlStatementModelObjectMother.CreateSqlStatement_Resolved (typeof (Cook));
      var subStatementTableInfo = new ResolvedSubStatementTableInfo ("c", subStatement);
     
      _stageMock
          .Expect (mock => mock.ApplySelectionContext (subStatement, SqlExpressionContext.ValueRequired, _mappingresolutionContext))
          .Return (subStatement);
      _stageMock.Replay ();

      var result = SqlContextTableInfoVisitor.ApplyContext (subStatementTableInfo, SqlExpressionContext.ValueRequired, _stageMock, _mappingresolutionContext);

      Assert.That (result, Is.SameAs (subStatementTableInfo));
      _stageMock.VerifyAllExpectations ();
    }

    [Test]
    public void ApplyContext_ResolvedSubStatementTableInfo_NewTableInfo ()
    {
      var subStatement = SqlStatementModelObjectMother.CreateSqlStatement_Resolved (typeof (Cook));
      var subStatementTableInfo = new ResolvedSubStatementTableInfo ("c", subStatement);
      var returnedStatement = SqlStatementModelObjectMother.CreateSqlStatement_Resolved (typeof (Cook));

      _stageMock
          .Expect (mock => mock.ApplySelectionContext (subStatement, SqlExpressionContext.ValueRequired, _mappingresolutionContext))
          .Return (returnedStatement);
      _stageMock.Replay ();

      var result = (ResolvedSubStatementTableInfo) SqlContextTableInfoVisitor.ApplyContext (
          subStatementTableInfo,
          SqlExpressionContext.ValueRequired,
          _stageMock,
          _mappingresolutionContext);

      _stageMock.VerifyAllExpectations ();
      Assert.That (result, Is.Not.SameAs (subStatementTableInfo));
      Assert.That (result.SqlStatement, Is.SameAs (returnedStatement));
      Assert.That (result.TableAlias, Is.EqualTo (subStatementTableInfo.TableAlias));
    }

    [Test]
    public void ApplyContext_ResolvedJoinedGroupingTableInfo_SameTableInfo ()
    {
      var subStatement = SqlStatementModelObjectMother.CreateSqlStatement_Resolved (typeof (Cook));
      var joinedGroupingTableInfo = SqlStatementModelObjectMother.CreateResolvedJoinedGroupingTableInfo (subStatement);

      _stageMock
          .Expect (mock => mock.ApplySelectionContext (subStatement, SqlExpressionContext.ValueRequired, _mappingresolutionContext))
          .Return (subStatement);
      _stageMock.Replay ();

      var result = SqlContextTableInfoVisitor.ApplyContext (joinedGroupingTableInfo, SqlExpressionContext.ValueRequired, _stageMock, _mappingresolutionContext);

      Assert.That (result, Is.SameAs (joinedGroupingTableInfo));
      _stageMock.VerifyAllExpectations ();
    }

    [Test]
    public void ApplyContext_ResolvedJoinedGroupingTableInfo_NewTableInfo ()
    {
      var subStatement = SqlStatementModelObjectMother.CreateSqlStatement_Resolved (typeof (Cook));

      var joinedGroupingTableInfo = SqlStatementModelObjectMother.CreateResolvedJoinedGroupingTableInfo (subStatement);
      var returnedStatement = SqlStatementModelObjectMother.CreateSqlStatement_Resolved (typeof (Cook));

      _stageMock
          .Expect (mock => mock.ApplySelectionContext (subStatement, SqlExpressionContext.ValueRequired, _mappingresolutionContext))
          .Return (returnedStatement);
      _stageMock.Replay ();

      var result = (ResolvedJoinedGroupingTableInfo) SqlContextTableInfoVisitor.ApplyContext (joinedGroupingTableInfo, SqlExpressionContext.ValueRequired, _stageMock, _mappingresolutionContext);

      _stageMock.VerifyAllExpectations ();
      Assert.That (result, Is.Not.SameAs (joinedGroupingTableInfo));
      Assert.That (result.GroupSourceTableAlias, Is.EqualTo (joinedGroupingTableInfo.GroupSourceTableAlias));
      Assert.That (result.AssociatedGroupingSelectExpression, Is.SameAs (joinedGroupingTableInfo.AssociatedGroupingSelectExpression));
      Assert.That (result.SqlStatement, Is.SameAs (returnedStatement));
      Assert.That (result.TableAlias, Is.EqualTo (joinedGroupingTableInfo.TableAlias));
    }

    [Test]
    [ExpectedException (typeof (InvalidOperationException), ExpectedMessage = "UnresolvedTableInfo is not valid at this point.")]
    public void ApplyContext_UnresolvedTableInfo ()
    {
      var tableInfo = SqlStatementModelObjectMother.CreateUnresolvedTableInfo();
     
      SqlContextTableInfoVisitor.ApplyContext (tableInfo, SqlExpressionContext.ValueRequired, _stageMock, _mappingresolutionContext);
    }

    [Test]
    [ExpectedException (typeof (InvalidOperationException), ExpectedMessage = "UnresolvedJoinTableInfo is not valid at this point.")]
    public void ApplyContext_UnresolvedJoinTableInfo ()
    {
      var tableInfo = SqlStatementModelObjectMother.CreateUnresolvedJoinTableInfo();
     
      SqlContextTableInfoVisitor.ApplyContext (tableInfo, SqlExpressionContext.ValueRequired, _stageMock, _mappingresolutionContext);
    }

    [Test]
    [ExpectedException (typeof (InvalidOperationException), ExpectedMessage = "UnresolvedCollectionJoinTableInfo is not valid at this point.")]
    public void ApplyContext_UnresolvedCollectionJoinTableInfo ()
    {
      var tableInfo = SqlStatementModelObjectMother.CreateUnresolvedCollectionJoinTableInfo();
     
      SqlContextTableInfoVisitor.ApplyContext (tableInfo, SqlExpressionContext.ValueRequired, _stageMock, _mappingresolutionContext);
    }

    [Test]
    [ExpectedException (typeof (InvalidOperationException), ExpectedMessage = "UnresolvedGroupReferenceTableInfo is not valid at this point.")]
    public void ApplyContext_UnresolvedGroupReferenceTableInfo ()
    {
      var tableInfo = SqlStatementModelObjectMother.CreateUnresolvedGroupReferenceTableInfo();

      SqlContextTableInfoVisitor.ApplyContext (tableInfo, SqlExpressionContext.ValueRequired, _stageMock, _mappingresolutionContext);
    }

  }
}