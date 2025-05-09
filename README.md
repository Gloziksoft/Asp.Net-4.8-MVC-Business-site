# ASP.NET 4.8 MVC Business Website

This repository contains a business-oriented website project built with ASP.NET 4.8 MVC.

---

## 📦 Requirements

- Visual Studio Community
- .NET Framework 4.8
- Windows OS

---

## 🛠️ Setup Instructions

### 1. Download the Project

Download or clone the repository to your computer.

---

### 2. Add the Validation Library

1. Open the solution file `www.gloziksoft.sk_2023.sln` in Visual Studio Community.
2. Go to the `dufeksoft.lib` folder.
3. Add the file `dufeksoft.lib.csproj` to your solution:
   - Right-click the solution → `Add` → `Existing Project...`
   - Browse to `dufeksoft.lib/dufeksoft.lib.csproj` and select it.
4. Right-click your main web project → `Add` → `Reference...`
5. In the `Projects` tab, check `dufeksoft.lib` and confirm.

> This library contains essential validation logic for the project to function correctly.

---

### 3. Build and Run

1. Build the solution in Visual Studio.
2. Press **F5** or click **Start** to run the project.

---

# 🧾 ASP.NET 4.8 MVC Firemná Webstránka (slovensky)

Tento repozitár obsahuje webový projekt orientovaný na firemnú prezentáciu, vytvorený v ASP.NET 4.8 MVC.

---

## 📦 Požiadavky

- Visual Studio Community
- .NET Framework 4.8
- Windows

---

## 🛠️ Inštalácia a spustenie

### 1. Stiahnutie projektu

Stiahni si celý priečinok projektu do svojho počítača.

---

### 2. Pridanie validačnej knižnice

1. Otvor súbor projekt `www.gloziksoft.sk_2023.sln` vo Visual Studio Community.
2. Prejdi do priečinka `dufeksoft.lib`.
3. Pridaj súbor `dufeksoft.lib.csproj` do projektu:
   - Pravým klikom na riešenie → `Add` → `Existing Project...`
   - Vyber `dufeksoft.lib/dufeksoft.lib.csproj`.
4. Pravým klikom na hlavný web projekt → `Add` → `Reference...`
5. V záložke `Projects` zaškrtni `dufeksoft.lib` a potvrď.

> Táto knižnica obsahuje dôležité validačné logiky potrebné pre správne fungovanie projektu.

---

### 3. Build a spustenie

1. Projekt zostav vo Visual Studiu.
2. Stlač **F5** alebo klikni na **Start**, aby sa projekt spustil.

---

## 📁 Štruktúra priečinkov

- `dufeksoft.lib/` – pomocná knižnica s validáciami
- `www.gloziksoft.sk_2023/` – hlavný webový projekt