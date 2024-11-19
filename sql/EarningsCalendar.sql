CREATE TABLE EarningsCalendar(
   symbol           VARCHAR(3) NOT NULL
  ,name             VARCHAR(36) 
  ,reportDate       DATE
  ,fiscalDateEnding DATE  NOT NULL
  ,estimate         NUMERIC(4,2)
  ,currency         VARCHAR(3) 
  ,PRIMARY KEY(symbol,fiscalDateEnding)
);
INSERT INTO EarningsCalendar(symbol,name,reportDate,fiscalDateEnding,estimate,currency) VALUES ('IBM','International Business Machines Corp','2024/4/24','2024/3/31',1.59,'USD');
INSERT INTO EarningsCalendar(symbol,name,reportDate,fiscalDateEnding,estimate,currency) VALUES ('IBM','International Business Machines Corp','2024/7/24','2024/6/30',NULL,'USD');
INSERT INTO EarningsCalendar(symbol,name,reportDate,fiscalDateEnding,estimate,currency) VALUES ('IBM','International Business Machines Corp','2024/10/23','2024/9/30',NULL,'USD');
INSERT INTO EarningsCalendar(symbol,name,reportDate,fiscalDateEnding,estimate,currency) VALUES ('IBM','International Business Machines Corp','2025/1/22','2024/12/31',NULL,'USD');
