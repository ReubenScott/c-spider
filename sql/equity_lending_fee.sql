-- 貸株金利情報
CREATE TABLE equity_lending_fee(
  symbol              TEXT NOT NULL ,   -- 銘柄コード
  name                TEXT          ,   -- 銘柄名
  lending_fee         REAL          ,   -- 貸株金利
  margin_lending_fee  REAL              -- 信用貸株金利
 ,PRIMARY KEY(symbol)
);
