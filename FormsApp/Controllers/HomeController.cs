using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FormsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FormsApp.Controllers;

public class HomeController : Controller
{
   

    public HomeController()
    {
     
    }

    [HttpGet] 
    public IActionResult Index(string searchString, string category) //ana sayfa aksiyonu
    {
       

        var products = Repository.Products; //ürünleri al

        if(!String.IsNullOrEmpty(searchString)) //arama dizesi(searchString) boş değilse
        {
            ViewBag.searchString = searchString; //arama dizesini ViewBag'e ata.

            products = products.Where(p=>p.Name.ToLower().Contains(searchString)).ToList(); //ürünleri filtrele ve arama dizesini içeren ürünleri al.
        }

        if(!String.IsNullOrEmpty(category) && category != "0") //kategori boş değilse ve kategori "0" değilse
        {
         products = products.Where(p=>p.CategoryId == int.Parse(category)).ToList();   //kategoriye göre ürünleri filtrele.
        }


        var model = new ProductViewModel //ProductViewModel örneği oluştur
        {
            Products = products, //ürünleri ata
            Categories = Repository.Categories, //kategorileri ata
            SelectedCategory = category //seçilen kategoriyi ata
        };
        
        return View(model); //modeli View'e gönder.
    }

    [HttpGet] //Create'in GET aksiyonu
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");//ViewBag'e kategorileri ata
        return View();
    }
    
    [HttpPost]
      
      public async Task<IActionResult> Create(Product model, IFormFile imageFile) //IformFile ile dosya yükleme işlemi için parametre eklenir. 
    { 
        var extension = ""; //dosya uzantısı için boş bir string tanımlanır.
    
        if(imageFile !=  null) //dosya yüklenmişse
        {
            var allowedExtensions = new [] {".jpg",".jpeg",".png"}; //izin verilen dosya uzantıları tanımlandı.
            extension = Path.GetExtension(imageFile.FileName); //dosya uzantısını alır(extension'a).
            if(!allowedExtensions.Contains(extension)) //dosya uzantısı izin verilenler arasında değilse
            {
                ModelState.AddModelError("","lütfen geçerli bir resim dosyası yükleyiniz (.jpg,.jpeg,.png)"); //geçersiz dosya uzantısı için hata mesajı ekler.
            }
        }
        if(ModelState.IsValid) //model girilen bilgiler doğru veya geçerliyse
        {
            if(imageFile != null) //dosya yüklenmişse
            {
                 var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}"); //dosya adı için benzersiz bir isim oluşturur.
                 var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img" , randomFileName); //dosyanın kaydedileceği yolu belirler.
                using(var stream = new FileStream(path,FileMode.Create)) //dosyayı oluşturmak için bir dosya akışı açar.
                {
                     await imageFile.CopyToAsync(stream); //dosyayı belirtilen yola kaydeder.
                }
                model.Image = randomFileName; //resim dosyasının sayfada gösterilmesi için modelin Image özelliğine dosya adını atar.
                model.ProductId = Repository.Products.Count + 1; //modelden ürün numara bilgisine bakıp yeni ürün numarasını atar.
                Repository.CreateProduct(model); //yeni ürünü ekle.
                return RedirectToAction("Index"); //form post edildikten sonra Index sayfasına gönderir yeni ürünü.
            }
        
        }

        ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");//ViewBag'e kategorileri ata
        return View(model); //model  girilen bilgiler yanlış veya geçersizse aynı View'e geri dön.
        
    }
    
        [HttpGet]
        public IActionResult Edit(int? id) //create sayfasından alınan id bilgisine göre düzenleme sayfasını açar.
       {
           if(id == null) //id bilgisi yoksa
           {

            return NotFound(); //id bilgisi yoksa 404 hatası döner.

           } 
           var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id); //id bilgisine göre ürünü (entity e atar)alır.
           if(entity == null) //ürün yoksa
           {
            return NotFound(); //ürün yoksa 404 hatası döner.
           }
           
           ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");//ViewBag'e kategorileri ata
           return View(entity); //ürün(entity) bilgilerini View'e gönder.
       }
       
       [HttpPost]
       public async Task<IActionResult> Edit(int id, Product model, IFormFile? imageFile) //düzenleme sayfasından gelen bilgileri alır.
       {            
           
            if(id != model.ProductId) //id bilgisi modeldeki ürün numarası ile eşleşmiyorsa
            {
                return NotFound(); //404 hatası döner.
            }

            if(ModelState.IsValid) //model girilen bilgiler doğru veya geçerliyse
            {
                 if(imageFile != null) //dosya yüklenmişse
                {
                
                    var extension = Path.GetExtension(imageFile?.FileName); //dosya uzantısını alır(extension'a).
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}"); //dosya adı için benzersiz bir isim oluşturur.
                    var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img" , randomFileName); //dosyanın kaydedileceği yolu belirler.
                    using(var stream = new FileStream(path,FileMode.Create)) //dosyayı oluşturmak için bir dosya akışı açar.
                    {
                     await imageFile.CopyToAsync(stream); //dosyayı belirtilen yola kaydeder.
                    }
                
                   model.Image = randomFileName; //yeni resim dosyasının sayfada gösterilmesi için modelin Image özelliğine dosya adını atar.
                
                }
                Repository.EditProduct(model); //ürünü düzenle
                return RedirectToAction("Index"); //düzenleme işlemi tamamlandıktan sonra Index sayfasına yönlendirir.
            }
                ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");//ViewBag'e kategorileri ata
                return View(model); //model  girilen bilgiler yanlış veya geçersizse aynı View'e geri dön.
                    
            }
            
            public IActionResult Delete(int? id) //ürün silme kısmı
            {
              if(id == null) //id bilgisi yoksa
              {
                 return NotFound(); //id bilgisi yoksa 404 hatası döner.
              }
              var entity = Repository.Products.FirstOrDefault(p=>p.ProductId == id); //id bilgisine göre ürünü (entity e atar)alır.
              if(entity == null) //ürün yoksa
              {
                return NotFound(); //ürün yoksa 404 hatası döner.
              }
              
              return View("DeleteConfirm",entity); //silme onay sayfasını açar.

            }
            [HttpPost]
            public IActionResult Delete(int id, int ProductId) //silme onay sayfasından gelen bilgileri alır.
           {
             if(id != ProductId) //id bilgisi modeldeki ürün numarası ile eşleşmiyorsa
             {
                return NotFound(); //404 hatası döner.
             }
             var entity = Repository.Products.FirstOrDefault(p=>p.ProductId == ProductId); //id bilgisine göre ürünü (entity e atar)alır.
             if(entity == null) //ürün yoksa
            {
         
            return NotFound(); //ürün yoksa 404 hatası döner.
            }    
            Repository.DeleteProduct(entity); // deleteProduct metodunu çağırarak ürünü siler.
            return RedirectToAction("Index"); //silme işlemi tamamlandıktan sonra Index sayfasına yönlendirir.
           }

           public IActionResult EditProducts(List<Product> products) //ürünlerin aktiflik durumunu toplu olarak güncelleme
           {
            foreach(var item in products) //gönderilen ürünler üzerinde döner
            {
                Repository.EditIsActive(item); //repository'deki EditIsActive metodunu çağırarak her bir ürünün aktiflik durumunu günceller.
            }
            return RedirectToAction("Index"); //güncelleme işlemi tamamlandıktan sonra Index sayfasına yönlendirir.
           }
        }
       
    
       
       
              
          
            
            
           
        
