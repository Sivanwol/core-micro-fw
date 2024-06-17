namespace Infrastructure.GQL.Common.Widgets;

public class MissionStatusWidget {
    public int TotalBacklog { get; set; }
    public int TotalPending { get; set; }
    public int TotalInProcess { get; set; }
    public int TotalDone { get; set; }
    public int AverageTaskComplete { get; set; }
    public decimal UserAvgProfit { get; set; }
    public int AvgProfitPercentage { get; set; }
    public int UserInstalls { get; set; }
    public decimal UserCPC { get; set; }
}