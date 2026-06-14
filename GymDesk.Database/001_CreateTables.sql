-- 1. Таблица Clients (Клиенты)
CREATE TABLE IF NOT EXISTS Clients (
    Id SERIAL PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Phone VARCHAR(20),
    Email VARCHAR(150),
    RegistrationDate DATE DEFAULT CURRENT_DATE
);

-- 2. Таблица Trainers (Тренеры)
CREATE TABLE IF NOT EXISTS Trainers (
    Id SERIAL PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Specialization VARCHAR(200),
    Phone VARCHAR(20)
);

-- 3. Таблица Subscriptions (Абонементы)
CREATE TABLE IF NOT EXISTS Subscriptions (
    Id SERIAL PRIMARY KEY,
    ClientId INT NOT NULL REFERENCES Clients(Id) ON DELETE CASCADE,
    Type VARCHAR(50) NOT NULL CHECK (Type IN ('Месячный', 'Годовой', 'Разовый')),
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Price DECIMAL(10, 2) NOT NULL
);

-- 4. Таблица TrainingSessions (Записи на тренировки)
CREATE TABLE IF NOT EXISTS TrainingSessions (
    Id SERIAL PRIMARY KEY,
    ClientId INT NOT NULL REFERENCES Clients(Id) ON DELETE CASCADE,
    TrainerId INT NOT NULL REFERENCES Trainers(Id) ON DELETE CASCADE,
    SessionDate DATE NOT NULL,
    SessionTime TIME NOT NULL,
    Notes TEXT
);

-- Клиенты (5 записей)
INSERT INTO Clients (FirstName, LastName, Phone, Email) VALUES
('Иван', 'Петров', '+79001112233', 'ivan.petrov@mail.ru'),
('Анна', 'Сидорова', '+79004445566', 'anna.sidorova@gmail.com'),
('Дмитрий', 'Козлов', '+79007778899', 'dmitry.kozlov@yandex.ru'),
('Елена', 'Морозова', '+79001234567', 'elena.morozova@mail.ru'),
('Сергей', 'Волков', '+79009876543', 'sergey.volkov@gmail.com');

-- Тренеры (5 записей)
INSERT INTO Trainers (FirstName, LastName, Specialization, Phone) VALUES
('Алексей', 'Смирнов', 'Силовые тренировки, пауэрлифтинг', '+79001110000'),
('Мария', 'Кузнецова', 'Йога, пилатес, растяжка', '+79002220000'),
('Олег', 'Новиков', 'Кроссфит, функциональный тренинг', '+79003330000'),
('Татьяна', 'Соколова', 'Аэробика, зумба', '+79004440000'),
('Игорь', 'Лебедев', 'Бокс, единоборства', '+79005550000');

-- Абонементы (5 записей, привязаны к клиентам 1-5)
INSERT INTO Subscriptions (ClientId, Type, StartDate, EndDate, Price) VALUES
(1, 'Месячный', '2026-06-01', '2026-06-30', 3500.00),
(2, 'Годовой', '2026-01-01', '2026-12-31', 25000.00),
(3, 'Разовый', '2026-06-15', '2026-06-15', 800.00),
(4, 'Месячный', '2026-06-10', '2026-07-10', 3500.00),
(5, 'Годовой', '2026-03-01', '2027-02-28', 25000.00);

-- Записи на тренировки (5 записей)
INSERT INTO TrainingSessions (ClientId, TrainerId, SessionDate, SessionTime, Notes) VALUES
(1, 1, '2026-06-15', '18:00', 'Первая силовая тренировка'),
(2, 2, '2026-06-16', '10:00', 'Утренняя йога'),
(3, 3, '2026-06-17', '19:00', 'Кроссфит WOD'),
(4, 4, '2026-06-18', '17:00', 'Зумба вечерняя'),
(5, 5, '2026-06-19', '20:00', 'Спарринг по боксу');