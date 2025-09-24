using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomNumber2;

namespace RandomNumber2.Tests
{
	[TestClass]
	public class GuessNumberGameTests
	{
		[TestMethod]
		public void GenerateRandomNumber_IsWithinRange()
		{
			GuessNumberGame game = new GuessNumberGame();
			int number = game.GenerateRandomNumber();
			Assert.IsTrue(number >= 1 && number <= 100);
		}

		[TestMethod]
		public void CheckGuess_ReturnsTrueIfCorrect()
		{
			GuessNumberGame game = new GuessNumberGame();
			bool result = game.CheckGuess(50, 50);
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void CheckGuess_ReturnsFalseIfIncorrect()
		{
			GuessNumberGame game = new GuessNumberGame();
			bool result = game.CheckGuess(30, 50);
			Assert.IsFalse(result);
		}
	}
}


