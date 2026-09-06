namespace Tempest.UI.Infrastructure.Plasma;

internal class PlasmaConfigEntryCallbackContext
{
    public string Section { get; }
    public string Key { get; }
    public Action<string> Callback { get; }

    public PlasmaConfigEntryCallbackContext(string section, string key, Action<string> callback)
    {
        Section = section;
        Key = key;
        Callback = callback;
    }
}