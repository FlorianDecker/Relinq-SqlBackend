﻿// This file is part of the re-linq project (relinq.codeplex.com)
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
using System.Collections.Generic;
using Remotion.Linq.SqlBackend.SqlGeneration;

namespace Remotion.Linq.LinqToSqlAdapter
{
  /// <summary>
  /// Provides functionality to get results from a database with a given command
  /// </summary>
  public interface IQueryResultRetriever
  {
    IEnumerable<T> GetResults<T> (Func<IDatabaseResultRow, T> projection, string commandText, CommandParameter[] parameters);
    T GetScalar<T> (Func<IDatabaseResultRow, T> projection, string commandText, CommandParameter[] parameters);
  }
}