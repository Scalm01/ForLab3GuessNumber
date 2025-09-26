// <copyright file="GuessNumberGameTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RandomNumber2.Tests
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomNumber2;

   /// <summary>
   /// Tester för GuessNumberGame-klassen.
   /// </summary>
[TestClass]
public class GuessNumberGameTests // test method
    {
      /// <summary>
      /// Verifierar att GenerateRandomNumber returnerar ett tal mellan 1 och 100.
      /// </summary>
        [TestMethod]
        public void GenerateRandomNumber_IsWithinRange()
        {
            GuessNumberGame game = new GuessNumberGame();
            int number = game.GenerateRandomNumber();
            Assert.IsTrue(number >= 1 && number <= 100);
        }

      /// <summary>
      /// Verifierar att CheckGuess returnerar true när gissningen är korrekt.
      /// </summary>
        [TestMethod]
        public void CheckGuess_ReturnsTrueIfCorrect()
        {
            GuessNumberGame game = new GuessNumberGame();
            bool result = game.CheckGuess(50, 50);
            Assert.IsTrue(result);
        }

      /// <summary>
      /// Verifierar att CheckGuess returnerar false när gissningen är felaktig.
      /// </summary>
        [TestMethod]
        public void CheckGuess_ReturnsFalseIfIncorrect()
        {
            GuessNumberGame game = new GuessNumberGame();
            bool result = game.CheckGuess(30, 50);
            Assert.IsFalse(result);
        }
    }
}