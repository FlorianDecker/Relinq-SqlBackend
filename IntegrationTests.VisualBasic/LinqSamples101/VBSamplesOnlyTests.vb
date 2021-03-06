﻿'Microsoft Public License (Ms-PL)

'This license governs use of the accompanying software. If you use the software, you
'accept this license. If you do not accept the license, do not use the software.

'1. Definitions
'The terms "reproduce," "reproduction," "derivative works," and "distribution" have the
'same meaning here as under U.S. copyright law.
'A "contribution" is the original software, or any additions or changes to the software.
'A "contributor" is any person that distributes its contribution under this license.
'"Licensed patents" are a contributor's patent claims that read directly on its contribution.

'2. Grant of Rights
'(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
'each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, 
'prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
'(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
'each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, 
'sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.

'3. Conditions and Limitations
'(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
'(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from 
'such contributor to the software ends automatically.
'(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present 
'in the software.
'(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of 
'this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a 
'license that complies with this license.
'(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. 
'You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws,
'the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.

Option Infer On
Option Strict On

Imports NUnit.Framework

Imports System.Reflection
Imports Remotion.Linq.IntegrationTests.Common


Namespace LinqSamples101
  <TestFixture()> _
  Public Class VBSamplesOnlyTests
    Inherits TestBase

    'This sample selects 2 columns and returns the data from the database.
    <Test()> _
    Public Sub LinqToSqlFirst01()

      'Instead of returning the entire Customers table, just return the
      'CompanyName and Country
      Dim londonCustomers = From cust In db.Customers _
                            Select cust.CompanyName, cust.Country

      TestExecutor.Execute(londonCustomers, MethodBase.GetCurrentMethod())
    End Sub

    'This sample uses a Where clause to filter for Customers in London.
    <Test()> _
    Public Sub LinqToSqlFirst02()

      'Only return customers from London
      Dim londonCustomers = From cust In DB.Customers _
            Where cust.City = "London" _
            Select cust.CompanyName, cust.City, cust.Country


      TestExecutor.Execute(londonCustomers, MethodBase.GetCurrentMethod())
    End Sub

    'This sample uses a method mapped to the 'ProductsUnderThisUnitPrice' function
    'in Northwind database to return products with unit price less than $10.00.
    'Methods can be created by dragging database functions from the Server
    'Explorer onto the O/R Designer which can be accessed by double-clicking
    'on the .DBML file in the Solution Explorer.
    <Test()> _
    <Explicit("Not tested: Stored procedures")> _
    Public Sub LinqToSqlStoredProc06()
      Dim cheapProducts = DB.ProductsUnderThisUnitPrice(10D)

      TestExecutor.Execute(cheapProducts, MethodBase.GetCurrentMethod())
    End Sub

    'This sample queries against a collection of products returned by
    'ProductsUnderThisUnitPrice' method. The method was created from the database
    'function 'ProductsUnderThisUnitPrice' in Northwind database.
    <Test()> _
    <Ignore("RM-3313: Add a TableInfo type allowing user-defined functions to be used as tables")> _
    Public Sub LinqToSqlStoredProc07()
      Dim cheapProducts = From prod In DB.ProductsUnderThisUnitPrice(10D) _
                          Where prod.Discontinued = True

      TestExecutor.Execute(cheapProducts, MethodBase.GetCurrentMethod())
    End Sub

  End Class

End Namespace

