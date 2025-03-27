using System.Collections;

namespace WebApi.Test.InlineData
{
    public class CultureInlineDataTest : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "pt-br" };
            yield return new object[] { "en" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
