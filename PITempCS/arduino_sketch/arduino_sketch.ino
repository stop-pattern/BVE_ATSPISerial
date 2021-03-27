// for SpeedometerDriveBoard rev.2
// this source si modded by stop-pattern@github
// original: saha209@github
// settings is below:
// board: ESP32 Dev Module

// setting
const int SPD = 19;
const int ledcChannel = 0;
const int ledcBaseFreq = 1000;
const int ledcTimerBit = 16;

void setup() {
  // serial setup
  Serial.begin(19200);
  while(!Serial) delay(1);
  Serial.println("Serial connected!");

  // pwm setup
  pinMode(SPD, OUTPUT);
  ledcSetup(ledcChannel, ledcBaseFreq, ledcTimerBit);
  ledcAttachPin(SPD, ledcChannel);
}

// 0-120の範囲で速度を受け取ってpwmで出力
void pwmWrite(int speed = 0) {
  speed = abs(speed);
  if(speed > 120) speed = 120;
  ledcWrite(0, pow(2, ledcTimerBit) * speed / 120);
}

void loop() {

  //速度計情報を要求
  Serial.print("ab\n");
  if (Serial.available()) {
    //速度計情報を受信
    String reads = Serial.readStringUntil('\n');
    if (reads.substring(0, 2) == "ab") {
      reads = reads.substring(2);
      int speed = reads.toInt(); 
      pwmWrite(speed);
    }
  }
  delay(100);
}

//ソースは第一閉塞進行様のホームページより改変
//http://sylph.lib.net/trainsim/buhin2_arduino.html
