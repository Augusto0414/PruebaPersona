-- Script de referencia (la aplicación crea estos objetos automáticamente al iniciar).
-- Se ejecuta con cualquier cliente SQLite, por ejemplo: sqlite3 persona.db < Scripts/schema.sql

CREATE TABLE IF NOT EXISTS Persona (
    Identificacion TEXT    NOT NULL PRIMARY KEY,
    Nombres        TEXT    NOT NULL,
    Apellidos      TEXT    NOT NULL,
    Edad           INTEGER NOT NULL CHECK (Edad BETWEEN 0 AND 150),
    Genero         TEXT    NOT NULL CHECK (Genero IN ('Masculino', 'Femenino'))
);

CREATE VIEW IF NOT EXISTS vw_PersonasFemeninas AS
SELECT Identificacion, Nombres, Apellidos, Edad, Genero
FROM Persona
WHERE Genero = 'Femenino';

-- Pruebas manuales de cada operación
-- INSERT
INSERT INTO Persona (Identificacion, Nombres, Apellidos, Edad, Genero)
VALUES ('2001', 'Sofía', 'López', 30, 'Femenino');

-- SELECT por identificación
SELECT * FROM Persona WHERE Identificacion = '2001';

-- SELECT desde la vista (solo mujeres)
SELECT * FROM vw_PersonasFemeninas;

-- UPDATE (solo edad, filtrado por identificación)
UPDATE Persona SET Edad = 31 WHERE Identificacion = '2001';

-- DELETE por identificación
DELETE FROM Persona WHERE Identificacion = '2001';
