using SalesAPI.Application.Commons;
using System;
using System.Text;

namespace SalesAPI.Tests.Helpers
{
    public static class UrlBuilder
    {
      
        public static void AppendQueryStringIfNotNullOrEmpty(StringBuilder urlBuilder, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                urlBuilder.Append(urlBuilder.ToString().Contains("?") ? "&" : "?");
                urlBuilder.Append($"{key}={Uri.EscapeDataString(value)}");
            }
        }
        public static string ToQueryString(this PaginationRequest request)
        {
            StringBuilder urlBuilder = new();

            AppendQueryStringIfNotNullOrEmpty(urlBuilder, "Id", request.Id?.ToString());
            AppendQueryStringIfNotNullOrEmpty(urlBuilder, "SearchText", request.SearchText);
            AppendQueryStringIfNotNullOrEmpty(urlBuilder, "StartDate", request.StartDate?.ToString("yyyy-MM-dd"));
            AppendQueryStringIfNotNullOrEmpty(urlBuilder, "EndDate", request.EndDate?.ToString("yyyy-MM-dd"));
            AppendQueryStringIfNotNullOrEmpty(urlBuilder, "PageNumber", request.PageNumber.ToString());
            AppendQueryStringIfNotNullOrEmpty(urlBuilder, "PageSize", request.PageSize.ToString());
            AppendQueryStringIfNotNullOrEmpty(urlBuilder, "OrderBy", request.OrderBy > 0 ? request.OrderBy.ToString() : "");
            AppendQueryStringIfNotNullOrEmpty(urlBuilder, "Descending", request.Descending.HasValue ? request.Descending.Value.ToString() : "");

            return urlBuilder.ToString();
        }
    }
}
