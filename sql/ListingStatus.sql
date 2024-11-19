CREATE TABLE ListingStatus (
   exchange      VARCHAR(9) NOT NULL
  ,symbol        VARCHAR(10) NOT NULL
  ,assetType     VARCHAR(5)
  ,name          VARCHAR(76)
  ,ipoDate       DATE
  ,delistingDate DATE
  ,status        VARCHAR(8)
  ,PRIMARY KEY(symbol,exchange)
);
