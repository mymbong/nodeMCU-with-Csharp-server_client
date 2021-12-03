/*
    This sketch sends a string to a TCP server, and prints a one-line response.
    You must run a TCP server in your local network.
    For example, on Linux you can use this command: nc -v -l 3000
*/

/* NOTE: THIS CODE IS FROM EXAMPLE OF ESP8266WIFI LIBRARY
   I HAVE SOME ADDTIONAL CODE FOR PROJECT REQUIREMENT OR 
   CONCEPT
*/

#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>

#ifndef STASSID
#define STASSID "your ssid"
#define STAPSK  "your wifi password"
#endif

#define IN1 D0

const char* ssid     = STASSID;
const char* password = STAPSK;

const char* host = "192.168.xx.xx"; // your ipv4 IP
const uint16_t port = 1000;

bool IN1_stat;
String nodeName = "node5"; // change according to your need/requirement
String space = " ";

ESP8266WiFiMulti WiFiMulti;

void setup() {
  Serial.begin(115200);

  pinMode(IN1, INPUT_PULLUP);

  // We start by connecting to a WiFi network
  WiFi.mode(WIFI_STA);
  WiFiMulti.addAP(ssid, password);

  Serial.println();
  Serial.println();
  Serial.print("Wait for WiFi... ");

  while (WiFiMulti.run() != WL_CONNECTED) {
    Serial.print(".");
    delay(500);
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());

  delay(500);
}


void loop() {
  Serial.print("connecting to ");
  Serial.print(host);
  Serial.print(':');
  Serial.println(port);

  // Use WiFiClient class to create TCP connections
  WiFiClient client;

  if (!client.connect(host, port)) {
    Serial.println("connection failed");
    Serial.println("wait 5 sec...");
    delay(5000);
    return;
  }

  // This will send a string to the server
  Serial.println("sending data to server");
  if (client.connected()) {
      IN1_stat = digitalRead(IN1);
    
    client.printf("%s%s%s", nodeName, space, (IN1_stat ? "true" : "false"));
  }

  //read back one line from server
  Serial.println("receiving from remote server");
  String line = client.readStringUntil('\r');
  Serial.println(line);

  Serial.println("closing connection");
  client.stop();

  Serial.println("wait 5 sec...");
  delay(5000);
}
