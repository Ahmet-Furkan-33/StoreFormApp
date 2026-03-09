namespace FormsApp.Models
{
    public class Repository //veri deposu sınıfı tanımlanıyor
    {
        private static readonly List<Product> _products = new(); //ürün listesi
        private static readonly List<Category> _categories = new(); //kategori listesi
        
        static Repository() //statik yapıcı
        {
            _categories.Add(new Category { CategoryId = 1 , Name = "Telefon"}); //kategori ekleme
          _categories.Add(new Category { CategoryId = 2 , Name = "Bilgisayar"}); //kategori ekleme

          _products.Add(new Product {ProductId=1, Name="İphone 14", Price =40000, Image="1.jpg",IsActive=true, CategoryId=1}); //ürün ekleme

          _products.Add(new Product {ProductId=2, Name="İphone 15", Price =50000, Image="2.jpg",IsActive=true, CategoryId=1}); //ürün ekleme

          _products.Add(new Product {ProductId=3, Name="İphone 16", Price =60000, Image="3.jpg",IsActive=true, CategoryId=1}); //ürün ekleme

          _products.Add(new Product {ProductId=4, Name="İphone 17", Price =70000, Image="4.jpg",IsActive=true, CategoryId=1}); //ürün ekleme

          _products.Add(new Product {ProductId=5, Name="Macbook Air", Price =80000, Image="5.jpg",IsActive=true, CategoryId=2}); //ürün ekleme

          _products.Add(new Product {ProductId=6, Name="Macbook Pro", Price =90000, Image="6.jpg",IsActive=true, CategoryId=2}); //ürün ekleme


             
        }
        public static List<Product> Products //ürünler özelliği
        {
          get
            {
                return _products; //ürün listesini döndür
            }
        }
        public static void CreateProduct(Product entity) //yeni ürün ekleme metodu
        {
            _products.Add(entity); //ürün listesine yeni ürünü(entity) ekle.
        }
        public static void EditProduct(Product updateProduct) //ürün güncelleme metodu
        {
           var entity = _products.FirstOrDefault(p=>p.ProductId == updateProduct.ProductId); //güncellenen ürünün id'sine göre ürün listesinden ürünü al.

           if(entity != null) //ürün bulunduysa
           {
            if(!string.IsNullOrEmpty(updateProduct.Name)) //ürün adı boş değilse
                {
                    entity.Name = updateProduct.Name; //ürün adını güncelle
                }
           
           
           entity.Price = updateProduct.Price ; //ürün fiyatını güncelle
           entity.Image = updateProduct.Image ;// ürün resmini güncelle
           entity.CategoryId = updateProduct.CategoryId ; // ürün kategori numarasını güncelle
           entity.IsActive = updateProduct.IsActive; //ürün aktiflik durumunu güncelle
               
           }
        }
        public static void EditIsActive(Product updateProduct) //ürün aktiflik durumunu güncelleme metodu
        {
           var entity = _products.FirstOrDefault(p=>p.ProductId == updateProduct.ProductId); //güncellenen ürünün id'sine göre ürün listesinden ürünü al.

           if(entity != null) //ürün bulunduysa
           {
             entity.IsActive = updateProduct.IsActive; //ürün aktiflik durumunu güncelle
                         
           }
        }
     
        public static void DeleteProduct(Product deletedProduct) //ürün silme metodu
        {
          var entity = _products.FirstOrDefault(p=>p.ProductId == deletedProduct.ProductId); //silinen ürünün id'sine göre ürün listesinden ürünü al.

          if(entity != null) //ürün bulunduysa
          {
            _products.Remove(entity); //ürünü ürün listesinden sil
          }

        }
            
        public static List<Category> Categories 
        {
            get
            {
                return _categories; //kategori listesini döndür
            }
        }
    }
}