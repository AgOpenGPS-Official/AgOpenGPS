# Instrucciones para Claude Code — Módulo VistaX en AgOpenGPS

## Objetivo

Integrar y mejorar el módulo **VistaX** (monitor de siembra en tiempo real) dentro de **AgOpenGPS**, embebido via CefSharp en WinForms. Este documento cubre la arquitectura existente, las mejoras pendientes de UI/UX, y las tareas de integración.

---

## Contexto del Proyecto

### AgOpenGPS (este repositorio)
- **Stack**: C# / .NET / WinForms
- **Solución**: `SourceCode/AgOpenGPS.sln`
- **Proyecto principal**: `SourceCode/GPS/AgOpenGPS.csproj`
- **Entry point**: `SourceCode/GPS/Program.cs`
- **Rama activa**: `feature/agro-parallel-integracion`

### Módulos AgroParallel
Todos los módulos de Agro Parallel viven dentro de:
```
SourceCode/GPS/AgroParallel/
├── Common/                  ← Componentes compartidos entre módulos
│   ├── FormAgroParallel.cs  ← Form base para modales AgroParallel
│   └── FormJsonEditor.cs   ← Editor JSON genérico
└── VistaX/                  ← Monitor de siembra (este módulo)
    ├── MqttClientWrapper.cs ← Cliente MQTT para recibir telemetría de nodos ESP32
    ├── SeedDataModels.cs    ← Modelos de datos (surcos, sensores, snapshots)
    ├── SeedMonitor.cs       ← Lógica de monitoreo (procesamiento de datos)
    ├── VistaXConfig.cs      ← Configuración del módulo (JSON-backed)
    ├── VistaXPanel.cs       ← UserControl con CefSharp ChromiumWebBrowser
    ├── VistaX_Integration.txt ← Notas de integración
    └── vistaX.json          ← Archivo de configuración persistente
```

Futuros módulos (QuantiX, FlowX, Storm, etc.) seguirán la misma estructura.

### VistaX — Servidor Node.js (proyecto externo)
- **Ubicación en disco**: `G:\VistaX\`
- **Stack**: Node.js + Express + Socket.IO + EJS + MQTT + Leaflet.js
- **Puerto por defecto**: 3001 (`.env`)
- **Entry point**: `server.js`
- **Frontend**: templates EJS en `views/` + assets en `public/`
- **Función**: monitoreo de siembra en tiempo real — pastillas por surco, mapa GPS con densidad, alertas de tapados

---

## Arquitectura de la Integración

```
┌──────────────────────────────────────────────────────┐
│                 AgOpenGPS (WinForms)                  │
│                                                       │
│  ┌──────────────┐    ┌─────────────────────────────┐  │
│  │  Paneles de   │    │  CefSharp ChromiumWebBrowser│  │
│  │  guía GPS,    │    │                             │  │
│  │  controles,   │    │  VistaX HTML (localhost:3001)│  │
│  │  secciones    │    │                             │  │
│  │              │    │  - Monitor pastillas         │  │
│  │              │    │  - Mapa de siembra           │  │
│  │              │    │  - Alertas                   │  │
│  └──────────────┘    └─────────────────────────────┘  │
│                                                       │
│  ┌─────────────────────────────────────────────────┐  │
│  │ AgIO (UDP/Serial) ← GPS, IMU, secciones          │  │
│  └─────────────────────────────────────────────────┘  │
└──────────────────────────────────────────────────────┘
         ↕ MQTT / UDP
┌──────────────────────────────────────────────────────┐
│              VistaX Server (Node.js :3001)             │
│  - Express + Socket.IO                                │
│  - MQTT client → broker local (sensores ESP32)        │
│  - Recibe GPS de AgOpenGPS por UDP                    │
└──────────────────────────────────────────────────────┘
         ↕ MQTT (1883)
┌──────────────────────────────────────────────────────┐
│         Nodos ESP32-S3 en la sembradora               │
│  - Pulsos de sensores de semilla (hasta 7 por nodo)   │
│  - Telemetría cada 250ms                              │
└──────────────────────────────────────────────────────┘
```

---

## Estado Actual del Sistema

La interfaz de VistaX ya está operativa con las siguientes características visibles:

### Header (barra superior)
- Logo **VistaX** + badge configuración (ej: "TAND: 43 SURCOS (2 TRENES)")
- KPIs en tiempo real: **VELOCIDAD** (km/h), **OBJETIVO** (semillas/m), **S/M PROM.** (promedio real), **FALLAS** (contador rojo), **ONLINE** (nodos conectados, ej: 44/44)
- Botones de herramientas y configuración

### Monitor de Surcos (zona principal)
- Dos secciones: **TREN (DELANTERO)** y **TREN (TRASERO)**
- Cada surco representado como un "tubo de ensayo" vertical con gradiente de color (azul→rosa)
- Indicadores LED por surco (verde = OK, rojo = falla)
- Toggle on/off por tren
- Selector de modo de vista

### Footer (barra inferior)
- Botón **SIN LOTE** / estado del lote activo
- Indicadores de módulos: **RPM 1**, **LIE 1**, **TOLVA 1**, **EJE 2**, **TOLVA 2**
- Cada módulo con su ícono, valor y estado (verde = activo)
- Texto central: "SISTEMA VISTAX OPERATIVO"
- Botones de configuración y audio

---

## Mejoras Pendientes de UI/UX

### 1. Centrado de sensores en el mapa (`SeedMonitor.cs`)
**Problema**: Los tubos de ensayo (sensores de siembra) no se centran correctamente cuando hay pocos surcos activos. Se desbordan o quedan desalineados respecto al área visible.

**Solución**:
- Calcular el ancho total de todos los sensores visibles + spacing
- Centrar el grupo horizontalmente dentro del contenedor del tren
- Aplicar clipping para evitar desbordes fuera del área del mapa
- Los sensores NO deben solaparse ni tapar elementos del mapa de guía GPS

### 2. Ventanas modales independientes (`FormAgroParallel.cs`, `FormJsonEditor.cs`)
**Problema**: Los popups/modales se abren embebidos dentro de la ventana principal, lo que interfiere con la visualización del mapa.

**Solución**:
- Todos los modales deben abrirse como **ventanas independientes** del sistema operativo
- En WinForms: usar `form.Show()` o `form.ShowDialog()` con `Owner` configurado, NO como controles embebidos
- Aplicar `FormBorderStyle.None` + bordes custom para mantener la estética AgroParallel

### 3. Eliminación de estilos nativos Windows
**Problema**: Bordes cuadrados, barras de título nativas y botones de control estándar de Windows rompen la estética del producto.

**Solución**:
- `FormBorderStyle = FormBorderStyle.None` en todos los forms AgroParallel
- Implementar barra de título custom con botones de cerrar/minimizar propios
- Bordes redondeados o sutiles via custom paint
- Sin chrome nativo de Windows en ninguna ventana del módulo

### 4. Unificación visual
**Requerimientos**:
- Aplicar el theme definido en `VistaXConfig.cs` / `vistaX.json` de forma consistente
- Fondo oscuro (#0a0a0a), acentos verdes (#00e676), alertas rojas (#ff1744)
- Tipografía legible para pantallas de tractor (7-12", baja resolución)
- Espacios equilibrados, transiciones suaves
- La estética debe coincidir con la imagen de referencia del monitor VistaX

### 5. Integración de datos en tiempo real
- `SeedDataModels.cs` debe alimentar correctamente posición y estado de cada sensor
- `MqttClientWrapper.cs` debe notificar cambios sin bloquear el renderizado del mapa
- Usar `BeginInvoke` / `InvokeRequired` para actualizar UI desde el hilo MQTT

---

## Configuración de Método de Inicio de Monitoreo

### Problema
El monitoreo parece iniciar tarde. Se necesita un sistema configurable de métodos de inicio.

### Métodos a implementar en `VistaXConfig.cs` / `vistaX.json`

```json
{
    "monitoreo": {
        "metodo_inicio": "sensores",
        "opciones": {
            "sensores": {
                "descripcion": "Inicia cuando se detecta caída de semilla en X sensores",
                "umbral_sensores_activos": 3,
                "tiempo_confirmacion_ms": 500
            },
            "herramienta": {
                "descripcion": "Inicia cuando baja la herramienta (señal digital)",
                "pin_o_mensaje": "sections_down",
                "debounce_ms": 200
            },
            "pintando": {
                "descripcion": "Inicia cuando AgOpenGPS indica que está pintando (secciones activas)",
                "fuente": "agopengps_sections"
            },
            "manual": {
                "descripcion": "Inicia al tocar botón en pantalla",
                "boton_ui": true
            }
        }
    }
}
```

### Implementación en `SeedMonitor.cs`

```csharp
public enum MetodoInicioMonitoreo
{
    Sensores,       // Por caída de semilla según cantidad de sensores activos
    Herramienta,    // Por bajada de herramienta (señal digital)
    Pintando,       // Por estado de "pintando" en AgOpenGPS (secciones activas)
    Manual          // Por toque en pantalla
}

// En SeedMonitor.cs
public void EvaluarInicio(SeedMonitorSnapshot snap)
{
    if (_monitoreoActivo) return;

    switch (_config.MetodoInicio)
    {
        case MetodoInicioMonitoreo.Sensores:
            int activos = snap.Surcos.Count(s => s.Flujo > 0);
            if (activos >= _config.UmbralSensoresActivos)
                IniciarMonitoreo();
            break;

        case MetodoInicioMonitoreo.Herramienta:
            if (snap.HerramientaBajada)
                IniciarMonitoreo();
            break;

        case MetodoInicioMonitoreo.Pintando:
            if (snap.SectionStates?.Any(s => s) == true)
                IniciarMonitoreo();
            break;

        case MetodoInicioMonitoreo.Manual:
            // Se inicia desde el botón en la UI
            break;
    }
}
```

### UI para selección de método
Agregar en la pantalla de configuración de VistaX (modal o sección en el footer):
- Selector con las 4 opciones
- Para "Sensores": slider o input numérico del umbral
- Indicador visual del estado actual (esperando inicio / monitoreando)
- El botón "SIN LOTE" en el footer podría cambiar a "▶ INICIAR" en modo manual

---

## Código Existente Clave

### VistaXPanel.cs — Comunicación C# → JavaScript
```csharp
// Patrón actual de inyección de datos al browser
public void UpdateDisplay(SeedMonitorSnapshot snap)
{
    if (!_isReady || snap == null || browser == null) return;
    string json = JsonSerializer.Serialize(snap);
    string script = $@"if(window.updateData) {{ window.updateData('{json}'); }}";
    browser.GetMainFrame().ExecuteJavaScriptAsync(script);
}

// Posicionamiento del panel (95% ancho, 180px alto, pegado abajo)
public void Reposition()
{
    this.Size = new Size((int)(this.Parent.Width * 0.95), 180);
    this.Location = new Point((this.Parent.Width - this.Width) / 2, 
                               this.Parent.Height - this.Height - 110);
    this.BringToFront();
}
```

### MqttClientWrapper.cs — Recepción de telemetría
- Se suscribe a `vistax/nodos/telemetria` y `vistax/nodos/registro`
- Parsea JSON de los nodos ESP32
- Notifica a `SeedMonitor.cs` con los datos procesados
- **Cuidado**: Las callbacks MQTT llegan en hilo separado → usar `Invoke` para UI

---

## Vocabulario del Dominio

| Término | Significado |
|---------|-------------|
| `bajada` | Punto de caída de semilla (= surco físico) |
| `cable` | Canal del sensor en ESP32 (0-6), se mapea a una bajada |
| `uid` | ID único del nodo ESP32 (derivado de EfuseMac) |
| `tren` | Grupo de bajadas (delantero/trasero). Actual: 2 trenes, 43+ surcos |
| `spm` | Semillas por metro — métrica principal de densidad |
| `flujo` | Pulsos por segundo del sensor |
| `raw` | Pulsos crudos del intervalo de telemetría (250ms) |
| `lote` | Sesión de grabación de siembra con GPS |
| `perfil` / `implemento` | Configuración de la sembradora (cantidad de bajadas, mapeo) |
| `tapado` | Sensor sin flujo — posible obstrucción del caño de bajada |
| `objetivo` | Densidad target de siembra (semillas/metro) configurada por operario |
| `pintando` | Estado de AgOpenGPS donde las secciones están activas (sembrando) |

---

## Archivos Clave de VistaX Server (G:\VistaX\)

| Archivo | Rol |
|---------|-----|
| `server.js` | Entry point — Express + Socket.IO + MQTT |
| `core/logic/mqtt_handler.js` | Procesa telemetría, mapea cables→bajadas, calcula spm |
| `core/logic/map_recorder.js` | Graba trayectoria GPS + densidad |
| `core/logic/state_engine.js` | Estado global acumulado |
| `views/index.ejs` | Monitor principal — pastillas/tubos de surcos |
| `views/mapa.ejs` | Mapa Leaflet con densidad |
| `public/js/render_engine.js` | Render de pastillas + alertas visuales |
| `public/js/config_modal.js` | Modal para configurar mapeo de sensores |
| `public/css/vistax.css` | Theme — variables CSS (`--accent`, `--danger`, etc.) |

---

## Dependencias

### NuGet (C#)
- **CefSharp.WinForms** — navegador Chromium embebido (ya instalado)
- **System.Text.Json** — serialización JSON

### CefSharp — Inicialización global
```csharp
// En Program.cs ANTES de Application.Run()
var settings = new CefSettings
{
    CachePath = Path.Combine(Environment.GetFolderPath(
        Environment.SpecialFolder.LocalApplicationData), "AgOpenGPS", "CefCache"),
    LogSeverity = LogSeverity.Disable
};
Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

// ... Application.Run(new FormGPS());

Cef.Shutdown();
```

### Node.js (VistaX server)
- `express`, `socket.io`, `mqtt`, `ejs`, `dotenv`
- Node.js debe estar instalado en la PC del operario

---

## Flujo de Datos Integrado

```
1. AgOpenGPS arranca → inicia proceso Node.js de VistaX (:3001)
2. CefSharp carga http://localhost:3001 (monitor VistaX)
3. MqttClientWrapper.cs se conecta al broker MQTT (:1883)
4. Nodos ESP32 en sembradora → MQTT → MqttClientWrapper + VistaX server
5. SeedMonitor.cs procesa datos → evalúa inicio de monitoreo
6. VistaXPanel.UpdateDisplay() inyecta snapshot al browser via JS
7. AgOpenGPS envía GPS y secciones → VistaX server
8. VistaX georeferencia y graba datos de siembra
9. Al cerrar AgOpenGPS → mata proceso Node.js de VistaX
```

---

## Checklist de Tareas

### Ya implementado ✅
- [x] `VistaXPanel.cs` con CefSharp ChromiumWebBrowser
- [x] `MqttClientWrapper.cs` para recepción de telemetría
- [x] `SeedDataModels.cs` con modelos de datos
- [x] `SeedMonitor.cs` con lógica de monitoreo
- [x] `VistaXConfig.cs` + `vistaX.json` para configuración
- [x] `FormAgroParallel.cs` como form base
- [x] Frontend VistaX operativo (monitor de surcos, 2 trenes, KPIs)

### Pendiente ⬜
- [ ] Centrar sensores/tubos en el mapa sin desbordes
- [ ] Modales como ventanas independientes del SO
- [ ] Eliminar estilos nativos Windows (chrome, bordes, barra título)
- [ ] Implementar método de inicio de monitoreo configurable (4 modos)
- [ ] Diagnosticar y corregir inicio tardío del monitoreo
- [ ] Puente UDP para GPS y secciones (AgOpenGPS → VistaX server)
- [ ] Exponer `window.updateData()` en frontend VistaX
- [ ] Conectar ciclo de vida completo (arranque/parada de Node.js)
- [ ] Testear con nodos ESP32 reales o simulador

---

## Consideraciones Técnicas

1. **CefSharp binarios**: ~200MB. Necesita `<CefSharpAnyCpuSupport>true</CefSharpAnyCpuSupport>` en `.csproj` si compila AnyCPU.
2. **Threading**: Callbacks MQTT llegan en hilo background → siempre `Invoke`/`BeginInvoke` para tocar UI.
3. **JSON escape**: Al inyectar JSON via `ExecuteJavaScriptAsync`, cuidado con comillas simples. Considerar Base64 encode como alternativa robusta.
4. **Puerto 3001**: Verificar libre antes de arrancar VistaX.
5. **MQTT Broker**: Mosquitto en `127.0.0.1:1883`.
6. **Node.js**: Verificar instalado (`where node`) al arrancar.
7. **Pantallas tractor**: 7-12", baja resolución, alto contraste, operación con guantes.
8. **Frame rate CefSharp**: Limitado a 30fps. Monitorear memoria (~200MB proceso Chromium).
9. **Inicio tardío**: Posible causa: el monitoreo espera demasiados ciclos de confirmación antes de activarse. Revisar los timers y umbrales en `SeedMonitor.cs`.
