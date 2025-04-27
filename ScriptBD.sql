--PRIMERA EJECUCI�N
-- Crear base de datos
CREATE DATABASE RegistroEstudiantesDB;
GO

USE RegistroEstudiantesDB;
GO

-- Tabla de profesores
CREATE TABLE Profesor (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL
);

-- Tabla de materias
CREATE TABLE Materia (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    ProfesorId INT NOT NULL,
    FOREIGN KEY (ProfesorId) REFERENCES Profesor(Id)
);

-- Tabla de estudiantes
CREATE TABLE Estudiante (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL
);

-- Relaci�n estudiante-materia
CREATE TABLE EstudianteMateria (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EstudianteId INT NOT NULL,
    MateriaId INT NOT NULL,
    FOREIGN KEY (EstudianteId) REFERENCES Estudiante(Id),
    FOREIGN KEY (MateriaId) REFERENCES Materia(Id)
);

-- Insertar profesores
INSERT INTO Profesor (Nombre) VALUES 
('Profesor Alberto'),
('Profesor Beto-'),
('Profesor Cristian'),
('Profesor Diego'),
('Profesora Erika');

-- Insertar materias (cada profesor dicta 2 materias)
INSERT INTO Materia (Nombre, ProfesorId) VALUES 
('Matem�ticas', 1),
('F�sica', 1),
('Qu�mica', 2),
('Biolog�a', 2),
('Historia', 3),
('Geograf�a', 3),
('Literatura', 4),
('Ingl�s', 4),
('Programaci�n', 5),
('Bases de Datos', 5);
--FIN PRIMERA EJECUCI�N

--SEGUNDA EJECUCI�N
--modificaciones 


ALTER TABLE EstudianteMateria
ADD ProfesorId INT NOT NULL;

ALTER TABLE EstudianteMateria
ADD CONSTRAINT FK_EstudianteMateria_Profesor
FOREIGN KEY (ProfesorId) REFERENCES Profesor(Id);

ALTER TABLE Materia ADD Creditos INT NOT NULL DEFAULT 3;


--SELECTS DE VERIFICACI�N
select * from Estudiante
select * from EstudianteMateria
select * from Materia
select * from Profesor
