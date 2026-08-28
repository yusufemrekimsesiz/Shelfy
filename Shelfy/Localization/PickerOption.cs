namespace Shelfy.Localization;

public sealed class PickerOption
{
    public string Key { get; }
    private readonly string _display;

    public PickerOption(string key, string display)
    {
        Key = key;
        _display = display;
    }

    public override string ToString() => _display;

    public override bool Equals(object? obj) => obj is PickerOption other && other.Key == Key;
    public override int GetHashCode() => Key.GetHashCode();
}