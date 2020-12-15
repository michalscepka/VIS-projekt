using System;
using System.Collections.Generic;
using BusinessLayer.BO;
using BusinessLayer.Controllers;
using DataLayer;
using DTO.Classes;

namespace CMDTest
{
	public class Program
	{
		static void Main(string[] args)
		{
			//Console.WriteLine(SpravaZamestnancu.Instance.CelkovyPocetZamestnancu);
			//Console.WriteLine(SpravaZamestnancu.Instance.SeznamZamestnancu[0].Pobocka.Id);

			//SpravaZamestnancu.Instance.SeznamZamestnancu[0].Jmeno = "Zmeneno";
			//SpravaZamestnancu.Instance.SaveAllData();

			//Console.WriteLine(SpravaZamestnancu.Instance.FindZamestnanec(1).Prijmeni);

			/*SpravaZamestnancu.Instance.AddZamestnanec(new Zamestnanec() {
				Id = -1,
				Jmeno = "Pepa",
				Prijmeni = "PLZ",
				Email = "hello",
				HodinovaMzda = 50,
				DatumNarozeni = new DateTime(2010, 10, 5),
				DatumNastupu = new DateTime(2020, 11, 11),
				Pobocka = new Pobocka() { Id = 1 },
				Telefon = "431431431"});*/

			/*Zamestnanec zamestnanec1 = new Zamestnanec()
			{
				Id = 3,
				Jmeno = "CHANGED",
				Prijmeni = "CHANGED",
				Email = "CHANGED",
				HodinovaMzda = 50,
				DatumNarozeni = new DateTime(2010, 10, 5),
				DatumNastupu = new DateTime(2020, 11, 11),
				Pobocka = new Pobocka() { Id = 1 },
				Telefon = "CHANGED"
			};

			//SpravaZamestnancu.Instance.UpdateZamestnanec(zamestnanec1);
			//SpravaZamestnancu.Instance.DeleteZamestnanec(zamestnanec1);

			Console.WriteLine(SpravaZamestnancu.Instance.CelkovyPocetZamestnancu);*/

			//Console.WriteLine(SpravaZakazniku.Instance.CelkovyPocetZakazniku);

			/*Zakaznik zakaznik = new Zakaznik()
			{
				Id = 2,
				Jmeno = "UPDATED",
				Prijmeni = "Novak",
				Email = "pls@help",
				Telefon = "12646546",
				DatumNarozeni = new DateTime(1990, 1, 1),
				Login = "plzzzz",
				Heslo = "plssss",
				CisloPlatebniKarty = "09897987897",
				Rezervace = new Rezervace() { Id = 1 },
				RidicskyPrukaz = new RidicskyPrukaz { Id = 1 }
			};*/

			//SpravaZakazniku.Instance.AddZakaznik(zakaznik);
			//SpravaZakazniku.Instance.UpdateZakaznik(zakaznik);
			//SpravaZakazniku.Instance.DeleteZakaznik(zakaznik);

			//Console.WriteLine(SpravaZakazniku.Instance.CelkovyPocetZakazniku);
			/*Console.WriteLine(SpravaRezervaci.Instance.CelkovyPocetRezervaci);

			Rezervace rezervace = new Rezervace()
			{
				Id = 10,
				DatumZacatkuRezervace = new DateTime(1990, 1, 1),
				DatumKonceRezervace = new DateTime(2020, 1, 1),
				Cena = 3000,
				Kauce = 0,
				Vozidlo = new Vozidlo() { Id = 1 },
				Zakaznik = new Zakaznik() { Id = 1 }
			};

			//SpravaRezervaci.Instance.AddRezervace(rezervace);
			//SpravaRezervaci.Instance.UpdateRezervace(rezervace);
			SpravaRezervaci.Instance.DeleteRezervace(rezervace);

			Console.WriteLine(SpravaRezervaci.Instance.CelkovyPocetRezervaci);*/

			/*Console.WriteLine(SpravaVozidel.Instance.CelkovyPocetVozidel);

			Vozidlo vozidlo = new Vozidlo()
			{
				Id = 1,
				Znacka = "BMW",
				Model = "E46",
				SPZ = "1T7 0514",
				CenaZaDen = 500,
				PocetDveri = 3,
				Motor = "320i 120kW",
				Spotreba = 10,
				Aktivni = true,
				Pobocka = new Pobocka() { Id = 1 }
			};

			//SpravaVozidel.Instance.AddVozidlo(vozidlo);
			//SpravaVozidel.Instance.UpdateVozidlo(vozidlo);
			SpravaVozidel.Instance.DeleteVozidlo(vozidlo);

			Console.WriteLine(SpravaVozidel.Instance.CelkovyPocetVozidel);*/

			/*Console.WriteLine(SpravaPobocek.Instance.CelkovyPocetPobocek);

			SpravaPobocek.Instance.AddPobocka(new Pobocka()
			{
				Id = -1,
				Mesto = "test",
				Ulice = "testuju",
				Telefon = "111111"
			});

			SpravaPobocek.Instance.SaveAll();
			Console.WriteLine(SpravaPobocek.Instance.CelkovyPocetPobocek);*/
		}
	}
}
