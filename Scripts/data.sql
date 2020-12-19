--ZAMESTNANEC
SELECT id, jmeno, prijmeni, email, telefon, datum_narozeni, datum_nastupu, hodinova_mzda, pobocka_id FROM zamestnanec;

SELECT jmeno, prijmeni, email, telefon, datum_narozeni, datum_nastupu, hodinova_mzda, pobocka_id FROM zamestnanec WHERE id=1

INSERT INTO zamestnanec (jmeno, prijmeni, email, telefon, datum_narozeni, datum_nastupu, hodinova_mzda, pobocka_id)
VALUES ('Michal', 'Pls', 'Funguj', '1235454', '2007-05-08', '2007-05-09', 150, 1);

--ZAKAZNIK
SELECT id, jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id FROM zakaznik;

INSERT INTO zakaznik(jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id)
VALUES ('Michal', 'Pls', 'Funguj', '1235454', '2007-05-08', 'michal', 'blabla', '90879834279', 1);
INSERT INTO zakaznik(jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id)
VALUES ('Pepa', 'Pls', 'Funguj', '1235454', '2007-05-08', 'michal', 'blabla', '90879834279', 1);

SELECT jmeno, prijmeni, email, telefon, datum_narozeni, login, heslo, cislo_platebni_karty, ridicsky_prukaz_id FROM zakaznik WHERE id=1;


--REZERVACE
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

SELECT id, datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id FROM rezervace;

UPDATE rezervace SET vozidlo_id = 1004 WHERE id = 1004

SELECT datum_zacatku, datum_konce, cena, kauce, zakaznik_id, vozidlo_id FROM rezervace WHERE id=1;

--VOZIDLO

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

SELECT id, znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, obrazek, aktivni, pobocka_id FROM vozidlo;

UPDATE vozidlo SET aktivni = 1 WHERE id = 5

UPDATE vozidlo SET aktivni = 1 WHERE id = 3

SELECT znacka, model, spz, cena_za_den, pocet_dveri, motor, spotreba, obrazek, aktivni, pobocka_id FROM vozidlo WHERE id=1;

