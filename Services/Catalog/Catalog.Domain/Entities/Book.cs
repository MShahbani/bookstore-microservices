namespace Catalog.Domain.Entities;

public class Book
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }

    private Book()
    {
    }

    public Book(string title, string author, decimal price, int stock)
    {
        Id = Guid.NewGuid();
        SetTitle(title);
        SetAuthor(author);
        SetPrice(price);
        SetStock(stock);
    }

    public void Update(string title, string author, decimal price, int stock)
    {
        SetTitle(title);
        SetAuthor(author);
        SetPrice(price);
        SetStock(stock);
    }

    public bool HasEnoughStock(int quantity)
    {
        return quantity > 0 && Stock >= quantity;
    }

    public bool ReserveStock(int quantity)
    {
        if (!HasEnoughStock(quantity))
        {
            return false;
        }

        Stock -= quantity;
        return true;
    }

    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty.");
        }

        Title = title.Trim();
    }

    private void SetAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            throw new ArgumentException("Author cannot be empty.");
        }

        Author = author.Trim();
    }

    private void SetPrice(decimal price)
    {
        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
        }

        Price = price;
    }

    private void SetStock(int stock)
    {
        if (stock < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(stock), "Stock cannot be negative.");
        }

        Stock = stock;
    }
}