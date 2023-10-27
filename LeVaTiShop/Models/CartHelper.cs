using LeVaTiShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public static class CartHelper
{
    private const string CartSessionKey = "Cart";

    public static List<CartItem> GetCartItems(HttpContextBase context)
    {
        var cartItems = context.Session[CartSessionKey] as List<CartItem>;
        return cartItems ?? new List<CartItem>();
    }


    public static void AddToCart(HttpContextBase context, CartItem item)
    {
        var cartItems = GetCartItems(context);
        var existingItem = cartItems.FirstOrDefault(i => i.ID == item.ID);

        if (existingItem != null)
        {
            existingItem.quantity += item.quantity;
        }
        else
        {
            cartItems.Add(item);
        }

        SaveCartItems(context, cartItems);
    }

    public static void RemoveFromCart(HttpContextBase context, int id)
    {
        var cartItems = GetCartItems(context);
        var itemToRemove = cartItems.FirstOrDefault(i => i.ID == id);

        if (itemToRemove != null)
        {
            cartItems.Remove(itemToRemove);
            SaveCartItems(context, cartItems);
        }
    }
    public static void UpdateCartItemQuantity(HttpContextBase context, int id, int newQuantity)
    {
        var cartItems = GetCartItems(context);
        var itemToUpdate = cartItems.FirstOrDefault(i => i.ID == id);

        if (itemToUpdate != null)
        {
            itemToUpdate.UpdateQuantity(newQuantity);
            SaveCartItems(context, cartItems);
        }
    }
    public static void ClearCart(HttpContextBase context)
    {
        context.Session.Remove(CartSessionKey);
    }

    private static void SaveCartItems(HttpContextBase context, List<CartItem> cartItems)
    {
        context.Session[CartSessionKey] = cartItems;
    }
}