CREATE TABLE Earnings(
   Symbol             VARCHAR(10) NOT NULL
  ,ReportType         VARCHAR(20) NOT NULL 
  ,fiscalDateEnding   DATE NOT NULL
  ,reportedDate       DATE  
  ,reportedEPS        NUMERIC(4,2) 
  ,estimatedEPS       NUMERIC(3,1) 
  ,surprise           NUMERIC(4,2) 
  ,surprisePercentage NUMERIC(6,4) 
  ,PRIMARY KEY(Symbol,ReportType,fiscalDateEnding)
);