# PokeBid API

REST API do zarządzania aukcjami kart kolekcjonerskich, stworzone jako projekt zaliczeniowy.

## 1. Instrukcja uruchomienia systemu

1. Otwórz terminal w głównym folderze projektu.
2. Uruchom serwer komendą:
  `dotnet run`
3. Otwórz browzer i przejdź pod adres wygenerowany w terminalu z końcówką `/swagger`, aby testować API.

## 2. Opis architektury

Projekt został zrealizowany w oparciu o architekturę wielowarstwową, co zapewnia separację obaw oraz wysoką testowalność kodu. System składa się z następujących warstw:
* **Warstwa Prezentacji (Controllers):** Odpowiada za obsługę żądań HTTP (REST API), walidację wejścia poprzez DTO oraz zwracanie odpowiednich kodów statusu.
* **Warstwa Logiki Biznesowej (Services):** Zawiera główną logikę aplikacji (np. walidacja, czy nowa oferta jest wyższa od obecnej ceny, sprawdzanie czasu zakończenia aukcji).
* **Warstwa Dostępu do Danych (Repositories):** Abstrakcja nad bazą danych (wzorzec Repository).
* **Baza Danych:** Relacyjna baza danych SQLite, obsługiwana przez ORM Entity Framework Core (podejście Code-First).

## 3. Diagram Związków Encji (ERD)

**Relacje w systemie:**
* **User** (1) --- posiada ---> (Wiele) **Auctions** (One-to-Many)
* **Auction** (1) --- posiada ---> (Wiele) **Bids** (One-to-Many)
* **User** (1) --- składa ---> (Wiele) **Bids** (One-to-Many)

**Struktura Tabel:**
* **Users:** `Id` (PK), `Username`, `Email`, `PasswordHash`
* **Auctions:** `Id` (PK), `Title`, `Description`, `Category`, `Condition`, `StartingPrice`, `CurrentHighestBid`, `StartDate`, `EndDate`, `OwnerId` (FK)
* **Bids:** `Id` (PK), `Amount`, `CreatedAt`, `AuctionId` (FK), `UserId` (FK)

## 4. Opis endpointów

**Aukcje (AuctionsController):**
* `GET /api/Auctions` - Pobiera listę wszystkich aukcji (możliwość filtrowania np. `?category=Fire`).
* `GET /api/Auctions/{id}` - Pobiera szczegóły konkretnej aukcji po jej ID.
* `POST /api/Auctions` - Tworzy nową aukcję w systemie.
* `PUT /api/Auctions/{id}` - Całkowicie aktualizuje dane istniejącej aukcji.
* `DELETE /api/Auctions/{id}` - Usuwa aukcję z bazy danych.
* `POST /api/Auctions/{id}/bids` - Składa nową ofertę (licytacja). Zawiera walidację kwoty oraz czasu zakończenia aukcji.

**Użytkownicy (UsersController):**
* `GET /api/Users` - Pobiera listę użytkowników.
* `GET /api/Users/{id}` - Pobiera dane konkretnego użytkownika.
* `POST /api/Users` - Rejestruje nowego użytkownika.
* `DELETE /api/Users/{id}` - Usuwa użytkownika z systemu.
