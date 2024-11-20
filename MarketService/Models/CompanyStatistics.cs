using System;
using System.Data.Linq.Mapping;


namespace Market.Models
{

    [Table(Name = "company_statistics")]
    class CompanyStatistics
    {
        //   コード
        [Column(Name = "symbol", IsPrimaryKey = true)]
        public string Symbol { get; set; }

        //   銘柄名
        [Column(Name = "name", CanBeNull = false)]
        public string Name { get; set; }

        //   市場区分
        [Column(Name = "exchange")]
        public string Exchange { get; set; }

        //   設立日 
        [Column(Name = "established_date")]
        public DateTime? EstablishedDate { get; set; }

        //   上場日
        [Column(Name = "listing_date")]
        public DateTime? ListingDate { get; set; }

        //   東証業種名 業種
        [Column(Name = "sector")]
        public string Sector { get; set; }

        //   日経業種分類 業界
        [Column(Name = "industry")]
        public string Industry { get; set; }

        //   除息日
        [Column(Name = "ex_dividend_date", CanBeNull = true)]
        public DateTime? ExDividendDate { get; set; }

        //   配当利回り
        [Column(Name = "dividend_yield")]
        public float? DividendYield { get; set; }

        //   現在株価
        [Column(Name = "present_price")]
        public decimal? PresentPrice { get; set; }

        //   出来高
        [Column(Name = "volume")]
        public int? Volume { get; set; }

        //   年初来株価上昇率
        [Column(Name = "year_change_ratio")]
        public float? YearChangeRatio { get; set; }

        //   年安値
        [Column(Name = "year_low")]
        public decimal? YearLow { get; set; }

        //   年高値
        [Column(Name = "year_high")]
        public decimal? YearHigh { get; set; }

        //   1株純資産
        [Column(Name = "book_value_per_share")]
        public decimal? BookValuePerShare { get; set; }

        //   株価収益率
        [Column(Name = "per")]
        public float? PER { get; set; }

        //   PBR(Price/Book)
        [Column(Name = "pbr")]
        public float? PBR { get; set; }

        //   Enterprise Value/Revenue
        [Column(Name = "ev_revenue")]
        public float? EVRevenue { get; set; }

        //   Enterprise Value/EBITDA
        [Column(Name = "ev_ebitda")]
        public float? EVEBITDA { get; set; }

        //   基本1株当たり利益
        [Column(Name = "eps")]
        public float? EPS { get; set; }

        //   ROA(Return on Assets)
        [Column(Name = "roa")]
        public float? ROA { get; set; }

        //   ROE(Return on Equity)
        [Column(Name = "roe")]
        public float? ROE { get; set; }

        //   债务权益比率
        [Column(Name = "debt_equity_ratio")]
        public float? DebtEquityRatio { get; set; }

        //   自己資本比率
        [Column(Name = "own_capital_ratio")]
        public float? OwnCapitalRatio { get; set; }

        //   時価総額
        [Column(Name = "market_cap")]
        public decimal? MarketCap { get; set; }

        //   企業価値
        [Column(Name = "enterprise_value")]
        public decimal? EnterpriseValue { get; set; }

        //   信用倍率
        [Column(Name = "credit_multiplier")]
        public float? CreditMultiplier { get; set; }

        //   200日移動平均乖離率
        [Column(Name = "moving_average")]
        public decimal? MovingAverage { get; set; }

        //   売上高
        [Column(Name = "amount_of_sales")]
        public string AmountOfSales { get; set; }

        //   当期純利益
        [Column(Name = "net_income")]
        public string NetIncome { get; set; }

        //   営業C/F
        [Column(Name = "sales_cf")]
        public string SalesCf { get; set; }

        //   総資産
        [Column(Name = "total_assets")]
        public string TotalAssets { get; set; }

        //   現預金等
        [Column(Name = "cash_and_deposits")]
        public string CashAndDeposits { get; set; }

        //   資本合計
        [Column(Name = "total_capital")]
        public string TotalCapital { get; set; }

        //   平均年収
        [Column(Name = "average_annual_income")]
        public string AverageAnnualIncome { get; set; }

        //   评级 レーティング
        [Column(Name = "grade_rating")]
        public float? GradeRating { get; set; }

        //   指数採用
        [Column(Name = "index_adoption")]
        public string IndexAdoption { get; set; }

        //   単元株数
        [Column(Name = "per_unit")]
        public string PerUnit { get; set; }

        //   発行済み株式数
        [Column(Name = "issued_shares")]
        public long? IssuedShares { get; set; }

        //   事業内容
        [Column(Name = "business_scope")]
        public string BusinessScope { get; set; }

        //   取扱い商品
        [Column(Name = "product_range")]
        public string ProductRange { get; set; }

        //   代表者
        [Column(Name = "representative")]
        public string Representative { get; set; }

        //   資本金
        [Column(Name = "capital_stock")]
        public string CapitalStock { get; set; }

        //   本社住所
        [Column(Name = "address")]
        public string Address { get; set; }

        //   電話番号
        [Column(Name = "tel")]
        public string Tel { get; set; }

        //   URL
        [Column(Name = "url")]
        public string Url { get; set; }

        //   上場廃止日
        [Column(Name = "delisting_date")]
        public DateTime? DelistingDate { get; set; }

        //   更新日
        [Column(Name = "update_date")]
        public DateTime? UpdateDate { get; set; }

    }
}
