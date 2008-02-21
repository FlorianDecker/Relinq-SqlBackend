using System;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Rubicon.Data.Linq.Clauses;

namespace Rubicon.Data.Linq.UnitTests.ParsingTest.StructureTest.QueryParserIntegrationTest
{
  [TestFixture]
  public class SelectWhereQueryTest : QueryTestBase<string>
  {
    protected override IQueryable<string> CreateQuery ()
    {
      return TestQueryGenerator.CreateSelectWhereQuery (QuerySource);
    }

    [Test]
    public override void CheckBodyClause ()
    {
      Assert.AreEqual (1, ParsedQuery.QueryBody.BodyClauses.Count);
      WhereClause whereClause = ParsedQuery.QueryBody.BodyClauses.First () as WhereClause;
      Assert.IsNotNull (whereClause);

      Assert.AreSame (SourceExpressionNavigator.Arguments[0].Arguments[1].Operand.Expression, whereClause.BoolExpression);
    }

    [Test]
    public override void CheckSelectOrGroupClause ()
    {
      Assert.IsNotNull (ParsedQuery.QueryBody.SelectOrGroupClause);
      SelectClause clause = ParsedQuery.QueryBody.SelectOrGroupClause as SelectClause;
      Assert.IsNotNull (clause);
      Assert.IsNotNull (clause.ProjectionExpression);
      Assert.IsInstanceOfType (typeof (MemberExpression), clause.ProjectionExpression.Body,
          "from s in ... select s.First => select expression must be member access");
      Assert.AreEqual ("First", ((MemberExpression) clause.ProjectionExpression.Body).Member.Name,
          "from s in ... select s.First => select expression must be access to First member");
    }
  }
}