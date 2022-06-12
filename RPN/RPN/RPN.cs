using System;
using System.Collections.Generic;

namespace RPNCalulator {
	public class RPN {
		private Stack<int> _operators;
		Dictionary<string, Func<int, int, int>> _operationFunction;

		

		public int EvalRPN(string input) {
			_operationFunction = new Dictionary<string, Func<int, int, int>>
			{
				["+"] = (fst, snd) => (fst + snd),
				["-"] = (fst, snd) => (fst - snd),
				["*"] = (fst, snd) => (fst * snd),
				["/"] = (fst, snd) => Dzielenie(fst, snd),
				["!"] = (fst, snd) => Factorial(fst)
				
			};
			
			_operators = new Stack<int>();

			var splitInput = input.Split(' ');
			foreach (var op in splitInput)
			{
				if (IsNumber(op))
					_operators.Push(Int32.Parse(op));
				else
				if (IsOperator(op))
				{
					var num1 = _operators.Pop();
					var num2 = _operators.Pop();
					_operators.Push(_operationFunction[op](num1, num2));
					//_operators.Push(Operation(op)(num1, num2));
				}
				else
					if(IsOneArgumentOperator(op))
				{
					var num1 = _operators.Pop();
					_operators.Push(_operationFunction[op](num1, 0));
				}
				
			}

			var result = _operators.Pop();
			if (_operators.IsEmpty)
			{
				return result;
			}
			throw new InvalidOperationException();
		}

		private int Dzielenie(int fst, int snd)
		{
			try
			{
				return(fst / snd);
			}
			catch (DivideByZeroException)
			{
				Console.WriteLine("Division of {0} by zero.", fst);
				return 999999999;
			}
		}

		private int Factorial(int fst)
		{
			return fst < 1 ? 1 : fst * Factorial(fst - 1);
		}

		private bool IsNumber(String input) => Int32.TryParse(input, out _);

		private bool IsOperator(String input) =>
			input.Equals("+") || input.Equals("-") ||
			input.Equals("*") || input.Equals("/");

		private bool IsOneArgumentOperator(String input) =>
			input.Equals("!");

		private Func<int, int, int> Operation(String input) =>
			(x, y) =>
			(
				(input.Equals("+") ? x + y :
					(input.Equals("*") ? x * y : int.MinValue)
				)
			);
	}
}