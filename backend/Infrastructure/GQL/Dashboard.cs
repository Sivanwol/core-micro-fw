using System.ComponentModel;
using Infrastructure.GQL.Common.Widgets;
namespace Infrastructure.GQL;

[Description("Dashboard Data")]
public class Dashboard {
    [Description("Data of profit widget")]
    public IEnumerable<ProfitWidget> Profits { get; set; }
    [Description("Data of users widget")]
    public IEnumerable<UsersWidget> Users { get; set; }
    [Description("Data of traffic source widget")]
    public IEnumerable<TrafficSourceWidget> TrafficSource { get; set; }
    [Description("Data of Missions widget")]
    public IEnumerable<MissionsWidget> Missions { get; set; }
    [Description("Data of Missions Status widget")]
    public MissionStatusWidget MissionStatus { get; set; }
}