using System.Globalization;

namespace Core.MergerService
{
    public class MergerService : IMergerService
    {
        public Uri Merge(string uriString, string msaId)
        {
            // 1. Remove trailing slash
            var uri = new Uri(uriString.TrimEnd('/'));

            // 2. Remove locale from URL (e.g., 'en-us/', 'fr-fr/', etc
            var cultureNames = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Skip(1)
                .Select(c => c.Name)
                .ToArray();
            var newUriSegments =
                from segment in uri.Segments
                    // Remove trailing slash
                select segment.TrimEnd('/') into segmentName
                // Use binary search to determine if the segment name is in the list of cultures
                let index = Array.BinarySearch(cultureNames, segmentName, StringComparer.OrdinalIgnoreCase)
                // Take only those segments which are not in the list of cultures
                where index < 0
                select segmentName;


            // 3. Build URL with MSA id at the end of the query string
            var newUri = new UriBuilder(uri)
            {
                Path = string.Join("/", newUriSegments).TrimEnd('/'),
                // Add "wt.mc_id={msaId}" at the end of the url.
                // If there is no query string, add "?wt.mc_id={msaId}"
                // If there is a query string, add "&wt.mc_id={msaId}"
                Query = uri.Query.Length == 0 ? $"wt.mc_id={msaId}" : $"{uri.Query}&wt.mc_id={msaId}"
            };

            // 4. Return the new URL
            return newUri.Uri;
        }
    }
}