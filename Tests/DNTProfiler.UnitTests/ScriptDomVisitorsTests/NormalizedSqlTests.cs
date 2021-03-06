﻿using DNTProfiler.Common.Toolkit;
using DNTProfiler.Infrastructure.ScriptDomVisitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNTProfiler.UnitTests.ScriptDomVisitorsTests
{
    [TestClass]
    public class NormalizedSqlTests
    {
        [TestMethod]
        public void SameQueriesWithDifferentLiteralValuesShouldHaveTheSameHash()
        {
            var sql1 = @"SELECT TOP (2)
                            [Extent1].[Id] AS [Id],
                            [Extent1].[Name] AS [Name],
                            [Extent1].[Value] AS [Value]
                            FROM [dbo].[SiteOptions] AS [Extent1]
                            WHERE (N'Store1' = [Extent1].[Name])";

            var sql2 = @"SELECT TOP (2)
                            [Extent1].[Id] AS [Id], [Extent1].[Name] AS [Name],
                            [Extent1].[Value] AS [Value]
                            FROM [dbo].[SiteOptions] AS [Extent1]
                            WHERE (N'Store2' = [Extent1].[Name])";

            var hash1 = RunTSqlFragmentVisitor.GetNormalizedSqlHash(sql1, sql1.ComputeHash());
            var hash2 = RunTSqlFragmentVisitor.GetNormalizedSqlHash(sql2, sql2.ComputeHash());
            Assert.AreEqual(hash1.NormalizedSqlHash, hash2.NormalizedSqlHash);
        }
    }
}
