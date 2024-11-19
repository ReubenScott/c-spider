CREATE TABLE IPOCalendar(
   symbol           VARCHAR(3) NOT NULL
  ,name             VARCHAR(36) 
  ,ipoDate          DATE
  ,priceRangeLow    NUMERIC(4,2)
  ,priceRangeHigh   NUMERIC(4,2)
  ,currency         VARCHAR(3) 
  ,exchange         VARCHAR(3) 
  ,PRIMARY KEY(symbol,exchange)
);