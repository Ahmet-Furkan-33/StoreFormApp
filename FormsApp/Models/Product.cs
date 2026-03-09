using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc; //veri ek açıklamaları için gerekli

namespace FormsApp.Models 
{ 
    public class Product //ürün sınıfı tanımlanıyor
    {
        [Display(Name="Ürün Id")]
        
        public int? ProductId { get; set; } //ürün kimliği
        [Display(Name="Ürün Adı")]
        [Required]//ürün adı zorunlu
        [StringLength(100)] //ürün adı en fazla 100 karakter olabilir
        public string Name { get; set; } =null!; //ürün adı değeri boş değer olmayacak anlamına gelir.
        [Display(Name="Ürün Fiyatı")]
        [Required(ErrorMessage ="Lütfen ürün fiyatını giriniz.")]//ürün fiyatı zorunlu
        [Range(0,100000)] //ürün fiyatı 0 ile 100000 arasında olmalı 
        public decimal? Price { get; set; } //ürün fiyatı
        [Display(Name="Ürün Resmi")]
        
        public string? Image { get; set; } //ürün resmi
        public bool IsActive { get; set; } //ürün aktiflik durumu

        [Display(Name="Category")]//sayfada gösterilecek isim(category)
        [Required]//kategori zorunlu
        public int? CategoryId { get; set; } //kategori kimliği
    }
}