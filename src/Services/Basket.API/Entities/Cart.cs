namespace Basket.API.Entities;

public class Cart
{
    public string UserName { get; set; } = string.Empty;
    public List<CartItem> Items { get; set; } = [];

    public Cart()
    {
    }

    public Cart(string userName)
    {
        UserName = userName;
    }

    public decimal TotalPrice => Items.Sum(item => item.ProductPrice * item.Quantity);
}
