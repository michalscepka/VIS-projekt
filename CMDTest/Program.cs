using System;

using BusinessLayer.BO;
using DataLayer;

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

			Database db = new Database();

			bool pls = db.Connect();
			Console.WriteLine(pls);
		}
	}
}
