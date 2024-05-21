# 專案資料結構說明

1. MS.Core放秘色相關的程式碼，MS.Core.MM釋放秘密相關的程式碼
2. 函示庫內的第一層會有以上六種類型
  2.1 Extensions 擴充方法
  2.2 Infrastructures 使用外部函式相關程式放在這邊
  2.3 Models 大部分直接使用的class例如Service，Repos會用的到或是跟Db關聯的Entities
  2.5 Repos 與資料庫溝通的相關程式
  2.6 Services 主要邏輯程式
  2.7 Utils 其他靜態class工具函式放在這邊
3. 如果其他部分有需要拉出來Models可以放在自己的資料夾, 例如Infrastructures/Redis/Models裡面放的是Redis會需要用的的設定檔結構