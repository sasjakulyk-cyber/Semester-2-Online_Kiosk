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
            //List<CartItem> list = new List<CartItem>();
            //list.Add(new CartItem(0, 1, 2, 5.00m));
            //list.Add(new CartItem(0, 2, 3, 10.00m));
            //list.Add(new CartItem(0, 3, 1, 15.00m));
            //return list;
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
