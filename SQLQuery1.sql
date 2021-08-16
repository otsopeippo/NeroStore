CREATE DATABASE NeroStoreDB;

USE NeroStoreDB;

CREATE TABLE Tilaus (
	tilaus_id INT NOT NULL PRIMARY KEY,
	email VARCHAR(100) NOT NULL,
	tilauspvm DATE NULL,
	kayttaja_id INT NULL ---tämä siltä varalta et laajennetaan käyttäjäkohtaiseen tilaamiseen.
);

CREATE TABLE Tuote (
	tuote_id INT NOT NULL PRIMARY KEY,
	nimi VARCHAR(100) NOT NULL,
	hinta DECIMAL(7,2) NOT NULL,
	lkm INT NOT NULL
);

CREATE TABLE TilausRivi (
	tilausrivi_id INT NOT NULL PRIMARY KEY,
	lkm int NOT NULL,
	tilaus_id int FOREIGN KEY REFERENCES Tilaus(tilaus_id),
	tuote_id int FOREIGN KEY REFERENCES Tuote(tuote_id)
);

CREATE TABLE Kayttaja (
	kayttaja_id INT NOT NULL PRIMARY KEY,
	email VARCHAR(100) NOT NULL,
	salasana VARCHAR(100) NOT NULL,

);

USE NeroStoreDB;
ALTER TABLE Tilaus
ADD toimitusPvm DATE NULL;
ALTER TABLE Tilaus
ADD	tilaussumma DECIMAL(7,2) NULL;
ALTER TABLE Tilaus
ALTER COLUMN tilauspvm DATE NOT NULL;

ALTER TABLE Tuote
ADD kuvaus VARCHAR(500) NULL;
ALTER TABLE Tuote
ADD tyyppi VARCHAR(50) NULL;
ALTER TABLE Tuote
ADD tuoteryhma VARCHAR(50) NULL;

ALTER TABLE Kayttaja
ADD etunimi VARCHAR(30) NULL;
ALTER TABLE Kayttaja
ADD sukunimi VARCHAR(30) NULL;
ALTER TABLE Kayttaja
ADD osoite VARCHAR(30) NULL;
ALTER TABLE Kayttaja
ADD postinumero CHAR(5) NULL;
ALTER TABLE Kayttaja
ADD syntymäaika DATE NULL;
