using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Responses.Controllers;

[SwaggerSchema(Required = new[] {
    "Religions Records for all Supported"
})]
[SwaggerTag("General")]
public class ReligionsResponse {
    [SwaggerSchema("Record ID")]
    public int ID { get; set; }

    [SwaggerSchema("Religion Name")]
    public string Name { get; set; }
}

[SwaggerSchema(Required = new[] {
    "Countries Records for all Supported"
})]
[SwaggerTag("General")]
public class CountriesResponse {
    [SwaggerSchema("Record ID")]
    public int ID { get; set; }

    [SwaggerSchema("Country Name")]
    public string CountryName { get; set; }

    [SwaggerSchema("Country Code")]
    public string CountryCode { get; set; }

    [SwaggerSchema("Country Code 3")]
    public string CountryCode3 { get; set; }

    [SwaggerSchema("Country Phone Prefix Number (it like +972/ +1 / +44 and so on)")]
    public string CountryNumber { get; set; }
}

[SwaggerSchema(Required = new[] {
    "Ethnicities Records for all Supported"
})]
[SwaggerTag("General")]
public class EthnicitiesResponse {
    [SwaggerSchema("Record ID")]
    public int ID { get; set; }

    [SwaggerSchema("Ethnicity Name")]
    public string Name { get; set; }
}

[SwaggerSchema(Required = new[] {
    "Languages Records for all Supported"
})]
[SwaggerTag("General")]
public class LanguagesResponse {
    [SwaggerSchema("Record ID")]
    public int ID { get; set; }

    [SwaggerSchema("Language Name")]
    public string Name { get; set; }

    [SwaggerSchema("Language Code")]
    public string Code { get; set; }
}

[SwaggerSchema(Required = new[] {
    "General Response for the Controllers when user login / open the app / register the user"
})]
[SwaggerTag("General")]
public class GeneralResponse {
    [SwaggerSchema("All Supported Religions")]
    public List<ReligionsResponse> Religions { get; set; }

    [SwaggerSchema("All Supported Countries")]
    public List<CountriesResponse> Countries { get; set; }

    [SwaggerSchema("All Supported Ethnicities")]
    public List<EthnicitiesResponse> Ethnicities { get; set; }

    [SwaggerSchema("All Supported Languages")]
    public List<LanguagesResponse> Languages { get; set; }
}