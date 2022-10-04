# Sprinkles(2021.03 ~ 2022.02)
![1](https://user-images.githubusercontent.com/97658764/193949440-0babfe89-4303-4729-8e5c-3271dcc4977a.PNG)
[Google PlayStore Link](https://play.google.com/store/apps/details?id=mobile.silverliningstudio.sprinkles)

## 프로젝트 내용
* 3D 맵 위에서의 땅따먹기 게임.
* 곳곳에 뿌려져 있는 스푼 아이템을 먹어 얻을 수 있는 슈팅 아이템 스쿱으로 상대방을 공격하는 동시에 자신의 땅을 넓힐 수 있음.
* 환경: Unity 2019.3.0a8
* 사용 라이브러리: Photon 2
---
# Game Flow
![Sprinkles GameFlow Diagram](https://user-images.githubusercontent.com/97658764/193947163-b6093a98-07cb-4931-a1a4-3027d4008543.png)

1. 게임의 각 단계를 파악하는 [GameManager](https://github.com/Luci-Park/TileFlippingGame/blob/master/Assets/Scripts/System/InGame/System/GameManager.cs)
2. 시간으로 나뉘는 단계의 지속시간을 조절하는[Timer](https://github.com/Luci-Park/TileFlippingGame/tree/master/Assets/Scripts/System/InGame/Timer)

---
# 3D 타일 중심 액션
![Communication Diagram](https://user-images.githubusercontent.com/97658764/193948168-be85271b-4fef-4f8d-b09e-248b5d006ac4.png)
1. 게임씬에 처음 로드 될 때 생성되는 [Player](https://github.com/Luci-Park/TileFlippingGame/tree/master/Assets/Scripts/Player)
2. Player가 쏘는 [Scoop](https://github.com/Luci-Park/TileFlippingGame/tree/master/Assets/Scripts/Scoops/Scoop)
3. 이웃 타일의 정보를 알고 이를 토대로 도착지를 지정해주는 [https://github.com/Luci-Park/TileFlippingGame/tree/master/Assets/Scripts/Map/Tile)
