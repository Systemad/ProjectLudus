using Microsoft.AspNetCore.Components;

namespace Ludus.Client.Pages;

public partial class Home : ComponentBase
{
    private string searchText = string.Empty;
    private int currentPage = 1;
    private int pageSize = 4;
    private List<Card> cards;

    protected override void OnInitialized()
    {
        // Example card data
        cards = new List<Card>
        {
            new Card { Title = "Card 1", Description = "Description for Card 1" },
            new Card { Title = "Card 2", Description = "Description for Card 2" },
            new Card { Title = "Card 3", Description = "Description for Card 3" },
            new Card { Title = "Card 4", Description = "Description for Card 4" },
            new Card { Title = "Card 5", Description = "Description for Card 5" },
            new Card { Title = "Card 6", Description = "Description for Card 6" },
            new Card { Title = "Card 7", Description = "Description for Card 7" },
            new Card { Title = "Card 8", Description = "Description for Card 8" },
        };
    }

    private List<Card> filteredCards =>
        cards
            .Where(card =>
                string.IsNullOrEmpty(searchText)
                || card.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)
            )
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();

    private bool CanGoPrev => currentPage > 1;
    private bool CanGoNext => (currentPage * pageSize) < cards.Count;

    private void PrevPage() => currentPage--;

    private void NextPage() => currentPage++;

    public class Card
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
