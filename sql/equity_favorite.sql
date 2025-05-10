
-- お気に入り銘柄情報
CREATE TABLE equity_favorite(
  exchange            TEXT NOT NULL ,   -- 上場市場
  symbol              TEXT NOT NULL ,   -- コード
  name                TEXT          ,   -- 銘柄名
  lending_fee         REAL          ,   -- 貸株金利
  margin_lending_fee  REAL          ,   -- 信用貸株金利
  spotlight           TEXT         
 ,PRIMARY KEY(exchange, symbol)
);