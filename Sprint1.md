# Sprint 1 Összefoglaló (M.A.D.E)

## Csapat felosztása
A projekt során a feladatok úgy lettek szétosztva a csapattagok között, hogy azok az egyéni érdeklődésekhez és korábbi tapasztalatokhoz a legjobban illeszkedjenek. Az architektúra modularitásának köszönhetően (mindenki saját branch-en, vagy scene-en dolgozott, pl. `Adrian`, `BalogDavid-AI`, `erik-interactions`) a csapat képes volt párhuzamosan fejleszteni, minimalizálva a kódütközéseket.

## Fő feladatkörök 
Bár a feladatok a következő sprintekben változhatnak, a **Sprint 1** során az alábbi fókuszköröket jelöltük ki:
- **Meilinger Marcell:** UI és Rendszer alapok (Kezdőképernyő, halál és szünet képernyők életerő menedzsment, főhős mozgása, történetmesélés), specifikáció és dokumentációk írása.
- **Balog Dávid:** Ellenséges Mesterséges Intelligencia (Enemy AI), harcrendszer szinkronizációja, boss harcok logikája, komponensek összekötése, összehagnolása
- **Patai Péter Erik:** Környezeti interakciók, tárgyfelvételi rendszerek (loot, érmék), UI visszajelzések és puzzle elemek (létra, teleporter, tolható ládák).
- **Rigó Imre Adrián:** Pályatervezés (Level Design), pályaelemek térbeli felépítése, parallax scrollozás és Tilemap kezelés.

## Kontribúció
A csapat tagjai aktívan hozzájárultak a projekt első mérföldkövéhez. Minden csapattag rendszeresen dokumentálta a haladását, és az előírt határidőre szállította a működő prototípushoz szükséges funkciókat.

---

## Sprint során elért eredmények 
A **Sprint 1** végeztével egy játszható, alapvető mechanikákban gazdag akció-platformer játék magját sikerült lerakni.  
- Kialakult a **harcrendszer** és a **játékos mozgás** fizikája (ugrás, létrán mászás).
- Elkészült a **Loot rendszer** és a **környezeti interakció** (kincsesládák nyitása, potion-ök, pénzgyűjtés).
- Sikerült bevezetni az elágazó **ellenséges AI** viselkedéseket a pályákra.
- Felépült az 1. és a kezdeti 2. pálya látványvilága (barlang és kastély témák).
- A fő UI elemek (Start menü, Pause menü, Death screen, HP sáv) működőképesek.

## Kontribuciók összefoglalása



### Patai Péter Erik
- Tolható ládák, vertikális mászás (LadderClimb), nyitható kapuk és pályák közötti teleporterek implementálása.
- Átfogó *Looting* mechanika: Nyitható kincsesládák, Potion-ök (HP, Damage, Poison efektek), UI érmeszámláló (`CoinUI`) programozása.
- A "Damage Power Up" vizuális aura effect elkészítése a főszereplőhöz.

### Balog Dávid
- Fejlett harci rendszerek elkészítése: Az alapvető ellenséges egységek és az első fázisos "Boss" mesterséges intelligencia fejlesztése.
- A harcrendszer támadási animációinak finomhangolása (pontatlan ütések javítása), valamint a karakter és az ellenfelek interakciójának letisztázása.

### Rigó Imre Adrián
- Pályák vizuális felerősítése, Tilemapok rajzolása az 1-es szintre, a barlangi ("cave") részre, és a legújabb `Pixel2DCastleTileset` asset csomag segítségével a második (kastély) pálya alapjaira.
- A térbeli mélységérzetet nyújtó **Parallax háttérscriptek** sikeres integrálása az utódpályákon és kamerakezelés.

### Meilinger Marcell
- Architektúra és projekt összeállítás. A karakter fő rendszereinek aszinkron szétválasztása (`WarriorHealth` és `WarriorMovement`).
- Történetmesélés, játékmenet bevezető, Death animációk összehangolása.
- Teljes menürendszer (Start Menu beállításokkal) és UI elemek leprogramozása.

---

## Akadályozók a feladatokban 
- **Merge Konfliktusok:** Ahogy egyre több mechanika épült ugyanarra a főhős-prefabre, a verziókezelőben konfliktusok alakultak ki az egyidejű Unity Scene szerkesztések miatt. Ezt idővel úgy hidaltuk át, hogy mindenki saját demo ("sandbox") scene-eket használt a fejlesztésre és git ágakat a biztonságos kódolvasztásra.
- **Unity Input System migráció:** Az Input rendszerre és animátor paraméterekre való rácsatlakozás esetenként okozott elcsúszásokat (pl. spammelhető támadások vagy ragadó animációk).
- **Megfelelő grafika (Asset Pack) megtalálása:** Viszonylag sok időt vitt el a megfelelő stílusú és licencű animált anyagok (pl. Pixel2DCastleTileset) felkutatása és beszabása a már rajzolt collision térképekbe.

## Mire van szükség a továbbhaladáshoz
1. **Pályák összeszerelése (Level Integration):** A külön kezelt tesztpályákat és asseteket egységes, fázisokra bontott történetté (level 1 -> level 2 -> level 3) kell összedrótozni.
2. **Kiegyensúlyozás (Balancing):** A megszerzett fegyvererősítők (Power-up) és aranyérmék jelenleg csak gyűlnek, a következő feladat ezek beválthatóságának (pl. Shop rendszer), és a Boss sebzés / játékos életerő egyensúlyának a belövése.
3. **Audio-Vizuális visszajelzések:** Több hanghatás (hangeffektek a kardcsapásokhoz, ugráshoz), vizuális részecskeszűrők és háttérzenék implementálása szükséges, hogy a játék élénkebb legyen.
4. **Hibafeltárás (Playtesting):** Az egyben lévő rendszer letesztelése majd a kritikus hibák kijelölése és kijavítása.
