-- *********************************************
-- * Standard SQL generation                   
-- *--------------------------------------------
-- * DB-MAIN version: 9.3.0              
-- * Generator date: Feb 16 2016              
-- * Generation date: Fri Dec 30 09:54:55 2016 
-- * LUN file: C:\Users\admin\Desktop\bib.lun 
-- * Schema: SCHEMA/SQL 
-- ********************************************* 


-- Database Section
-- ________________ 

create database BIBLI;


-- DBSpace Section
-- _______________


-- Tables Section
-- _____________ 

create table Auteur (
     idAut varchar(5) not null,
     nomAuteur varchar(60) not null,
     prenomAut varchar(60) not null,
     constraint ID_Auteur_ID primary key (idAut));

create table Emprunt (
     idEmprunt varchar(5) not null,
     idEx varchar(5) not null,
     numCarte varchar(5) not null,
     dateEmprnt date not null,
     dateMax date not null,
     constraint SID_Emprunt_ID unique (numCarte, idEx),
     constraint ID_Emprunt_ID primary key (idEmprunt));

create table Emprunteur (
     numCarte varchar(5) not null,
     nom varchar(20) not null,
     prenom varchar(25) not null,
     adresse varchar(200) not null,
     email varchar(30) not null,
     constraint ID_Emprunteur_ID primary key (numCarte));

create table Exemplaire (
     idEx varchar(5) not null,
     reference varchar(10) not null,
     anneEdition numeric(4) not null,
     idliv varchar(5) not null,
     idME varchar(5) not null,
     constraint ID_Exemplaire_ID primary key (idEx));

create table Livre (
     idliv varchar(5) not null,
     titre varchar(30) not null,
     anneeCrea numeric(4) not null,
     descr varchar(100) not null,
     idAut varchar(5) not null,
     constraint ID_Livre_ID primary key (idliv));

	 
create table MaisonEdition (
     idME varchar(5) not null,
     nom varchar(30) not null,
     ville varchar(25) not null,
     pays varchar(30) not null,
     constraint ID_MaisonEdition_ID primary key (idME));


-- Constraints Section
-- ___________________ 

alter table Emprunt add constraint FKemp_Exe_FK
     foreign key (idEx)
     references Exemplaire;

alter table Emprunt add constraint FKemp_Emp
     foreign key (numCarte)
     references Emprunteur;

alter table Exemplaire add constraint FKref_FK
     foreign key (idliv)
     references Livre;

alter table Exemplaire add constraint FKR_FK
     foreign key (idME)
     references MaisonEdition;

alter table Livre add constraint FKecrit_FK
     foreign key (idAut)
     references Auteur;


-- Index Section
-- _____________ 

create unique index ID_Auteur_IND
     on Auteur (idAut);

create unique index SID_Emprunt_IND
     on Emprunt (numCarte, idEx);

create unique index ID_Emprunt_IND
     on Emprunt (idEmprunt);

create index FKemp_Exe_IND
     on Emprunt (idEx);

create unique index ID_Emprunteur_IND
     on Emprunteur (numCarte);

create unique index ID_Exemplaire_IND
     on Exemplaire (idEx);

create index FKref_IND
     on Exemplaire (idliv);

create index FKR_IND
     on Exemplaire (idME);

create unique index ID_Livre_IND
     on Livre (idliv);

create index FKecrit_IND
     on Livre (idAut);

create unique index ID_MaisonEdition_IND
     on MaisonEdition (idME);
	 
insert into Auteurs values
('00001','Hugo','Victor'),
('00002','Zola','Emile');
	insert into  Livre values
( '00001','Les miserables',1971,'...','00001'),
( '00002','Germinal',1981,'...','00002');

