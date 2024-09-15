using System.Reflection;

namespace Catalog.API.Helpers
{
    public static class RequestQueryHandler
    {
        public static T CreateRequestFromQuery<T>(IQueryCollection query) where T : class, new()
        {
            var request = new T();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                string? queryValue = query[property.Name.ToLower()].ToString();

                if (string.IsNullOrEmpty(queryValue))
                {
                    continue;
                }
                var propertyType = property.PropertyType;

                // Convert query value to property type
                var convertedValue = Convert.ChangeType(queryValue, propertyType);

                if (convertedValue != null)
                {
                    // Set the property value
                    property.SetValue(request, convertedValue);
                }

            }

            return request;
        }
    }
}
