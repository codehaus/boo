"""
Si
"""
using System
using Boo.Tests.Ast.Compiler from Boo.Tests

p = Person(LastName: "Simpson")

firstLetter, secondLetter = p.LastName

Console.Write("${firstLetter}${secondLetter}")
