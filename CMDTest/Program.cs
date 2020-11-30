using System;

using BusinessLayer.BO;

namespace CMDTest
{
	public class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			Zakaznik uzivatel = new Zakaznik
			{
				Jmeno = "Petr"
			};
			Console.WriteLine(uzivatel.Jmeno);
		}
	}
}
