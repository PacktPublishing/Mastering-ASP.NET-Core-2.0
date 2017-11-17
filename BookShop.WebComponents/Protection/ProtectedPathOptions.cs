using Microsoft.AspNetCore.Http;

namespace BookShop.WebComponents.Protection
{
    public class ProtectedPathOptions
    {
        public PathString Path { get; set; }
        public string PolicyName { get; set; }
    }
}
