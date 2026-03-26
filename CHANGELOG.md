# Changelog

Ez a dokumentum a **M.A.D.E** projekt legfontosabb frissítéseit (Pull Requesteit) és fejlesztéseit tartalmazza a `main` ágon végrehajtott történetük alapján. A részletek azt mutatják be, hogy az adott fejlesztési ágak milyen új funkciókat hoztak a projektbe.

---

## 2026. Március

### Új Asset Pack és Vizuális Elemek
- **PR #17** – *Márc. 22.*
  - A `Pixel2DCastleTileset` grafikai csomag integrálása a projektbe (várfalak, talaj, animált fények, hátterek). A harmadik, illetve a kettes pálya vizuális finomhangolása, valamint a `background_lvl2` prefab átszervezése.

### Pályatervezés és Környezet
- **PR #15** – *Márc. 13.*
  - A Level 2 pálya vizuális bővülése új dekorációs elemekkel és új prefabekkel a gazdagabb környezetért.

### Főmenü és Mesélés
- **PR #14** – *Márc. 12.*
  - Elkészült a játékmenet előtti bevezető képernyő. Tartalmaz egy új kezdőképernyőt, részletes beállítási lehetőségekkel (Start screen & settings). Bekerültek a játék indításakor látható történetmesélési (storytelling) narratívák, valamint Doxygen kód-dokumentációval lettek ellátva a legfontosabb scriptek.

### Ellenséges AI Javítások
- **PR #13** – *Márc. 8.*
  - Az ellenségek mozgás AI-jának logikai és vizuális javítása, illetve a Level 1 térképének kisebb finomhangolásai (Enemy movement fix).

### Környezeti Interakciók bővülése
- **PR #12** – *Márc. 7.*
  - Platforming kihívások: Tolható ládák (`Pushable Crate`) bevezetése. Továbbá a vertikális közlekedéshez megjelent a mászható létra (`Ladder`), elkészültek a nyitható kapuk (`Gate`), illetve teleporterek kerültek be a pályarészek közti vándorláshoz.

---

## 2026. Február

### Interakciók és Felhasználói Felület
- **PR #11** – *Feb. 27.*
  - Új Coin UI (Pénzszámláló) és kapcsolódó vizuális elemek megvalósítása a HUD-on. Bekerült a felvehető tartalmak vizuális feedbackje ("Damage Power Up Aura").

- **PR #10** – *Feb. 24.*
  - Rendszerszintű harci és mozgási hibajavítások. A harcos egyensúlyozása (Warrior movement fix) és az ellenfelek (enemies) támadási pontatlanságainak elhárítása.

### Karakter Irányítás Bővítés
- **PR #9** – *Feb. 20.*
  - Jelentős kódrefaktorálás a karakternél (Separated Health and Movement): A `WarriorHealth` és `Movement` folyamatok szétválasztása az átláthatóbb és fejleszthetőbb szerkezet érdekében.

### Level 1 Kialakítása
- **PR #8** – *Feb. 20.*
  - Az 1. pálya (lvl 1) struktúrájának véglegesítése és a felmerülő térképi hibák elhárítása.

### Loot Rendszer (Ládák és Italok)
- **PR #6** – *Feb. 17.*
  - A tárgyfelvételi mechanikák alapja! Elkészült a kincsesládák (2 típus) nyitási rendszere. Bekerült 3 különböző főzet (HP, Damage, Poison) képessége is, és sikeresen rajtolt a `PlayerWallet` script (játékos pénztárcája) a begyűjthető aranyakért.

### Barlang Pályarész (Cave)
- **PR #5** – *Feb. 15.*
  - Barlangi térképek frissítése, a zónák összekapcsolása, illetve kiegészítésként a halál animáció, Életerő (HP) sáv és a pause menü fúziója a főszereplőnél.

### Parallax Kamera és Térképi mélység
- **PR #3** – *Feb. 11.*
  - Parallax háttérscriptek implementálása a festői, több mélységű háttérérzetért. A zökkenőmentes kamera követés beállítása, és az első ellenséges egységek (Boss és normál) pályára helyezése tesztelés céljából.

### Kezdeti Változatok és Alapanimációk
- **PR #2** – *Feb. 6.*
  - A kezdetleges pályatervek (Tilemap próbák) elkészülte bemutatása, és Asset köteg betöltése.

- **PR #1** – *Feb. 6.*
  - Az első nagy mérföldkő! Alapvető karakter fizika, ugrás, kardcsapás (Attack 1), sérülés (Take Hit), illetve halál bevezetése. Az ellenség alapvető animációinak szinkronizálása a Unity játékmotor Animator állapotgépével.
