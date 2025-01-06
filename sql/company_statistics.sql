
-- 銘柄基本情報
CREATE TABLE company_statistics (
  symbol                    TEXT NOT NULL  ,  -- コード
  name                      TEXT           ,  -- 銘柄名
  exchange                  TEXT           ,  -- 市場区分
  established_date          TEXT           ,  -- 設立日 
  listing_date              TEXT           ,  -- 上場日
  sector                    TEXT           ,  -- 東証業種名 業種
  industry                  TEXT           ,  -- 日経業種分類 業界
  dividend_yield            REAL           ,  -- 配当利回り
  ex_dividend_date          TEXT           ,  -- 除息日
  year_change_ratio         REAL           ,  -- 年初来株価上昇率
  present_price             REAL           ,  -- 現在株価
  book_value_per_share      REAL           ,  -- 1株純資産
  year_low                  REAL           ,  -- 年初来安値
  year_high                 REAL           ,  -- 年初来高値
  moving_average            REAL           ,  -- 200日移動平均線
  volume                    INTEGER        ,  -- 出来高
  per                       REAL           ,  -- 株価収益率
  pbr                       REAL           ,  -- 株価純資産倍率
  ev_revenue                REAL           ,  -- 企业价值/收入
  ev_ebitda                 REAL           ,  -- 企业价值/息税前利润
  eps                       REAL           ,  -- 基本1株当たり利益
  roa                       REAL           ,  -- 総資産利益率
  roe                       REAL           ,  -- 株主資本利益率
  debt_equity_ratio         REAL           ,  -- 债务权益比率
  own_capital_ratio         REAL           ,  -- 自己資本比率
  market_cap                REAL           ,  -- 時価総額
  enterprise_value          REAL           ,  -- 企業価値
  credit_multiplier         REAL           ,  -- 信用倍率
  grade_rating              REAL           ,  -- レーティング
  index_adoption            TEXT           ,  -- 指数採用
  per_unit                  TEXT           ,  -- 単元株数
  issued_shares             INTEGER        ,  -- 発行済株数
  business_scope            TEXT           ,  -- 事業内容
  product_range             TEXT           ,  -- 取扱い商品
  representative            TEXT           ,  -- 代表者
  capital_stock             TEXT           ,  -- 資本金
  address                   TEXT           ,  -- 本社住所
  tel                       TEXT           ,  -- 電話番号
  url                       TEXT           ,  -- URL
  amount_of_sales           TEXT           ,  -- 売上高
  net_income                TEXT           ,  -- 当期純利益
  sales_cf                  TEXT           ,  -- 営業C/F
  total_assets              TEXT           ,  -- 総資産
  cash_and_deposits         TEXT           ,  -- 現預金等
  total_capital             TEXT           ,  -- 資本合計
  average_annual_income     TEXT           ,  -- 平均年収
  delisting_date            TEXT           ,  -- 上場廃止日
  update_date               TEXT           ,  -- 更新日
  PRIMARY KEY(symbol)
);
