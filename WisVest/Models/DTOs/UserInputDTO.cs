namespace WisVestAPI.Models.DTOs
{
    public class UserInputDTO
    {
        public string? RiskTolerance { get; set; }           // Low, Medium, High
        public string? InvestmentHorizon { get; set; }       // Short, Medium, Long
        public int Age { get; set; }
        public string? Goal { get; set; }                    // Emergency Fund, Retirement, etc.
        public decimal TargetAmount { get; set; }           // The amount the user wants to accumulate
    }
}

