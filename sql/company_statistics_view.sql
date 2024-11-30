-- 删除原视图
DROP VIEW company_statistics_view;

-- 创建新视图
CREATE VIEW company_statistics_view AS
SELECT			
   symbol                    AS  コード
  ,name                      AS  銘柄名
  ,exchange                  AS  市場区分
  ,established_date          AS  設立日 
  ,industry                  AS  日経業種分類
  ,sector                    AS  東証業種名
  ,ROUND(CAST(volume*1000 AS REAL)/issued_shares, 2) AS '取引回転率‰' 
  ,dividend_yield            AS  '配当利回り%'
  ,IFNULL(debt_equity_ratio, 0)         AS  '债务权益比率%'
  ,per                       AS  '株価収益率(倍)'
  ,pbr                       AS  '株価純資産倍率(倍)'
  ,ev_revenue                AS  '企业价值/收入'
  ,ev_ebitda                 AS  '企业价值/息税前利润'
  ,ROUND((present_price - moving_average)*100 /moving_average, 2)  AS  '200日移動平均乖離率%'
  ,year_change_ratio         AS  '年初来株価上昇率%'
  ,present_price             AS  現在株価
  ,book_value_per_share      AS  '1株純資産'
  ,year_low                  AS  年初来安値
  ,year_high                 AS  年初来高値
  ,moving_average            AS  '200日移動平均線'
  ,eps                       AS  '基本1株当たり利益'
  ,roa                       AS  '総資産利益率%'
  ,roe                       AS  '株主資本利益率%'
  ,own_capital_ratio         AS  '自己資本比率%'
  ,market_cap                AS  '時価総額(億円)'
  ,enterprise_value          AS  '企業価値(億円)'
  ,credit_multiplier         AS  '信用倍率(倍)'
  ,ex_dividend_date          AS  除息日
  ,amount_of_sales           AS  売上高
  ,net_income                AS  当期純利益
  ,sales_cf                  AS  '営業C/F'
  ,total_assets              AS  総資産
  ,cash_and_deposits         AS  現預金等
  ,total_capital             AS  資本合計
  ,average_annual_income     AS  平均年収
  ,grade_rating              AS  レーティング
  ,index_adoption            AS  指数採用
  ,per_unit                  AS  単元株数
  ,issued_shares             AS  発行済株数
  ,business_scope            AS  事業内容
  ,product_range             AS  取扱い商品
  ,representative            AS  代表者
  ,capital_stock             AS  資本金
  ,address                   AS  本社住所
  ,tel                       AS  電話番号
  ,url                       AS  URL
  ,delisting_date            AS  上場廃止日
  ,update_date               AS  更新日
  ,listing_date              AS  上場日
  ,volume                    AS  出来高
FROM company_statistics
WHERE delisting_date is NULL
  AND exchange like '東証%'
ORDER BY 
  CAST(volume AS REAL)/issued_shares DESC   -- 换手率
 ,year_change_ratio asc
;


