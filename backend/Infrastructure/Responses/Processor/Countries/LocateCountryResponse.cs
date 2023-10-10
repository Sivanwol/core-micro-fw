namespace Infrastructure.Responses.Processor.Countries;

public class LocateCountryResponse {
    public bool IsFound { get; set; }
    public Domain.Entities.Countries? Record { get; set; }
}