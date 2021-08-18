USE NeroStoreDB;

CREATE TABLE Tilaus (
	tilaus_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	email VARCHAR(100) NOT NULL,
	tilauspvm DATE NULL,
	toimitusPvm DATE NULL,
	tilaussumma DECIMAL(7,2) NULL,
	kayttaja_id INT NULL ---tämä siltä varalta et laajennetaan käyttäjäkohtaiseen tilaamiseen.
);

CREATE TABLE Tuote (
	tuote_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	nimi VARCHAR(100) NOT NULL,
	hinta DECIMAL(7,2) NOT NULL,
	lkm INT NOT NULL,
	kuvaus VARCHAR(500) NULL,
	tyyppi VARCHAR(50) NULL,
	tuoteryhma VARCHAR(50) NULL
);

CREATE TABLE TilausRivi (
	tilausrivi_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	lkm int NOT NULL,
	tilaus_id int FOREIGN KEY REFERENCES Tilaus(tilaus_id),
	tuote_id int FOREIGN KEY REFERENCES Tuote(tuote_id)
);

CREATE TABLE Kayttaja (
	kayttaja_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	email VARCHAR(100) NOT NULL,
	salasana VARCHAR(100) NOT NULL,
	etunimi VARCHAR(30) NULL,
	sukunimi VARCHAR(30) NULL,
	osoite VARCHAR(30) NULL,
	postinumero CHAR(5) NULL,
	syntymäaika DATE NULL
);

ALTER TABLE Tuote
ALTER COLUMN tyyppi VARCHAR(500) NULL;

ALTER TABLE Kayttaja
ADD onAdmin BIT NOT NULL;


CREATE TABLE Nayttokerrat (
	tuote_id INT NOT NULL PRIMARY KEY,
	lkm INT NOT NULL
	CONSTRAINT FK_Nayttokerrat_Tuote FOREIGN KEY(tuote_id)
	references Tuote(tuote_id)
);

