void setup() {
  Serial.begin(115200);
  pinMode(14,INPUT); //UP
  pinMode(15,INPUT); //DOWN
  pinMode(16,INPUT); //RIGHT
  pinMode(17,INPUT); //LEFT
  delay(1000);
}

void loop() {
  int U = analogRead(14);
  int D = analogRead(15);
  int R = analogRead(16);
  int L = analogRead(17);
  String result = String(U) + "," + String(D) + "," + String(R) + "," + String(L) + ","; //U,D,R,L
  Serial.println(result);
  delay(100);
}
