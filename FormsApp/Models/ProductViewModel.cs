namespace FormsApp.Models
{
    public class ProductViewModel
    {
        public List<Product> Products {get; set; } = null!;  //Product listesini paketleme
        public List<Category> Categories {get; set; } = null!; // Category listesini paketleme
        public string? SelectedCategory {get; set; } //seçilen kategori
    }
}