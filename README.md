# **3DCameraGame**
## **概要**

・制限時間内に対象のオブジェクトを撮影してクリアを目指す

・このステージでの撮影対象は吸血鬼。街中を徘徊しているが、近づくと追いかけてくる。

・撮影対象に触れられるか、時間切れの場合はゲームオーバー

・特殊ギミックとして街中に落ちているアイテム(にんにく)を拾い撮影対象に投げることができるようになっている。

　→ぶつけると撮影対象が倒れるため、撮影しやすくなる。(ぶつけなくても撮影は可能)

プラットフォーム：Android 

アセット

プレイヤー(操作UI含む)：

一人称：Starter Assets - FirstPerson

三人称：Starter Assets - ThirdPerson

撮影対象(吸血鬼)：Vampire 1

ステージのグラフィック：Demo City By Versatile Studio

## **実装**

・視点切り替え 

・撮影(Shutter) 　判定方法はraycast

・アイテム収集

・アイテム投下

・制限時間 

・撮影枚数 

・タイトル画面 

・プレイヤーを見つけたら追跡 

・プレイヤーを見つけていない場合、マップ内徘徊

※その他の不足機能・画面などについては適宜、実装予定

## **画面概要**

### TitleScene

![image](https://github.com/user-attachments/assets/87682687-6328-45b1-b740-4d6e8c3ef249)

・画面真ん中のSTARTボタンでCamera3DGameSceneに画面遷移

※試しの画面のため後ほど改修予定

### Camera3DGameScene(実際のプレイ画面)

![image](https://github.com/user-attachments/assets/60f190f3-6596-41dc-a58a-c1ff77a10c09)

#### 操作
左下のジョイスティック：移動

![image](https://github.com/user-attachments/assets/d26ea497-d868-4fef-a29a-65d91ea98767)

右下のジョイスティック；視点移動

![image](https://github.com/user-attachments/assets/f1fc4688-82c2-45c2-8a58-f7a0a4e9ac80)

右下のボタン：

右上：ジャンプ

右下：ダッシュ

左上：視点切り替え(一人称or三人称)

左下：アクション(一人称のときのみ表示)※

※状態により機能が変わる

![image](https://github.com/user-attachments/assets/e3723e4e-683f-411f-abd0-357fec5345dc)

アクションの状態

アイテムなしの場合；撮影

![image](https://github.com/user-attachments/assets/2be5a773-5982-48e5-8db7-0d65af0aab70)

アイテムの近くにいる場合：アイテム収集

![image](https://github.com/user-attachments/assets/db583bf1-087d-468f-aedc-e42f95785703)

アイテムを拾った後の場合：アイテム投下

![image](https://github.com/user-attachments/assets/7f52eb85-809f-4b90-a16d-5892d83be6ca)

右上のボタン：ポーズボタン(ポーズ画面を開く。もう一度押すとポーズ画面を閉じる。)

![image](https://github.com/user-attachments/assets/f8d0813e-013d-4b9e-83c0-cc991093d9c0)

ポーズ、ゲームクリア、ゲームオーバー時の画面：

ポーズ

![image](https://github.com/user-attachments/assets/1174d088-945d-4e99-807e-7c5a582ad0e9)

ゲームクリア

![image](https://github.com/user-attachments/assets/3593d3f6-3097-4a6a-adbc-26684bf5bcd6)

ゲームオーバー

![image](https://github.com/user-attachments/assets/28d1c5d1-3679-4095-9442-1bcb34f6c97b)

RETRY：プレイをやり直す

END：TitleSceneに戻る
