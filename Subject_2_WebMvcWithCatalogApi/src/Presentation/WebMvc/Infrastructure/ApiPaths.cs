namespace WebMvc.Infrastructure
{
    public static class ApiPaths
    { 
        public static class Catalog
        {
            public static string GetAllCatalogItems(string baseUri, int pageIndex, int pageSize, int? type)
            {
                string typeQueryString = "";
                if (type.HasValue)
                {
                    typeQueryString = type.Value.ToString();
                }

                return $"{baseUri}items?catalogTypeId={typeQueryString}&pageIndex={pageIndex}&pageSize={pageSize}";
            }

            public static string GetAllTypes(string baseUri)
            {
                return $"{baseUri}catalogTypes";
            }
        }
    }
}
