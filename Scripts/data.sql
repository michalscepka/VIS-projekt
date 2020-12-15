--ZAMESTNANEC
SELECT id, jmeno, prijmeni, email, telefon, datum_narozeni, datum_nastupu, hodinova_mzda, pobocka_id FROM zamestnanec;

SELECT jmeno, prijmeni, email, telefon, datum_narozeni, datum_nastupu, hodinova_mzda, pobocka_id FROM zamestnanec WHERE id=1

INSERT INTO zamestnanec (jmeno, prijmeni, email, telefon, datum_narozeni, datum_nastupu, hodinova_mzda, pobocka_id)
VALUES ('Michal', 'Pls', 'Funguj', '1235454', '2007-05-08', '2007-05-09', 150, 1);

--ZAKAZNIK
SELECT id, jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id FROM zakaznik;

INSERT INTO zakaznik(jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id)
VALUES ('Michal', 'Pls', 'Funguj', '1235454', '2007-05-08', 'michal', 'blabla', '90879834279', 1);

SELECT jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id FROM zakaznik WHERE id=1;


--REZERVACE
INSERT INTO rezervace(datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id)
VALUES ('2021-03-01', '2021-03-01', 250, 0, 1, 1);
INSERT INTO rezervace(datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id)
VALUES ('2021-03-01', '2021-03-01', 260, 0, 1, 2);
INSERT INTO rezervace(datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id)
VALUES ('2021-03-02', '2021-03-02', 250, 0, 1, 1);
INSERT INTO rezervace(datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id)
VALUES ('2021-03-03', '2021-03-03', 290, 0, 1, 3);
INSERT INTO rezervace(datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id)
VALUES ('2021-03-03', '2021-03-03', 290, 0, 1, 4);

SELECT id, datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id FROM rezervace;

UPDATE rezervace SET vozidlo_id = 1004 WHERE id = 1004

SELECT datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id FROM rezervace WHERE id=1;

--VOZIDLO

INSERT INTO vozidlo(znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, aktivni, pobocka_id)
VALUES ('Škoda', 'Fabia', '1T1 0000', 250, 5, '-', 5.5, 1, 1);
INSERT INTO vozidlo(znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, aktivni, pobocka_id)
VALUES ('Škoda', 'Fabia', '1T1 0001', 260, 5, '-', 5.9, 1, 1);
INSERT INTO vozidlo(znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, aktivni, pobocka_id)
VALUES ('Škoda', 'Octavia', '1T1 0002', 290, 5, '-', 6.2, 1, 1);
INSERT INTO vozidlo(znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, aktivni, pobocka_id)
VALUES ('Škoda', 'Octavia', '1T1 0003', 290, 5, '-', 6.2, 1, 1);
INSERT INTO vozidlo(znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, aktivni, pobocka_id)
VALUES ('Škoda', 'Superb', '1T1 0004', 300, 5, '-', 6.5, 1, 1);

SELECT id, znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, aktivni, pobocka_id FROM vozidlo;

UPDATE vozidlo SET aktivni = 1 WHERE id = 5

UPDATE vozidlo SET aktivni = 1 WHERE id = 3

SELECT znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, aktivni, pobocka_id FROM vozidlo WHERE id=1;

