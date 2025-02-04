using System.Globalization;

namespace Habbits.Api.Middleware;
public class LanguageMiddleware
{
    private readonly RequestDelegate _next;

    public LanguageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

        var requestedLanguages = context.Request.Headers["Accept-Language"].ToString();

        var languageInfo = new CultureInfo("en");

        if (!string.IsNullOrEmpty(requestedLanguages))
        {

            var languagePreferences = requestedLanguages
                .Split(',')
                .Select(lang => lang.Split(';')[0])
                .ToList();

            var matchedLanguage = languagePreferences.FirstOrDefault(lang =>
                supportedLanguages.Any(supported => supported.Name.Equals(lang, StringComparison.OrdinalIgnoreCase)));

            if (matchedLanguage != null)
            {
                languageInfo = new CultureInfo(matchedLanguage);
            }
        }

        CultureInfo.CurrentCulture = languageInfo;
        CultureInfo.CurrentUICulture = languageInfo;

        await _next(context);
    }
}
