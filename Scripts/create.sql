
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

--DROP TABLE zamestnanec;


SELECT id, jmeno, prijmeni, email, telefon, datum_narozeni, datum_nastupu, hodinova_mzda, pobocka_id FROM zamestnanec;

SELECT jmeno, prijmeni, email, telefon, datum_narozeni, datum_nastupu, hodinova_mzda, pobocka_id FROM zamestnanec WHERE id=1

INSERT INTO zamestnanec (jmeno, prijmeni, email, telefon, datum_narozeni, datum_nastupu, hodinova_mzda, pobocka_id)
VALUES ('Michal', 'Pls', 'Funguj', '1235454', '2007-05-08', '2007-05-09', 150, 1);

--DROP TABLE zakaznik;

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

SELECT id, jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id FROM zakaznik;

INSERT INTO zakaznik(jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id)
VALUES ('Michal', 'Pls', 'Funguj', '1235454', '2007-05-08', 'michal', 'blabla', '90879834279', 1, 1);

SELECT jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id FROM zakaznik WHERE id=1;


CREATE TABLE rezervace (
	id INTEGER PRIMARY KEY IDENTITY,
	datum_zacatku DATETIME,
	datum_konce DATETIME,
	cena INTEGER,
	kauce INTEGER,
	zakaznik_id INTEGER,
	vozidlo_id INTEGER);

SELECT id, datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id FROM rezervace;

INSERT INTO rezervace(datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id)
VALUES ('2007-05-08', '2007-05-08', 300, 10, 1, 1);

SELECT datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id FROM rezervace WHERE id=1;


CREATE TABLE vozdilo (
	id INTEGER PRIMARY KEY IDENTITY,
	znacka VARCHAR(100),
	model VARCHAR(100),
	spz VARCHAR(100),
	cena_za_den INTEGER,
	pocet_dveri INTEGER,
	motor VARCHAR(100),
	spotreba FLOAT,
	aktivni BIT);

SELECT id, znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, aktivni FROM vozdilo;

INSERT INTO vozdilo(znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, aktivni)
VALUES ('noname', 'trash', 'pls', 1000, 5, 'uzasny', 10.2, 1);

SELECT znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, aktivni FROM vozdilo WHERE id=1;
