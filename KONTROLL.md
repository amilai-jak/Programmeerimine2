# KONTROLL - kõik käsud, millega oma tööd ise üle kontrollida

Kõik käsud käivita repo juurest: `C:\Users\AJ\Programmeerimine2`

> NB! Kasuta teedel **kaldkriipsu** (`/`). Kui kopeerid kuskilt tagurpidi kaldkriipsuga
> tee, võib `\` ära kaduda ja tekib selline viga:
> `The provided file path does not exist: KooliProjekt.WebAPIKooliProjekt.WebAPI.csproj`

---

## 1. Kõik korraga (soovitatud)

```powershell
powershell -ExecutionPolicy Bypass -File .\verify-all.ps1
```

Kontrollib buildi + kõik 4 testiprojekti + teeb coverage raporti.
Exit code 0 = kõik OK, 1 = midagi kukkus läbi.

---

## 2. Testid ükshaaval

```powershell
# Rakenduse ühiktestid - 150 testi (16.01, 22.01, 23.01, 05.02, 06.02)
dotnet test KooliProjekt.Application.UnitTests/KooliProjekt.Application.UnitTests.csproj

# Integratsioonitestid - 44 testi (13.02, 19.02)
dotnet test KooliProjekt.IntegrationTests/KooliProjekt.IntegrationTests.csproj

# Windows Formsi presenteri testid - 9 testi (02.04)
dotnet test KooliProjekt.WindowsForms.UnitTests/KooliProjekt.WindowsForms.UnitTests.csproj

# WPF view modeli testid - 12 testi (17.04)
dotnet test KooliProjekt.WpfApplication.UnitTests/KooliProjekt.WpfApplication.UnitTests.csproj
```

Ühe testi nimi järgi:

```powershell
dotnet test KooliProjekt.Application.UnitTests/KooliProjekt.Application.UnitTests.csproj --filter "FullyQualifiedName~AssetTests"
```

---

## 3. Testide raport + coverage (12.02 ülesanne)

```powershell
cd KooliProjekt.Application.UnitTests
powershell -ExecutionPolicy Bypass -File .\run-tests.ps1
```

Raport: `KooliProjekt.Application.UnitTests/BuildReports`

Käsitsi, kui tahad XML-i ise vaadata:

```powershell
dotnet test KooliProjekt.Application.UnitTests/KooliProjekt.Application.UnitTests.csproj `
  --collect "XPlat Code Coverage" `
  --results-directory ./KooliProjekt.Application.UnitTests/BuildReports/UnitTests
```

---

## 4. Rakendused käima

**Järjekord on oluline: WebAPI peab jooksma enne kliente.**
Iga rida eraldi aknas (API aken jäta lahti).

```powershell
# 1) WebAPI  -> http://localhost:5086/swagger
dotnet run --project KooliProjekt.WebAPI/KooliProjekt.WebAPI.csproj --launch-profile http

# 2) Windows Forms (27.02, 19.03, 20.03, 26.03, 27.03)
dotnet run --project KooliProjekt.WindowsForms/KooliProjekt.WindowsForms.csproj

# 3) WPF (09.04 - 17.04)
dotnet run --project KooliProjekt.WpfApplication/KooliProjekt.WpfApplication.csproj

# 4) Blazor WASM (30.04, 07.05, 08.05) -> http://localhost:5258/assets
dotnet run --project KooliProjekt.BlazorWasm/KooliProjekt.BlazorWasm.csproj --launch-profile http
```

### Mida igas aknas vaadata

**WebAPI / Swagger** (`http://localhost:5086/swagger`)
Nelja kontrolleri all peavad olema List / Get / Save / Delete:
`Assets`, `AssetClasses`, `MonthlyStates`, `MonthlyHoldings`.

**Windows Forms**
- tabel täitub API-st (10 rida)
- rea valimine täidab paremal ID / Nimi / Ticker / Vara klass ID
- "Lisa uus" tühjendab väljad
- "Salvesta" salvestab, "Kustuta" küsib kinnitust

**WPF**
- sama, aga kõik kolm nuppu on `RelayCommand`-idega seotud
- muuda paremal tekstikasti - DataGridis peab lahter kohe muutuma
  (INotifyPropertyChanged)

**Blazor** (`http://localhost:5258/assets`)
- tabel andmetega, rea kohta Edit nupp, üleval Add new
- `/edit/1` - väljad täidetud vara andmetega
- kustuta Nimi ära ja vajuta Save -> peab tekkima punane teade
  "Vara nimi on kohustuslik"
- Add new -> täida väljad -> Save -> suunab tagasi loendisse, rida on juures

---

## 5. Windows Formsi tutorialid (26.02)

```powershell
dotnet run --project Tutorials/PictureViewer/PictureViewer.csproj
dotnet run --project Tutorials/MathQuiz/MathQuiz.csproj
dotnet run --project Tutorials/MatchingGame/MatchingGame.csproj
```

- PictureViewer: "Show a picture" avab failivaliku, "Stretch" venitab pildi
- MathQuiz: "Start the quiz" alustab 30 s taimerit, liitmistehted
- MatchingGame: 4x4 laual paaride leidmine

Need on .NET Frameworki tutorialid, mis on üle toodud .NET 8 peale -
sellel masinal .NET Frameworki targeting pack'i ei ole.

---

## 6. API käsitsi test (curl)

```bash
curl "http://localhost:5086/api/Assets/List?page=1&pageSize=5"
curl "http://localhost:5086/api/Assets/List?page=1&pageSize=5&name=Vanguard"
curl "http://localhost:5086/api/Assets/Get?id=1"

# vigane page -> peab olema 400
curl -o /dev/null -w "%{http_code}\n" "http://localhost:5086/api/Assets/List?page=0&pageSize=5"

# valideerimine -> peab olema 400 + propertyErrors
curl -X POST "http://localhost:5086/api/Assets/Save" \
  -H "Content-Type: application/json" \
  -d '{"id":0,"assetClassID":0,"name":"","ticker":"","isRealEstate":false}'
```

---

## 7. Andmebaas

WebAPI kasutab SQLite faili ja loob selle ise esimesel käivitamisel
(migratsioonid + seemneandmed).

```powershell
# andmebaasi lähtestamine
Remove-Item KooliProjekt.WebAPI/KooliProjekt.db -ErrorAction SilentlyContinue
Remove-Item KooliProjekt.WebAPI/KooliProjekt.db-shm -ErrorAction SilentlyContinue
Remove-Item KooliProjekt.WebAPI/KooliProjekt.db-wal -ErrorAction SilentlyContinue
```

Integratsioonitestid kasutavad eraldi faili `kooliprojekt-integration.db`.

---

## 8. Kui midagi ei tööta

| Probleem | Lahendus |
|---|---|
| `The provided file path does not exist: ...` | Kasuta `/` teel, mitte `\` |
| `Failed to bind to address ... address already in use` | Eelmine aken on veel lahti - sulge see või `netstat -ano \| findstr 5086` |
| Blazor näitab tühja tabelit | WebAPI ei jookse või on vale port. Kontrolli `http://localhost:5086/api/Assets/List?page=1&pageSize=2` |
| Blazor näitab "Andmete laadimine..." lõputult | Vaata brauseri konsooli (F12); enamasti on API maas |
| `dotnet build` kukub: fail on lukus | Mõni rakendus jookseb - sulge kõik aknad ja proovi uuesti |
| WinForms/WPF aken on tühi | API aken peab jooksma ENNE kliendi käivitamist |

---

## 9. Repo struktuur

```
KooliProjekt.sln
KooliProjekt.Application/                 # DTOd, feature'id, handlerid, validaatorid
KooliProjekt.WebAPI/                      # 4 kontrollerit + CORS
KooliProjekt.Application.UnitTests/       # 150 testi + run-tests.ps1
KooliProjekt.IntegrationTests/            # 44 testi
KooliProjekt.WindowsForms/                # + Api kaust, MVP
KooliProjekt.WindowsForms.UnitTests/      # 9 testi
KooliProjekt.WpfApplication/              # MVVM + RelayCommand
KooliProjekt.WpfApplication.UnitTests/    # 12 testi
KooliProjekt.BlazorWasm/                  # Assets.razor + Edit.razor
Tutorials/                                # PictureViewer, MathQuiz, MatchingGame
verify-all.ps1                            # kõik korraga
```
