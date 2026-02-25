#include <Arduino.h>
#include <SPI.h>
#include <Ethernet.h>
#include "MgsModbus.h"
#include <Servo.h>

MgsModbus Mb;
// Ethernet settings (depending on MAC and Local network) 
byte mac[] = {0x90, 0xA2, 0xDA, 0x0E, 0x94, 0xB5}; 
IPAddress ip (192, 168, 5, 120); // IP modbus client 
IPAddress gateway (192, 168, 5, 3); // IP PLC
IPAddress subnet (255, 255, 255, 0);

Servo myServo;

void setup()
{
  Serial.begin(9600);
  Ethernet.begin(mac, ip, gateway, subnet); // start etehrnet interface

  // Kiểm tra xem địa chỉ IP được gán có hợp lệ không
  if (Ethernet.localIP() == INADDR_NONE) { 
    Serial.println("Failed to configure Ethernet");
    while (true) {
      // Vòng lặp vô hạn để báo lỗi (có thể thay bằng tín hiệu LED nhấp nháy)
    }
  } else {
    Serial.println("Ethernet configured successfully");
    Serial.print("Assigned IP: ");
    Serial.println(Ethernet.localIP()); // In địa chỉ IP được gán
  }

  myServo.attach(5);

  Mb.MbData [0] = 0; // thanh ghi 4001 
  Mb.MbData [1] = 0; // thanh ghi 4002
  Mb.MbData [2] = 0; // thanh ghi 4003
  Mb.MbData [3] = 0; // thanh ghi 4004
  Mb.MbData [4] = 0; // thanh ghi 4005
}
void loop()
{
  // Đọc giá trị thanh ghi 4002 và điều khiển servo dựa trên giá trị đó
  int angle = Mb.MbData[0]; // Lấy giá trị thanh ghi 4001 để điều khiển góc servo
  Serial.println(angle);
  if (angle == 0) {
    myServo.write(0); // Quay servo 0 độ
  } else if (angle == 90) {
    myServo.write(90); // Quay servo 90 độđộ
  } else if (angle == 135) {
    myServo.write(135); // Quay servo 135 độ
  } else if (angle == 180) {
    myServo.write(180); // Quay servo 180 độ
  }
  Mb.MbsRun();
}
