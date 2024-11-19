
CREATE TABLE CompanyOverview(
   Symbol                     VARCHAR(10) NOT NULL  -- 符号                     
  ,AssetType                  VARCHAR(12)           -- 资产类型                  
  ,Name                       VARCHAR(31)           -- 名称                       
  ,Description                VARCHAR               -- 描述                
  ,CIK                        INTEGER               -- CIK                        
  ,Exchange                   VARCHAR(4)            -- 兑换                   
  ,Currency                   VARCHAR(3)            -- 货币                   
  ,Country                    VARCHAR(3)            -- 国家                    
  ,Sector                     VARCHAR(10)           -- 版块                     
  ,Industry                   VARCHAR(27)           -- 行业                   
  ,Address                    VARCHAR(34)           -- 地址                    
  ,FiscalYearEnd              VARCHAR(8)            -- 财政年度末              
  ,LatestQuarter              DATE                  -- 最新季度              
  ,MarketCapitalization       INTEGER               -- 市值       
  ,EBITDA                     INTEGER               -- 息税折旧摊销前利润                     
  ,PERatio                    NUMERIC(5,2)          -- 市盈率                    
  ,PEGRatio                   NUMERIC(5,3)          -- 市盈率                   
  ,BookValue                  NUMERIC(5,2)          -- 账面价值                  
  ,DividendPerShare           NUMERIC(4,2)          -- 每股股息           
  ,DividendYield              NUMERIC(6,4)          -- 股息率              
  ,EPS                        NUMERIC(4,2)          -- 每股收益                        
  ,RevenuePerShareTTM         NUMERIC(5,2)          -- 每股收益TTM         
  ,ProfitMargin               NUMERIC(5,3)          -- 利润率               
  ,OperatingMarginTTM         NUMERIC(4,2)          -- 营业利润率TTM         
  ,ReturnOnAssetsTTM          NUMERIC(6,4)          -- 资产回报率TTM          
  ,ReturnOnEquityTTM          NUMERIC(5,3)          -- 股权回报率TTM          
  ,RevenueTTM                 INTEGER               -- 收入TTM                 
  ,GrossProfitTTM             INTEGER               -- 毛利润TTM             
  ,DilutedEPSTTM              NUMERIC(4,2)          -- 摊薄每股收益TTM              
  ,QuarterlyEarningsGrowthYOY NUMERIC(5,3)          -- 季度收益同比增长率 
  ,QuarterlyRevenueGrowthYOY  NUMERIC(5,3)          -- 季度收入同比增长  
  ,AnalystTargetPrice         NUMERIC(6,2)          -- 分析师目标价         
  ,AnalystRatingStrongBuy     INTEGER               -- 分析师评级强力买入     
  ,AnalystRatingBuy           INTEGER               -- 分析师评级买入           
  ,AnalystRatingHold          INTEGER               -- 分析师评级持有          
  ,AnalystRatingSell          INTEGER               -- 分析师评级卖出          
  ,AnalystRatingStrongSell    BIT                   -- 分析师强烈卖出    
  ,TrailingPE                 NUMERIC(5,2)          -- 追踪PE                 
  ,ForwardPE                  NUMERIC(5,2)          -- 远期PE                  
  ,PriceToSalesRatioTTM       NUMERIC(5,3)          -- 价格销售比TTM       
  ,PriceToBookRatio           NUMERIC(4,2)          -- 账面价格比           
  ,EVToRevenue                NUMERIC(4,2)          -- EV收入比                
  ,EVToEBITDA                 NUMERIC(5,2)          -- EVT至EBITDA                 
  ,Beta                       NUMERIC(5,3)          -- 贝塔值                       
  ,High52Week                 NUMERIC(6,2)          -- 最高52周                 
  ,Low52Week                  NUMERIC(6,2)          -- 52周低点                  
  ,MovingAverage50Day         NUMERIC(6,2)          -- 50天移动平均值         
  ,MovingAverage200Day        NUMERIC(5,1)          -- 200天移动平均值        
  ,SharesOutstanding          INTEGER               -- 流通股          
  ,DividendDate               DATE                  -- 分红日               
  ,ExDividendDate             DATE                  -- 除息日
  ,PRIMARY KEY(Symbol)
);


INSERT INTO CompanyOverview(Symbol,AssetType,Name,Description,CIK,Exchange,Currency,Country,Sector,Industry,Address,FiscalYearEnd,LatestQuarter,MarketCapitalization,EBITDA,PERatio,PEGRatio,BookValue,DividendPerShare,DividendYield,EPS,RevenuePerShareTTM,ProfitMargin,OperatingMarginTTM,ReturnOnAssetsTTM,ReturnOnEquityTTM,RevenueTTM,GrossProfitTTM,DilutedEPSTTM,QuarterlyEarningsGrowthYOY,QuarterlyRevenueGrowthYOY,AnalystTargetPrice,AnalystRatingStrongBuy,AnalystRatingBuy,AnalystRatingHold,AnalystRatingSell,AnalystRatingStrongSell,TrailingPE,ForwardPE,PriceToSalesRatioTTM,PriceToBookRatio,EVToRevenue,EVToEBITDA,Beta,52WeekHigh,52WeekLow,50DayMovingAverage,200DayMovingAverage,SharesOutstanding,DividendDate,ExDividendDate) VALUES ('IBM','Common Stock','International Business Machines','International Business Machines Corporation (IBM) is an American multinational technology company headquartered in Armonk, New York, with operations in over 170 countries. The company began in 1911, founded in Endicott, New York, as the Computing-Tabulating-Recording Company (CTR) and was renamed International Business Machines in 1924. IBM is incorporated in New York. IBM produces and sells computer hardware, middleware and software, and provides hosting and consulting services in areas ranging from mainframe computers to nanotechnology. IBM is also a major research organization, holding the record for most annual U.S. patents generated by a business (as of 2020) for 28 consecutive years. Inventions by IBM include the automated teller machine (ATM), the floppy disk, the hard disk drive, the magnetic stripe card, the relational database, the SQL programming language, the UPC barcode, and dynamic random-access memory (DRAM). The IBM mainframe, exemplified by the System/360, was the dominant computing platform during the 1960s and 1970s.',51143,'NYSE','USD','USA','TECHNOLOGY','COMPUTER & OFFICE EQUIPMENT','1 NEW ORCHARD ROAD, ARMONK, NY, US','December','2023-12-31',168800272000,13777000000,22.57,4.251,24.63,6.63,0.0358,8.16,67.89,0.121,0.23,0.0447,0.337,61860000000,32688000000,8.16,0.197,0.041,182.16,4,3,10,3,0,22.57,18.32,2.729,7.49,3.48,14.65,0.716,195.12,116.32,173.01,149.2,916745000,'2024-03-09','2024-02-08');

