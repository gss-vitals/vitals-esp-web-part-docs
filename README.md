# WebPart 開發手冊

---

## 什麼是WebPart
* 可以拖拉順序自訂版面，每一格視窗都是獨立內容,所以可以自行定義與製作
* 設定獨立區塊的各種設定，也可以自行定義設定內容

---

## 開發流程
![developmentProcess.png](/img/developmentProcess.png)
1. [**製作客製WebPart元件流程**](#製作客製WebPart元件流程)
2. **佈署dll**：在`{ESP安裝目錄}\data\config\webpart\implement\`目錄下，貼上步驟1的產生的客製WebPart dll檔(pdb檔可不貼)
3. **添加客製WebPart設定**：在`{ESP安裝目錄}\data\config\webpart\WebPartManageConfig.xml`檔案中，加入[對應的XML設定](#WebPart的XML檔設定)，並重新啟動IIS
    * 如有新增修改刪除此取XML檔的設定，則要重新啟動IIS
    * WebPart的主要設定跟參數都可以透過這個檔案進行更改
4. **確認是否需要修改webpartservice的共用內容**：在`{ESP安裝目錄}\web\webpart\`目錄下，cs，js，css檔案的修改將會影響WebPart的呈現

---

## 製作客製WebPart元件流程
1. 以Visual Studio新增專案，選擇 **`類別庫(.NET Framework)`** 專案，下一頁的架構如果開發ESP版本為6.2請選擇 **.NETFramework4.5**
2. 在方案總管的解決方案按右鍵新增項目，新增類別，這就是客製WebPart的類別，名稱建議不要與專案相同，之後在設定時才較好區分。
    * 補充：自動產生的class1.cs可以刪除
3. 在方案總管的參考按右鍵加入參考，選擇瀏覽，到`{ESP安裝目錄}\web\Bin\`目錄下，加入 **`GSS.Core.WebPart.dll`** 和 **`GSS.Core.WebPart.Interface.dll`** 兩個元件
    * 上述兩個元件至少需要添加的，如果開發過程中有用到其他ESP的元件可以繼續添加
4. 讓此類別繼承**BaseWebPart**，並至少實作以下3個function：
    * RenderTitle：用於返回建立時顯示的標題，通常讀取[XML設定](#WebPart的XML檔設定)的ConfigSettingList
    * RenderDescription：用於返回標題後的補充描述
    * RenderContent：用於返回WebPart的HTML字串的內容，最後會透過webpart.css和webpart.js渲染
5. 實作過程中取得設定的方法請參考[通用類別](#通用類別)章節，另外也可以使用BaseWebPart(被繼承類別)裡的方法
6. 在方案總管的解決方案按右鍵建置方案，客製的dll元件會被輸出在專案位置的Bin目錄下

---

## WebPart的XML檔設定
範例：
```xml
<WebParts>
    ......
  <WebPart name="WebpartTestPractice" type="WebpartTest.WebpartTestPractice" status="On">
    <SettingList>
      <Setting name="TopN" value="5" />
      <setting name="Folder" value="" />
    </SettingList>
    <ConfigSettingList>
      <ConfigSetting key="Title" value="區塊類別名稱" />    
    </ConfigSettingList>
  </WebPart>
</WebParts>
```
1. WebPart標籤
    * name：WebPart的唯一識別，為WebPart的CategoryName，僅可使用英文，避免特殊字元
    * type：輸入使用的dll，字串組合為namespace.classname
    * status：Webpart是否啟用，可填`On`或`Off`
2. SettingList區塊：此區域設定為WebPart設定選項
    * Setting中的name：設定的key值
    * Setting中的value：設定的預設值
    * 下圖紅字為內建設定選項的key值，直接設定就會在設定清單產生控制項
    ![buildInWebPartSettings.png](/img/buildInWebPartSettings.png)
    ![webPartSettingRangeControlValue.png](/img/webPartSettingRangeControlValue.png)
    * 如果dll有實作[RenderSettings](#RenderSettings實作範例)，那內建的key值就必須自行實作
3. ConfigSettingList區塊：此區域設定為自行定義的參數值，主要提供dll開發時使用

---

## 通用類別
### WebPartSettingList
* 取得WebPart的設定內容，即XML檔設定的SettingList區塊
* Get：取得SettingList設定
* Exist：判斷SettingList是否存在此設定
* add：新增SettingList設定
### ConfigSettingList
* 取得WebPart的參數內容，即XML檔設定的ConfigSettingList區塊
* Get：取得ConfigSettingList設定
* Exist：判斷ConfigSettingList是否存在此設定
* add：新增ConfigSettingList設定
### SettingsHelper
* 宣告：參數為`BaseWebPart的CategoryName`
* SettingsHelper.GetSettingValue：取得SettingList設定，能指定預設值
* CreateXXX：輔助[RenderSettings](#RenderSettings實作範例)方法畫出單行文字輸入框、下拉式選單、時間格式輸入框等HTML字串
### WebPartHtmlElement
* 裝填`Dropdown List`、`Checkbox`、`Radio Button`類型的物件
* 可被Add到`WebPartHtmlElementList`內

## RenderSettings實作範例
* 功能：根據WebPartSetting的參數將欄位渲染到畫面上
![renderSettingsCode.jpg](/img/renderSettingsCode.jpg)
![renderSettingsDemo.jpg](/img/renderSettingsDemo.jpg)

---

## 偵錯模式開發
1. 在Visaul Studio選擇偵錯->附加至處理程序，找w3wp.exe按附加開啟ESP站台，會同樣建立解決方案專案
2. 在檔案總管的解決方案按右鍵加入新增專案，而非新開一個Visual Studio
3. 接續[**製作客製WebPart元件流程**](#製作客製WebPart元件流程)的前3步驟
4. 在新增的客製WebPart專案按右鍵屬性->建置事件->建置後事件，加入以下命令：
```bash
copy /y "$(ProjectDir)$(OutDir)專案名稱.dll" "ESP安裝目錄\data\config\webpart\implement"
```
6. 在**ESP站台的專案**的參考按右鍵加入參考，選擇專案，找到剛剛建立的專案按打勾
7. 在Visaul Studio選擇偵錯->附加至處理程序，勾選顯示所有使用者的處理序，找到w3wp.exe，按附加，進入Debug模式
    * 出現兩個w3wp.exe時，選擇使用者名稱為NETWORK SERVICE者
8. 編輯過客製WebPart專案按下存檔後，到方案總管把此專案按右鍵重建，修改過的程式就同樣能進入Debug模式，也能下中斷點了
    #### 註
    * 第4步驟會讓專案每次重建自動部屬dll，實際部屬時記得改為Release模式再編譯一次
    * 如有新增修改刪除XML檔的設定，還是要重新啟動IIS
