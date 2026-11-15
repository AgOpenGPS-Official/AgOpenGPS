/*
 * AgOpenGPS Bluetooth GNSS bridge for ESP32
 *
 * Connects the rover RTK receiver (e.g. u-blox F9P, UART1) to the AgOpenGPS Android app over
 * Bluetooth Classic SPP, in both directions:
 *
 *   receiver --UART--> ESP32 --Bluetooth--> tablet   NMEA position (GGA, VTG, ...)
 *   receiver <--UART-- ESP32 <--Bluetooth-- tablet   RTCM corrections from the app's NTRIP client
 *
 * Board: a classic ESP32 (ESP32-WROOM-32 / ESP32 DevKit). The ESP32-S2/S3/C3/C6 have no
 * Bluetooth Classic and cannot run this. Arduino IDE: install "esp32 by Espressif Systems",
 * board "ESP32 Dev Module". See README.md next to this folder for wiring and receiver setup.
 */

#include <Arduino.h>
#include <BluetoothSerial.h>

#if !defined(CONFIG_BT_ENABLED) || !defined(CONFIG_BLUEDROID_ENABLED) || !defined(CONFIG_BT_SPP_ENABLED)
#error "Bluetooth Classic SPP is not available: use a classic ESP32 (not S2/S3/C3/C6) and board 'ESP32 Dev Module'."
#endif

// ----------------------------------------------------------------- settings

// Name shown in Android's Bluetooth list.
static const char *BT_NAME = "AgOpenGPS-GNSS";

// Receiver UART. Must match the receiver's UART1 baud rate (u-center: UART1 baud).
static const uint32_t GNSS_BAUD = 115200;
static const int GNSS_RX_PIN = 16;   // ESP32 RX2  <- receiver TX1
static const int GNSS_TX_PIN = 17;   // ESP32 TX2  -> receiver RX1

// On-board LED (GPIO2 on most DevKit boards). -1 disables it.
static const int LED_PIN = 2;

// Print link statistics on the USB serial port (115200 baud) every 5 s.
static const bool DEBUG_STATS = true;

// ----------------------------------------------------------------- implementation

static BluetoothSerial SerialBT;
static HardwareSerial GNSS(2);

static uint8_t buf[1024];
static uint32_t bytesToTablet = 0;    // NMEA sent to the tablet
static uint32_t bytesToReceiver = 0;  // RTCM sent to the receiver
static uint32_t bytesDropped = 0;     // receiver data while no tablet was connected
static uint32_t lastDataMs = 0;
static uint32_t lastStatsMs = 0;
static bool wasConnected = false;

static void updateLed(bool connected) {
  if (LED_PIN < 0) return;
  uint32_t now = millis();
  bool on;
  if (!connected) {
    on = (now % 1000) < 100;               // short blink: waiting for the tablet
  } else if (now - lastDataMs < 200) {
    on = (now % 100) < 50;                 // fast flicker: data flowing
  } else {
    on = true;                             // solid: connected, no data
  }
  digitalWrite(LED_PIN, on ? HIGH : LOW);
}

void setup() {
  Serial.begin(115200);

  if (LED_PIN >= 0) {
    pinMode(LED_PIN, OUTPUT);
    digitalWrite(LED_PIN, LOW);
  }

  // Room for one second of 10 Hz NMEA plus RTCM bursts. Must be set before begin().
  GNSS.setRxBufferSize(4096);
  GNSS.begin(GNSS_BAUD, SERIAL_8N1, GNSS_RX_PIN, GNSS_TX_PIN);

  if (!SerialBT.begin(BT_NAME)) {
    Serial.println("Bluetooth init failed");
    while (true) {
      if (LED_PIN >= 0) digitalWrite(LED_PIN, (millis() % 200) < 100);  // panic blink
      delay(10);
    }
  }

  Serial.printf("AgOpenGPS BT bridge ready: '%s', receiver UART %lu baud (RX %d, TX %d)\n",
                BT_NAME, (unsigned long)GNSS_BAUD, GNSS_RX_PIN, GNSS_TX_PIN);
}

void loop() {
  bool connected = SerialBT.hasClient();
  if (connected != wasConnected) {
    wasConnected = connected;
    Serial.println(connected ? "Tablet connected" : "Tablet disconnected");
  }

  // Receiver -> tablet.
  int avail = GNSS.available();
  if (avail > 0) {
    size_t n = GNSS.readBytes(buf, min((size_t)avail, sizeof(buf)));
    if (connected) {
      SerialBT.write(buf, n);
      bytesToTablet += n;
    } else {
      bytesDropped += n;  // drop, so the tablet gets fresh positions when it connects
    }
    lastDataMs = millis();
  }

  // Tablet -> receiver (RTCM corrections).
  avail = SerialBT.available();
  if (avail > 0) {
    size_t n = SerialBT.readBytes(buf, min((size_t)avail, sizeof(buf)));
    GNSS.write(buf, n);
    bytesToReceiver += n;
    lastDataMs = millis();
  }

  updateLed(connected);

  if (DEBUG_STATS && millis() - lastStatsMs >= 5000) {
    lastStatsMs = millis();
    Serial.printf("tablet:%s  to tablet %lu B  to receiver %lu B  dropped %lu B\n",
                  connected ? "yes" : "no", (unsigned long)bytesToTablet,
                  (unsigned long)bytesToReceiver, (unsigned long)bytesDropped);
  }

  if (avail <= 0) delay(1);  // idle: let the Bluetooth stack run
}
