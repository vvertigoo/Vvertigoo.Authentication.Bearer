using System.Collections.Generic;
using System.Text;

namespace Vvertigoo.Authentication.Bearer
{
    internal class BearerJsonResult
    {
        private List<BearerJsonResultItem> _items;

        public BearerJsonResult()
        {
            _items = new List<BearerJsonResultItem>();
        }

        public void Add(string key, string value)
        {
            _items.Add(new BearerJsonResultItem(key, value));
        }

        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append("{");
            for (int i = 0; i < _items.Count; i++)
            {
                if (i == 0)
                {
                    resultBuilder.Append("\"");
                    resultBuilder.Append(_items[i].Key);
                    resultBuilder.Append("\":\"");
                    resultBuilder.Append(_items[i].Value);
                    resultBuilder.Append("\"");
                }
                else
                {
                    resultBuilder.Append(",\"");
                    resultBuilder.Append(_items[i].Key);
                    resultBuilder.Append("\":\"");
                    resultBuilder.Append(_items[i].Value);
                    resultBuilder.Append("\"");
                }
            }
            resultBuilder.Append("}");

            return resultBuilder.ToString();
        }
    }
}
