namespace Domain.Filters.Macros;

public abstract class BaseMacro(Dictionary<string, string> marcoData, string macroIdentify) : IMacro {
    protected Dictionary<string, string> MarcoData { get; private set; } = marcoData;
    public string MacroIdentify { get; private set; } = macroIdentify;

    public string Match() {
        if (!MarcoData.TryGetValue(MacroIdentify, out string? value)) {
            throw new Exception("Macro not found");
        }
        return value;
    }
}