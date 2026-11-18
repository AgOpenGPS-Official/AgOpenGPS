# AgOpenGPS za Android – Upute za korištenje

## Što vam treba

- Android tablet (Android 8 ili noviji) s Bluetoothom.
- RTK prijemnik (npr. u-blox F9P) spojen preko Bluetootha putem **ESP32** ili **Raspberry Pi** uređaja.
- Za RTK: korekcije s NTRIP poslužitelja (npr. vaše bazne stanice) i mobilni internet na tabletu.
- Za isprobavanje bez traktora koristite **simulator**.

## Prvo postavljanje

1. Uparite ESP32 ili Pi u Bluetooth postavkama Androida (**Postavke → Upari...** ih otvara).
2. Otvorite **Postavke**:
   - **Jezik**: Sustav, English ili Hrvatski. **Boje**: *Noć* (tamno) ili *Dan* (svijetlo, za jako sunce).
   - **Prijemnik**: odaberite upareni uređaj. *Automatski se spoji* spaja se pri svakom pokretanju.
   - **Priključak**: puna **širina priključka** i **preklop**. Prohodi su razmaknuti za širinu umanjenu za preklop.
   - **Vozilo**: položaj antene, mjereno od sredine stražnje osovine (lijevo i iza su negativni). Ako niste sigurni, ostavite 0.
   - **NTRIP**: poslužitelj, port, mountpoint, korisnik i lozinka te označite *Šalji NTRIP korekcije*.
3. Vratite se (**Natrag**) i pritisnite **Spoji**.

## Zaslon za vožnju

- **Oznaka** (gore lijevo): **RTK Fix** zeleno = centimetarska točnost, **RTK Float** narančasto = oko 30 cm, **GPS** = 1–2 m.
- **Svjetlosna traka** (gore): upaljena polja pokazuju na koju stranu i koliko upravljati. Upaljena samo sredina = na smjeru.
- **◀ 12 cm**: 12 cm ste desno od linije, upravljajte lijevo. Zeleno ispod 5 cm, žuto ispod 15 cm, crveno iznad.
- **Karta**: dva prsta ili **+ / −** za povećanje, povlačenje za pomicanje. **Prati** vraća na traktor, **Parcela** prikazuje cijelu parcelu.

## Parcele

- **Parcela: ...** (desna ploča) otvara popis: **Nova parcela**, otvori, preimenuj, obriši.
- **Uvezi iz datoteke** učitava obrise parcela (GeoJSON ili KML). **Izvezi** sprema parcele s međama i linijama u datoteku, kao sigurnosnu kopiju ili za drugi tablet.

### Snimanje međe

Pritisnite **Snimi među**, zatim odaberite:

- **Vožnjom uz rub**: obiđite parcelu s podignutim priključkom i pritisnite **Završi među** unutar 30 m od početka.
- **Kutovima (ravne stranice)**: odvezite se do svakog kuta i pritisnite **Kut** (**Poništi** briše zadnji). Nakon zadnjeg kuta pritisnite **Završi među**. Nije potrebno vraćati se na početak.

Zatim odaberite što prati rub: sredina priključka ili njegov lijevi ili desni kraj.

## Linije navođenja

1. Odaberite **Ravna** ili **Krivulja**.
2. Odvezite se na početak i pritisnite **A**.
3. Vozite uz rub (barem 5 m, što dulje to bolje) i pritisnite **B**.

Linija se sprema u otvorenu parcelu. Prohodi su s obje strane razmaknuti za radnu širinu. Plave oznake **A** i **B** pokazuju gdje je aktivna linija snimljena. **Linije...** prikazuje spremljene linije.

**Pomak**: **◀ / ▶** pomiču liniju za nekoliko cm, **Ovdje** je pomiče na trenutni položaj traktora, **Poništi pomak** je vraća.

## Obrađena površina

Pritisnite **Rad: UKLJ.** dok priključak radi i **Rad: ISKLJ.** na uvrtinama. Obrađeno tlo na karti postaje zeleno (preklop crveno). Ispod gumba: **obrađeno**, **preostalo**, **% gotovo** i **preklop** u hektarima. Sprema se za svaku parcelu.

## Uvrtine

**Postavke → Uvrtine**: unesite **širinu uvrtina** (npr. dvije širine priključka). Isprekidana linija na karti označava pojas uvrtina uz među; upozorenje *Uvrtine ispred* oglasi se prije nego što ih dosegnete.

Uz *Rad se isključuje na uvrtinama i ponovno uključuje u glavnom dijelu*, Rad se isključuje kad priključak uđe u uvrtine i ponovno uključuje kad se vrati u glavni dio (gumb u međuvremenu pokazuje **Rad: uvrtine**). To vrijedi samo nakon što ste sami uključili Rad; ručno isključivanje zaustavlja automatiku.

## Dnevnik poslova

Posao počinje s **Rad: UKLJ.** na parceli i bilježi datum, početak i kraj, radno vrijeme, obrađene hektare, prijeđeni put i prosječnu brzinu. Promjena parcele ili odspajanje ga završava. **Poslovi...** (ispod statistike) prikazuje poslove: **Izvoz...** ih sprema kao **CSV** (Excel) ili **PDF** (za ispis), **Završi posao** zatvara trenutni, a dodirom posla brišete ga.

## Zvučna upozorenja

**Postavke → Zvučna upozorenja**: rub parcele ispred, izlazak s parcele, RTK Fix izgubljen i ponovno tu, korekcije prestale, veza izgubljena, izvan linije. Svako se oglasi jednom po događaju, i kad je tablet utišan. **Probni zvuk** provjerava glasnoću.

## Satelitska karta

**Postavke → Satelitska karta**. Kvadrati karte preuzimaju se jednom i čuvaju na tabletu. **Preuzmi kartu za otvorenu parcelu** unaprijed preuzima cijelu parcelu (najbolje preko Wi-Fi mreže), pa karta radi i bez interneta.

## Sigurnosna kopija

**Postavke → Sigurnosna kopija → Spremi kopiju** sprema jednu datoteku sa svim postavkama i svim parcelama (međe i linije). **Vrati kopiju** na ovom ili drugom tabletu sve ponovno postavlja. Datoteka sadrži NTRIP lozinku: čuvajte je privatno.

## Simulator

**Postavke → Simulator → Koristi simulator**, zatim se vratite: virtualni traktor s RTK fiksom kreće na otvorenoj parceli. **− / +** za brzinu, **< | >** za upravljanje, **Auto** za praćenje linije, **Na parcelu** za skok na parcelu.

## Kad nešto ne radi

- **Nema pozicije / Bluetooth: spojeno, ali nema podataka**: prijemnik je isključen ili nije spojen na ESP32/Pi, ili se brzina prijenosa razlikuje.
- **RTK FLOAT / NEMA RTK**: nema korekcija. Provjerite internet i NTRIP postavke; gornja traka prikazuje stanje NTRIP-a.
- **Karta ostaje prazna**: nije bilo interneta kad se područje prvi put prikazalo. Preuzmite kartu parcele preko Wi-Fi mreže.
- **Linije su stalno pomaknute za isti iznos**: provjerite položaj antene u **Postavke → Vozilo** ili koristite **Ovdje**.
