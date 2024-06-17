namespace Domain.DTO.ConfigurableEntities;

public class ConfigurableEntityMetaData {
    public ConfigurableEntityMetaData(string key, string displayName, string description, string value) {
        Key = key;
        DisplayName = displayName;
        Description = description;
        Value = value;
    }
    public string Key { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string Value { get; set; }
}