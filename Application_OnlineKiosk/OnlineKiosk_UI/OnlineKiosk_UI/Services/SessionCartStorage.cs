using Microsoft.AspNetCore.Http;
using OnlineKiosk_Domain.Data;
using OnlineKiosk_Domain.Interfaces;
using OnlineKiosk_UI.Extensions;

namespace OnlineKiosk_UI.Services
{
    public class SessionCartStorage : ICartStorage
    {
        private readonly IHttpContextAccessor _accessor;

        public SessionCartStorage(
            IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        private ISession Session =>
            _accessor.HttpContext?.Session
            ?? throw new InvalidOperationException(
                "Session is not available.");

        public List<CartItem> GetCart()
        {
            return Session.GetObject<List<CartItem>>("Cart")
                ?? new List<CartItem>();
        }

        public void SaveCart(List<CartItem> items)
        {
            Session.SetObject("Cart", items);
        }

        public void ClearCart()
        {
            Session.Remove("Cart");
        }
    }
}
