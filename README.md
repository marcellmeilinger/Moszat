# M.A.D.E

![Project Status](https://img.shields.io/badge/status-active-success.svg)
![Unity](https://img.shields.io/badge/unity-2022.3%2B-black?logo=unity)
![Platform](https://img.shields.io/badge/platform-Windows-blue)

**M.A.D.E** egy 2D Action-Platformer játék, amely a Unity játékmotor legújabb Universal Render Pipeline (URP) alapú megvilágítási rendszerére épül. A vizuális megjelenés a klasszikus 2D Pixel Art stílust követi, professzionálisan animált assetekkel életre keltve.

## 📖 Tartalomjegyzék
- [A Játékról](#-a-játékról)
  - [Játékmenet](#játékmenet)
  - [Harcrendszer](#harcrendszer)
  - [Interakciók és Feladványok](#interakciók-és-feladványok)
- [✨ Funkciók](#-funkciók)
- [🎮 Irányítás](#-irányítás)
- [🛠️ Felhasznált Technológiák](#️-felhasznált-technológiák)
- [🏗️ Architektúra](#️-architektúra)
- [🚀 Telepítés és Futtatás](#-telepítés-és-futtatás)

## 🕹️ A Játékról

### Játékmenet
A játék során egy fegyveres harcos karakter bőrébe bújsz, kinek célja, hogy végigküzdje magát a lineárisan felépülő pályákon (level1, level2, level3). A platforming elemek (ugrálás, akadályok kerülgetése) mellett a játék szívét a pörgős közelharci rendszer adja. 
A pályákon fellelhető elszórt kincsesládák, a legyőzött ellenfelektől kapott aranyérmék és a különféle stat-növelő vagy gyógyító italok (HP Potion, Damage, Poison) mind arra ösztönzik a játékost, hogy alaposan feltérképezze a környezetét.

### Harcrendszer
A harcrendszer feszes és fókuszált. A támadás során egy rövid, 0.6 másodperces várakozási idő (cooldown) biztosítja a taktikus harcot, megakadályozva a támadások „spammelését”. A karakter támadás közben egy pillanatra megtorpan, ezzel átadva az ütés súlyát.

### Interakciók és Feladványok
A világ tele van interaktív elemekkel:
- **Tolható tárgyak**: Utat nyithatsz bizonyos akadályok vízszintes eltolásával.
- **Kapuk és Teleporterek**: A pályarészek közötti logikai és azonnali mozgást biztosítanak.
- **Létrán mászás**: Vertikális felfedezést tesz lehetővé, kikapcsolva a gravitációt a mozgás idejére.

## ✨ Funkciók

- **Dinamikus 2D Platforming**: Gördülékeny mozgás fizikára (Rigidbody2D) építve.
- **Új Unity Input System**: Teljeskörű billentyűzet és kontroller támogatás.
- **Moduláris Kód architektúra**: Egyfeladatú komponensekre bontott felépítés az átláthatóságért.
- **UI és Menürendszer**: Beépített főmenü, Pause panel, Sound mixer és Death Screen funkciók.
- **Pályatervezés**: 2D Tilemap grid és `Parallax` háttér a mélységérzetért.

## 🎮 Irányítás

Az irányítás a Unity új Input rendszerén (Input System) keresztül valósul meg.

| Akció | Gomb / Input |
| :--- | :--- |
| **Mozgás** | Nyílbillentyűk / WASD |
| **Ugrás** | Space |
| **Támadás** | Bal egérgomb / Dedikált támadás gomb |
| **Interakció** *(Kapuk, Ládák stb.)* | `E` |
| **Tárgyak felvétele** *(Looting)* | `F` |
| **Létrán mászás** | Fel / Le (Vertical input) |
| **Szünet (Pause)** | Escape (Esc) |

## 🛠️ Felhasznált Technológiák

A játék fejlesztéséhez a következő eszközök és környezetek lettek felhasználva:
- **Játékmotor**: Unity Editor (C#)
- **Fejlesztői környezet**: VSCode (Unity kiegészítőkkel), .NET SDK
- **Grafikai Renderelés**: Universal Render Pipeline (URP)
- **Rendszerek**: Unity Input System, TextMesh Pro, Unity Test Framework
- **Diagramok**: PlantUML
- **Dokumentáció**: Doxygen, Markdown

## 🏗️ Architektúra

A projekt a *felelősség szétválasztásának (Separation of Concerns)* elvére épít.
Az óriás-scriptek helyett specializált komponensek működnek együtt, például:
- A hős irányításáért a `WarriorMovement`, `WarriorHealth`, `PlayerInteractions` és `PlayerWallet` felelnek.
- Az ellenséges intelligenciát az `EnemyAI` és annak leszármazottai (pl. `ShieldAI`, `BossCharged`) szabályozzák.
- A környezeti elemek az `IInteractable` interfészt használják (`Chests`, `LootItem`, `GateInteraction` stb.).

## 🚀 Telepítés és Futtatás

1. Klónozd a tárolót:
   ```bash
   git clone [repository url]
   ```
2. Nyisd meg a projektet a **Unity Hub**-on keresztül. Győződj meg róla, hogy a megfelelő Unity verziót használod (2022.3+ ajánlott a támogatott csomagok miatt).
3. A szerkesztő betöltődése után nyisd meg a `main` vagy `StartMenu` Scene-t.
4. Nyomj a Play (▶️) gombra a játék indításához!