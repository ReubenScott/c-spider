

-- 初始化
DELETE FROM company_statistics ;

INSERT INTO company_statistics (
  symbol
) select symbol from CompanyProfile
;


insert into  listing_status (
   exchange
  ,symbol
  ,name
  ,assetType
  ,ipoDate  
  ,status  
) 
select 
  '東証'
  ,symbol
  , name
  ,'Stock'
  ,replace(replace(replace(replace(listing_date , "年", "") , "月", "") , "日", "") , '&nbsp;', '')
  -- ,delistingDate 
  ,'Active'  
from  company_profile



-- SQLite 隨機取數
SELECT symbol, name, update_date FROM company_statistics
   WHERE (update_date <> '2024/11/27' OR update_date IS NULL)
       AND delisting_date IS NULL
   ORDER BY RANDOM() LIMIT 100



-- 廃場
update company_statistics as t1
 set (
     name
    ,exchange
    ,established_date
    ,listing_date
    ,sector
    ,industry
    ,ex_dividend_date
    ,dividend_yield
    ,year_change_ratio
    ,year_low
    ,year_high
    ,book_value_per_share
    ,per
    ,pbr
    ,eps
    ,roa
    ,roe
    ,debt_equity_ratio
    ,own_capital_ratio
    ,market_cap
    ,enterprise_value
    ,credit_multiplier
    ,moving_average_deviation
    ,amount_of_sales
    ,net_income
    ,sales_cf
    ,total_assets
    ,cash_and_deposits
    ,total_capital
    ,average_annual_income
    ,grade_rating
    ,index_adoption
    ,business_scope
    ,product_range
    ,representative
    ,capital_stock
    ,address
    ,tel
    ,per_unit
    ,url
    ,delisting_date
 ) =  (
  select 
     a.name
    ,a.exchange
    ,a.established_date
    ,a.listing_date
    ,a.sector
    ,a.industry
    ,a.ex_dividend_date
    ,a.dividend_yield
    ,a.year_change_ratio
    ,a.year_low
    ,a.year_high
    ,a.book_value_per_share
    ,a.per
    ,a.pbr
    ,a.eps
    ,a.roa
    ,a.roe
    ,a.debt_equity_ratio
    ,a.own_capital_ratio
    ,a.market_cap
    ,a.enterprise_value
    ,a.credit_multiplier
    ,a.moving_average_deviation
    ,a.amount_of_sales
    ,a.net_income
    ,a.sales_cf
    ,a.total_assets
    ,a.cash_and_deposits
    ,a.total_capital
    ,a.average_annual_income
    ,a.grade_rating
    ,a.index_adoption
    ,a.business_scope
    ,a.product_range
    ,a.representative
    ,a.capital_stock
    ,a.address
    ,a.tel
    ,a.per_unit
    ,a.url
    ,b.delistingDate
from company_profile a
 left join listing_status b
  on a.symbol = b.symbol
 where a.symbol=t1.symbol)
where t1.update_date is null 
;

 
update company_statistics
set ex_dividend_date  = (
	SELECT
	  substr(ex_dividend_date, 1, 4) || '/' ||
	  substr(ex_dividend_date, 5, 2) || '/' ||
	  substr(ex_dividend_date, 7, 2)
    AS formatted_date
)
where LENGTH(ex_dividend_date) = 8 ;



update company_statistics
 set dividend_yield = REPLACE(dividend_yield, '%', '')
 where dividend_yield  LIKE '%\%%'  ESCAPE '\';
 
 





