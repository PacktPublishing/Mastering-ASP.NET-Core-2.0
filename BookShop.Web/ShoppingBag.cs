using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using BookShop.WebComponents;

namespace BookShop.Web
{
    [Serializable]
    public class ShoppingBag
    {
        public const string SessionKey = "cart";

        private readonly IDictionary<int, int> _quantities = new Dictionary<int, int>();

        public static ShoppingBag Get(HttpContext ctx)
        {
            return ctx.Session.GetOrAdd<ShoppingBag>(SessionKey, () => new ShoppingBag());
        }

        public int this[int bookId]
        {
            get
            {
                return this._quantities[bookId];
            }
        }

        public IEnumerable<int> BookIds
        {
            get
            {
                return this._quantities.Keys;
            }
        }

        public void Clear()
        {
            this._quantities.Clear();
        }

        public int Add(int id)
        {
            if (this._quantities.ContainsKey(id) == true)
            {
                this._quantities[id]++;
            }
            else
            {
                this._quantities[id] = 1;
            }

            return this._quantities[id];
        }

        public int Remove(int id)
        {
            int quantity;

            this._quantities.TryGetValue(id, out quantity);

            quantity--;

            if (quantity > 0)
            {
                this._quantities[id] = quantity;
            }
            else
            {
                this._quantities.Remove(id);
            }

            return quantity;
        }

        public int Quantity(int id)
        {
            return this._quantities[id];
        }

        public void Save(HttpContext ctx)
        {
            ctx.Session.Set(SessionKey, this);
        }

        public int Count
        {
            get
            {
                return this._quantities.Values.Sum();
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this._quantities.Count == 0;
            }
        }
    }
}
