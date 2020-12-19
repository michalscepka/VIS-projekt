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

INSERT INTO zakaznik(jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id)
VALUES ('Michal', 'Pls', 'Funguj', '1235454', '2007-05-08', 'michal', 'blabla', '90879834279', 1);
INSERT INTO zakaznik(jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id)
VALUES ('Pepa', 'Pls', 'Funguj', '1235454', '2007-05-08', 'michal', 'blabla', '90879834279', 1);

INSERT INTO rezervace(datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id)
VALUES ('2021-01-01', '2021-01-01', 250, 0, 1, 1);
INSERT INTO rezervace(datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id)
VALUES ('2021-01-01', '2021-01-01', 260, 0, 1, 2);
INSERT INTO rezervace(datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id)
VALUES ('2021-01-02', '2021-01-02', 250, 0, 1, 1);
INSERT INTO rezervace(datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id)
VALUES ('2021-01-03', '2021-01-03', 290, 0, 1, 3);
INSERT INTO rezervace(datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id)
VALUES ('2021-01-03', '2021-01-03', 290, 0, 1, 4);
INSERT INTO rezervace(datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id)
VALUES ('2021-02-01', '2021-02-01', 290, 0, 2, 4);

INSERT INTO vozidlo(znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, obrazek, aktivni, pobocka_id)
VALUES ('Škoda', 'Fabia III', '1T1 2222', 250, 5, '1,0 TSI (70 kW)', 5.2, 'https://homel.vsb.cz/~sce0007/VIS/fabia1.jpg', 1, 1);
INSERT INTO vozidlo(znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, obrazek, aktivni, pobocka_id)
VALUES ('Škoda', 'Fabia III', '1T1 0001', 260, 5, '1,4 TSI (92 kW)', 6.1, 'https://homel.vsb.cz/~sce0007/VIS/fabia2.jpg', 1, 1);
INSERT INTO vozidlo(znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, obrazek, aktivni, pobocka_id)
VALUES ('Škoda', 'Octavia III', '1T1 0002', 290, 5, '2,0 TDI (110 kW)', 4.1, 'https://homel.vsb.cz/~sce0007/VIS/octavia1.jpg', 1, 1);
INSERT INTO vozidlo(znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, obrazek, aktivni, pobocka_id)
VALUES ('Škoda', 'Octavia III', '1T1 0003', 290, 5, '2,0 TDI (110 kW)', 6.2, 'https://homel.vsb.cz/~sce0007/VIS/octavia2.jpg', 1, 1);
INSERT INTO vozidlo(znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, obrazek, aktivni, pobocka_id)
VALUES ('Škoda', 'Superb III', '1T1 0004', 300, 5, '2,0 TDI (140 kW)', 7.2, 'https://homel.vsb.cz/~sce0007/VIS/superb1.jpg', 1, 1);
