
-- 銘柄基本情報
CREATE TABLE company_profile(
   symbol                     VARCHAR(10) NOT NULL  -- コード
  ,grade_rating                  VARCHAR(12)        -- 评级
  ,name                       VARCHAR(50)           -- 企業名
  ,exchange                   VARCHAR(31)           -- 上場市場
    
  -- https://www.nikkei.com/nkd/company/gaiyo/?scode=7003 
  ,established_date       VARCHAR           -- 設立年月日 
  ,listing_date       VARCHAR           -- 上場年月日
  ,sector                   VARCHAR           -- 東証業種名 業種
  ,industry                 VARCHAR           -- 日経業種分類 業界
  ,index_adoption           VARCHAR           -- 指数採用
  
   
-- https://dt.kabumap.com/servlets/dt/Action?SRC=basic/base&codetext=7003
-- https://finance.yahoo.com/quote/7003.T
-- https://kabuyoho.jp/reportTop?bcode=5020
  ,dividend_yield       VARCHAR           -- 配当利回り
  ,per       VARCHAR           -- PE Ratio (TTM)	
  ,pbr       VARCHAR           -- 実績PBR
  ,eps       VARCHAR           -- EPS (TTM)
  ,roa       VARCHAR     -- ROA
  ,roe       VARCHAR     -- ROE
  ,debt_equity_ratio       VARCHAR     -- 债务权益比率
  ,own_capital_ratio       VARCHAR     -- 自己資本比率
  ,market_cap       VARCHAR           -- 時価総額
  ,enterprise_value       VARCHAR     -- 企業価値
  ,credit_multiplier       VARCHAR     -- 信用倍率
  ,ex_dividend_date       VARCHAR     -- 除息日
   

-- https://shikiho.toyokeizai.net/stocks/7003
  ,book_value_per_share       VARCHAR     -- 1株純資産
  ,year_high       VARCHAR     -- 年高値
  ,year_low       VARCHAR      -- 年安値
  ,year_change_ratio       NUMERIC     -- 年初来株価上昇率
  ,moving_average_deviation       VARCHAR     -- 200日移動平均乖離率
  

  ,amount_of_sales            VARCHAR(10)           -- 売上高
  ,net_income                 VARCHAR(10)           -- 当期純利益
  ,sales_cf                   VARCHAR(10)           -- 営業C/F
  ,total_assets               VARCHAR(10)           -- 総資産
  ,cash_and_deposits          VARCHAR(10)           -- 現預金等
  ,total_capital              VARCHAR(10)           -- 資本合計
  ,average_annual_income      VARCHAR(27)           -- 平均年収    
  ,business_scope       VARCHAR     -- 事業内容
  ,product_range       VARCHAR     -- 取扱い商品
  -- https://minkabu.jp/stock/7003/fundamental
  ,per_unit           VARCHAR           -- 単元株数
  ,representative     VARCHAR           -- 代表者
  ,capital_stock     VARCHAR           -- 資本金
  ,address     VARCHAR           -- 本社住所
  ,tel     VARCHAR           -- 電話番号
  ,url                      VARCHAR           -- URL
  ,spotlight                  VARCHAR(12)           -- 注目度
	,update_date                VARCHAR(8)  -- 更新日
  
  ,PRIMARY KEY(symbol)
);



-- データ追加
insert into company_profile
("symbol", "spotlight", "name", "industry", "exchange", "amount_of_sales", "net_income", "sales_cf", "total_assets", "cash_and_deposits", "total_capital", "average_annual_income")
select * from CompanyProfile







