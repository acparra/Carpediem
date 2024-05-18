DROP DATABASE if exists Carpediem;
CREATE DATABASE Carpediem;
USE Carpediem;

-- Table roles
CREATE TABLE roles(
    id INT PRIMARY key AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL
);

INSERT INTO roles (name) VALUES ('Administrador');
INSERT INTO roles (name) VALUES ('Psicologo');
INSERT INTO roles (name) VALUES ('Acudiente');

-- Table users
CREATE TABLE users (
    id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(50) UNIQUE,
    password VARCHAR(50) NOT NULL,
    rol_id INT NOT NULL,
    FOREIGN KEY (rol_id) REFERENCES roles(id)
);

insert into users (username, password, rol_id) values ('admin', 'admin', 1);
insert into users (username, password, rol_id) values ('psicologo', 'psicologo', 2);
insert into users (username, password, rol_id) values ('acudiente', 'acudiente', 3);

-- Table document_type
CREATE TABLE document_type (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL
);

INSERT INTO document_type (name) VALUES ('Cedula de ciudadania');
INSERT INTO document_type (name) VALUES ('Tarjeta de identidad');

-- Table announcements
CREATE TABLE announcements (
    id INT PRIMARY key AUTO_INCREMENT,
    title VARCHAR(100) NOT NULL,
    description TEXT NOT NULL,
    datetime DATETIME NOT NULL,
    image BLOB,
    user_id INT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users(id)
);

INSERT INTO announcements (title, description, datetime, user_id) VALUES ('Anuncio 1', 'Descripcion del anuncio 1', '2021-10-10 10:00:00', 1);
INSERT INTO announcements (title, description, datetime, user_id) VALUES ('Anuncio 2', 'Descripcion del anuncio 2', '2021-10-10 10:00:00', 1);

-- Table persons
CREATE TABLE persons (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    sex BIT NOT NULL,
    eps VARCHAR(50) NOT NULL,
    email VARCHAR(100) NOT NULL,
    phone VARCHAR(20) NOT NULL,
    document_number VARCHAR(20) NOT NULL,
    document_type_id INT NOT NULL,
    user_id INT,
    FOREIGN KEY (document_type_id) REFERENCES document_type(id),
    FOREIGN KEY (user_id) REFERENCES users(id)
);

INSERT INTO persons (name, last_name, sex, eps, email, phone, document_number, document_type_id, user_id) VALUES ('Juan', 'Perez', 0, 'Sura', 'juanperez@gmail.com', '1234567', '1234567', 1, null);
INSERT INTO persons (name, last_name, sex, eps, email, phone, document_number, document_type_id, user_id) VALUES ('Maria', 'Gomez', 1, 'Colmedica', 'mariagomez@gmail.com', '7654321', '7654321', 1, 2);

-- Table guardians
CREATE TABLE guardians (
    id INT PRIMARY KEY AUTO_INCREMENT,
    person_id INT NOT NULL,
    FOREIGN KEY (person_id) REFERENCES persons(id)
);

insert into guardians (person_id) values (2);

-- Table patients
CREATE TABLE patients (
    id INT PRIMARY KEY AUTO_INCREMENT,
    person_id INT NOT NULL,
    guardian_id INT NOT NULL,
    FOREIGN KEY (person_id) REFERENCES persons(id),
    FOREIGN KEY (guardian_id) REFERENCES guardians(id)
);

insert into patients (person_id, guardian_id) values (1, 1);

-- Table psychologists
CREATE TABLE psychologists (
    id INT PRIMARY KEY AUTO_INCREMENT,
    person_id INT NOT NULL,
    arl VARCHAR(50) NOT NULL,
    FOREIGN KEY (person_id) REFERENCES persons(id)
);

insert into psychologists (person_id, arl) values (2, 'Sura');

-- Table appointments
CREATE TABLE appointments (
    id INT PRIMARY KEY AUTO_INCREMENT,
    title VARCHAR(100) NOT NULL,
    description TEXT NOT NULL,
    datetime DATETIME NOT NULL,
    note TEXT NOT NULL,
    psychologist_id INT NOT NULL,
    patient_id INT NOT NULL,
    FOREIGN KEY (psychologist_id) REFERENCES psychologists(id),
    FOREIGN KEY (patient_id) REFERENCES patients(id)
);

INSERT INTO appointments (title, description, datetime, note, psychologist_id, patient_id) VALUES ('Cita 1', 'Descripcion de la cita 1', '2021-10-10 10:00:00', 'Nota de la cita 1', 1, 1);