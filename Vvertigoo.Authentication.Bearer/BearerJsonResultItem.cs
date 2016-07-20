namespace Vvertigoo.Authentication.Bearer
{
    internal class BearerJsonResultItem
    {
        public BearerJsonResultItem(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; private set; }
        public string Value { get; private set; }
    }
}
