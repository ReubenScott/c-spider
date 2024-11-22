using System;
using System.Data.Linq.Mapping;


namespace Market.Models
{

    [Table(Name = "company_statistics")]
    class CompanyStatistics
    {
        //  コード
        [Column(Name = "symbol", IsPrimaryKey = true)]
        public string Symbol { get; set; }

        //  銘柄名
        [Column(Name = "name", CanBeNull = false)]
        public string Name { get; set; }

        //  市場区分
        [Column(Name = "exchange")]
        public string Exchange { get; set; }

        //  設立日 
        [Column(Name = "established_date")]
        public DateTime? EstablishedDate { get; set; }

        //  上場日
        [Column(Name = "listing_date")]
        public DateTime? ListingDate { get; set; }

        //  東証業種名 業種
        [Column(Name = "sector")]
        public string Sector { get; set; }

        //  日経業種分類 業界
        [Column(Name = "industry")]
        public string Industry { get; set; }

        //  配当利回り
        [Column(Name = "dividend_yield")]
        public decimal? DividendYield { get; set; }

        //  除息日
        [Column(Name = "ex_dividend_date", CanBeNull = true)]
        public DateTime? ExDividendDate { get; set; }

        //  年初来株価上昇率
        [Column(Name = "year_change_ratio")]
        public decimal? YearChangeRatio { get; set; }

        //  現在株価
        [Column(Name = "present_price")]
        public decimal? PresentPrice { get; set; }

        //  1株純資産
        [Column(Name = "book_value_per_share")]
        public decimal? BookValuePerShare { get; set; }

        //  年安値
        [Column(Name = "year_low")]
        public decimal? YearLow { get; set; }

        //  年高値
        [Column(Name = "year_high")]
        public decimal? YearHigh { get; set; }

        //  200日移動平均乖離率
        [Column(Name = "moving_average")]
        public decimal? MovingAverage { get; set; }

        //  出来高
        [Column(Name = "volume")]
        public int? Volume { get; set; }

        //  株価収益率
        [Column(Name = "per")]
        public decimal? PER { get; set; }

        //  株価純資産倍率 PBR(Price/Book)
        [Column(Name = "pbr")]
        public decimal? PBR { get; set; }

        //  企业价值/收入  Enterprise Value/Revenue
        [Column(Name = "ev_revenue")]
        public decimal? EVRevenue { get; set; }

        //  企业价值/息税前利润 Enterprise Value/EBITDA
        [Column(Name = "ev_ebitda")]
        public decimal? EVEBITDA { get; set; }

        //  基本1株当たり利益
        [Column(Name = "eps")]
        public decimal? EPS { get; set; }

        //  総資産利益率 ROA(Return on Assets)
        [Column(Name = "roa")]
        public decimal? ROA { get; set; }

        //  株主資本利益率 ROE(Return on Equity)
        [Column(Name = "roe")]
        public decimal? ROE { get; set; }

        //  债务权益比率
        [Column(Name = "debt_equity_ratio")]
        public decimal? DebtEquityRatio { get; set; }

        //  自己資本比率
        [Column(Name = "own_capital_ratio")]
        public decimal? OwnCapitalRatio { get; set; }

        //  時価総額
        [Column(Name = "market_cap")]
        public decimal? MarketCap { get; set; }

        //  企業価値
        [Column(Name = "enterprise_value")]
        public decimal? EnterpriseValue { get; set; }

        //  信用倍率
        [Column(Name = "credit_multiplier")]
        public decimal? CreditMultiplier { get; set; }

        //  评级 レーティング
        [Column(Name = "grade_rating")]
        public decimal? GradeRating { get; set; }

        //  指数採用
        [Column(Name = "index_adoption")]
        public string IndexAdoption { get; set; }

        //  単元株数
        [Column(Name = "per_unit")]
        public string PerUnit { get; set; }

        //  発行済み株式数
        [Column(Name = "issued_shares")]
        public long? IssuedShares { get; set; }

        //  事業内容
        [Column(Name = "business_scope")]
        public string BusinessScope { get; set; }

        //  取扱い商品
        [Column(Name = "product_range")]
        public string ProductRange { get; set; }

        //  代表者
        [Column(Name = "representative")]
        public string Representative { get; set; }

        //  資本金
        [Column(Name = "capital_stock")]
        public string CapitalStock { get; set; }

        //  本社住所
        [Column(Name = "address")]
        public string Address { get; set; }

        //  電話番号
        [Column(Name = "tel")]
        public string Tel { get; set; }

        //  URL
        [Column(Name = "url")]
        public string Url { get; set; }

        //  売上高
        [Column(Name = "amount_of_sales")]
        public string AmountOfSales { get; set; }

        //  当期純利益
        [Column(Name = "net_income")]
        public string NetIncome { get; set; }

        //  営業C/F
        [Column(Name = "sales_cf")]
        public string SalesCf { get; set; }

        //  総資産
        [Column(Name = "total_assets")]
        public string TotalAssets { get; set; }

        //  現預金等
        [Column(Name = "cash_and_deposits")]
        public string CashAndDeposits { get; set; }

        //  資本合計
        [Column(Name = "total_capital")]
        public string TotalCapital { get; set; }

        //  平均年収
        [Column(Name = "average_annual_income")]
        public string AverageAnnualIncome { get; set; }

        //  上場廃止日
        [Column(Name = "delisting_date")]
        public DateTime? DelistingDate { get; set; }

        //  更新日
        [Column(Name = "update_date")]
        public DateTime? UpdateDate { get; set; }

    }
}
