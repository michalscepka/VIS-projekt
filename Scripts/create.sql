DROP TABLE zamestnanec;
DROP TABLE zakaznik;
DROP TABLE rezervace;
DROP TABLE vozidlo;

CREATE TABLE zamestnanec (
	id INTEGER PRIMARY KEY IDENTITY,
	jmeno VARCHAR(100),
	prijmeni VARCHAR(100),
	email VARCHAR(100),
	telefon VARCHAR(100),
	datum_narozeni DATETIME,
	datum_nastupu DATETIME,
	hodinova_mzda INTEGER,
	pobocka_id INTEGER);
GO

CREATE TABLE zakaznik (
	id INTEGER PRIMARY KEY IDENTITY,
	jmeno VARCHAR(100),
	prijmeni VARCHAR(100),
	email VARCHAR(100),
	telefon VARCHAR(100),
	datum_narozeni DATETIME,
	login VARCHAR(100),
	heslo VARCHAR(100),
	cislo_platebni_karty VARCHAR(100),
	ridicsky_prukaz_id INTEGER);
GO

CREATE TABLE rezervace (
	id INTEGER PRIMARY KEY IDENTITY,
	datum_zacatku DATETIME,
	datum_konce DATETIME,
	cena INTEGER,
	kauce INTEGER,
	zakaznik_id INTEGER,
	vozidlo_id INTEGER);
GO

CREATE TABLE vozidlo (
	id INTEGER PRIMARY KEY IDENTITY,
	znacka VARCHAR(100),
	model VARCHAR(100),
	spz VARCHAR(100),
	cena_za_den INTEGER,
	pocet_dveri INTEGER,
	motor VARCHAR(100),
	spotreba FLOAT,
	obrazek VARCHAR(100),
	aktivni BIT,
	pobocka_id INTEGER);
GO
