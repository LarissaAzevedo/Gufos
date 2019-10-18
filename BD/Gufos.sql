create database Gufos;

use Gufos;

CREATE TABLE Tipo_usuario(
	Tipo_usuario_id INT IDENTITY PRIMARY KEY,
	Titulo VARCHAR(255) UNIQUE NOT NULL
);

CREATE TABLE Usuario(
	Usuario_id INT IDENTITY PRIMARY KEY,
	Nome VARCHAR(255) NOT NULL,
	Email VARCHAR(255) UNIQUE NOT NULL,
	Senha TEXT NOT NULL,
	Tipo_usuario_id INT FOREIGN KEY REFERENCES Tipo_usuario(Tipo_usuario_id)	
);

CREATE TABLE Localizacao (
	Localizacao_id INT IDENTITY PRIMARY KEY,
	CNPJ CHAR(14) UNIQUE NOT NULL,
	Razao_social VARCHAR(255) UNIQUE NOT NULL,
	Endereco VARCHAR(255) NOT NULL
);

CREATE TABLE Categoria (
	Categoria_id INT IDENTITY PRIMARY KEY,
	Titulo VARCHAR(255) UNIQUE NOT NULL
);

CREATE TABLE Evento (
	Evento_id INT IDENTITY PRIMARY KEY,
	Titulo VARCHAR(255) NOT NULL,
	Categoria_id INT FOREIGN KEY REFERENCES Categoria(Categoria_id),
	Acesso_livre BIT DEFAULT(1) NOT NULL,
	Data_evento DATETIME NOT NULL,
	Localizacao_id INT FOREIGN KEY REFERENCES Localizacao(Localizacao_id)
);

CREATE TABLE Presenca (
	Presenca_id INT IDENTITY PRIMARY KEY,
	Usuario_id INT FOREIGN KEY REFERENCES Usuario(Usuario_id),
	Evento_id INT FOREIGN KEY REFERENCES Evento(Evento_id),
	Presenca_status VARCHAR(255) NOT NULL
);

  
INSERT INTO Tipo_usuario(Titulo) VALUES ('Administrador'), ('Aluno');

INSERT INTO Usuario(Nome, Email, Senha, Tipo_usuario_id) 
VALUES 
	('admin', 'admin@email.com', 'admin123', 1),
	('Ariel', 'ariel@email.com', '123', 2);

INSERT INTO Localizacao(CNPJ, Razao_social, Endereco)
VALUES
	('12345678901234', 'Escola SENAI de Informática', 'Al. Barão de Limeira, 539')

INSERT INTO Categoria(Titulo)
VALUES
	('Desenvolvimento'),
	('HTML + CSS'),
	('Marketing')

INSERT INTO Evento(Titulo, Categoria_id, Acesso_livre, Data_evento, Localizacao_id)
VALUES
	('C#', 2, 0, '2019-08-07T18:00:00', 1),
	('Estrutura Semantica', 2, 1, GETDATE(), 1)

INSERT INTO Presenca(Evento_id, Usuario_id, Presenca_status)
VALUES 
	(1, 2, 'AGUARDANDO'),
	(1, 1, 'CONFIRMADO')