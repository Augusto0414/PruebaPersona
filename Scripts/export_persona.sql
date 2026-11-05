BEGIN TRANSACTION;
CREATE TABLE Persona (
    Identificacion TEXT    NOT NULL PRIMARY KEY,
    Nombres        TEXT    NOT NULL,
    Apellidos      TEXT    NOT NULL,
    Edad           INTEGER NOT NULL CHECK (Edad BETWEEN 0 AND 150),
    Genero         TEXT    NOT NULL CHECK (Genero IN ('Masculino', 'Femenino'))
);
INSERT INTO "Persona" VALUES('1001','Ana María','García',28,'Femenino');
INSERT INTO "Persona" VALUES('1002','Carlos','Pérez',35,'Masculino');
INSERT INTO "Persona" VALUES('1003','Laura','Martínez',22,'Femenino');
INSERT INTO "Persona" VALUES('1004','Juan','Rojas',41,'Masculino');
CREATE VIEW vw_PersonasFemeninas AS
SELECT Identificacion, Nombres, Apellidos, Edad, Genero
FROM Persona
WHERE Genero = 'Femenino';
COMMIT;
