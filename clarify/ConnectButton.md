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