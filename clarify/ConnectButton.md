<!-- use mermaid to draw flow chart-->
<!-- TB = Top To Button
     TD = Top to Down/same as top to bottom
     BT = Button To Top
     LR = Left to Right
     RL = Right to Left
-->
[mermaid](https://mermaid-js.github.io/mermaid/#/./flowchart?id=flowcharts-basic-syntax)

```mermaid;
flowchart TB;
st[Connect Button Start]
isconnect[Whether It Was Connect]
ed[Connect Button End]
st --> isconnect --> ed
```
```mermaid;
flowchart TB;
jw{jwReader.RFID_Open}
Y[Is Connect]
N[Not Connect]

jw --> |== 0| Y
jw --> |!= 0| N
```
DataColumn include("No.", "Tag_ID", "Last_Time")


```mermaid
flowchart TB;
cs[Connect success]
jwt[wReader.TagReported += TagReport]
jwg[jwReader.gpiEventReported += GPIEventReport]
btn_c[button_connect.Text = Disconnect]
btn_s[button_set.Enable = true]
eut[enter_update_thread = true]
Th[Thread create]
Thib[threadIsBackground = true]
Ths[Thread.Start]

cs-->jwt
jwt-->jwg
cs-->btn_c
btn_c-->btn_s
cs-->eut
cs-->Th
Th-->Thib
Thib-->Ths
```

```mermaid
flowchart TB;
st[Start Program]
cn[Connect To Receive Side]
gt[Get Data From sensor]

```

```mermaid
flowchart TB
```


