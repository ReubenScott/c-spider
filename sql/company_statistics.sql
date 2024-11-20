
-- 銘柄基本情報
CREATE TABLE company_statistics (
  symbol                    VARCHAR(10)    ,      -- コード
  name                      VARCHAR(50)    ,      -- 銘柄名
  exchange                  VARCHAR(30)    ,      -- 市場区分
  established_date          VARCHAR(10)    ,      -- 設立日 
  listing_date              VARCHAR(10)    ,      -- 上場日
  sector                    VARCHAR        ,      -- 東証業種名 業種
  industry                  VARCHAR        ,      -- 日経業種分類 業界
  dividend_yield            REAL           ,      -- 配当利回り
  ex_dividend_date          VARCHAR(10)    ,      -- 除息日
  year_change_ratio         REAL           ,      -- 年初来株価上昇率
  present_price             NUMERIC        ,      -- 現在株価
  book_value_per_share      NUMERIC        ,      -- 1株純資産
  year_low                  NUMERIC        ,      -- 年初来安値
  year_high                 NUMERIC        ,      -- 年初来高値
  moving_average            NUMERIC        ,      -- 200日移動平均線
  volume                    INTEGER        ,      -- 出来高
  per                       REAL           ,      -- 株価収益率
  pbr                       REAL           ,      -- 株価純資産倍率
  ev_revenue                REAL           ,      -- 企业价值/收入
  ev_ebitda                 REAL           ,      -- 企业价值/息税前利润
  eps                       REAL           ,      -- 基本1株当たり利益
  roa                       REAL           ,      -- 総資産利益率
  roe                       REAL           ,      -- 株主資本利益率
  debt_equity_ratio         REAL           ,      -- 债务权益比率
  own_capital_ratio         REAL           ,      -- 自己資本比率
  market_cap                NUMERIC        ,      -- 時価総額
  enterprise_value          NUMERIC        ,      -- 企業価値
  credit_multiplier         REAL           ,      -- 信用倍率
  grade_rating              REAL           ,      -- レーティング
  index_adoption            VARCHAR        ,      -- 指数採用
  per_unit                  VARCHAR        ,      -- 単元株数
  issued_shares             BIGINT         ,      -- 発行済株数
  business_scope            VARCHAR        ,      -- 事業内容
  product_range             VARCHAR        ,      -- 取扱い商品
  representative            VARCHAR        ,      -- 代表者
  capital_stock             VARCHAR        ,      -- 資本金
  address                   VARCHAR        ,      -- 本社住所
  tel                       VARCHAR        ,      -- 電話番号
  url                       VARCHAR        ,      -- URL
  amount_of_sales           VARCHAR(10)    ,      -- 売上高
  net_income                VARCHAR(10)    ,      -- 当期純利益
  sales_cf                  VARCHAR(10)    ,      -- 営業C/F
  total_assets              VARCHAR(10)    ,      -- 総資産
  cash_and_deposits         VARCHAR(10)    ,      -- 現預金等
  total_capital             VARCHAR(10)    ,      -- 資本合計
  average_annual_income     VARCHAR        ,      -- 平均年収
  delisting_date            VARCHAR(10)    ,      -- 上場廃止日
  update_date               VARCHAR(10)    ,      -- 更新日
  PRIMARY KEY(symbol)
);
