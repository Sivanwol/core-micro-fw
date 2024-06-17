namespace Infrastructure.GQL.Common.Widgets;

public class MissionsWidget {
    public string IssueBy { get; set; }
    public string Client { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    public int Losses { get; set; }
    public int Profit { get; set; }
    public DateTime LastUpdate { get; set; }
    public string ReportBy { get; set; }
}