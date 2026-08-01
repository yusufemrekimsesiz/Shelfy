\# 🥫 Shelfy



Shelfy, kilerinizdeki ürünleri barkod okutarak takip etmenizi sağlayan bir .NET MAUI mobil uygulamasıdır. Ürün bilgilerini otomatik olarak OpenFoodFacts veritabanından çeker, son kullanma tarihi yaklaşan ürünler için bildirim gönderir.



\## ✨ Özellikler



\- 📷 \*\*Barkod Tarama\*\* — ZXing.Net.MAUI ile hızlı ve güvenilir barkod okuma

\- 🌐 \*\*Otomatik Ürün Bilgisi\*\* — OpenFoodFacts API üzerinden ürün adı, marka ve görsel otomatik çekilir

\- ✍️ \*\*Manuel Ürün Ekleme\*\* — API'de bulunamayan ürünler için elle giriş formu

\- 🔍 \*\*Arama ve Filtreleme\*\* — Ürün adı veya markaya göre anlık arama

\- 🔔 \*\*SKT Bildirimleri\*\* — Son kullanma tarihine 2 gün kala otomatik bildirim

\- 🗑️ \*\*Güvenli Silme\*\* — Yanlışlıkla silmeyi önleyen onay diyaloğu

\- 💾 \*\*Yerel Veri Tabanı\*\* — SQLite ile tamamen çevrimdışı çalışabilen veri kalıcılığı

\- 🌗 \*\*Karanlık Mod Desteği\*\*



\## 🛠️ Kullanılan Teknolojiler



| Katman | Teknoloji |

|---|---|

| UI Framework | .NET MAUI |

| Mimari | MVVM (CommunityToolkit.Mvvm) |

| Barkod Okuma | ZXing.Net.MAUI.Controls |

| Yerel Veri Tabanı | sqlite-net-pcl |

| Ürün Bilgisi API | OpenFoodFacts |

| Bildirimler | Plugin.LocalNotification |

| Dependency Injection | Microsoft.Extensions.DependencyInjection |



\## 📱 Ekranlar



\- \*\*Kilerim (InventoryPage)\*\* — Tüm ürünlerin kart görünümünde listelendiği ana ekran, arama çubuğu ile birlikte

\- \*\*Tarama (ScanPage)\*\* — Kamera üzerinden canlı barkod okuma

\- \*\*Ürün Detayı (ProductDetailsPage)\*\* — Taranan barkoda ait bilgilerin gösterildiği ve SKT/adet girişinin yapıldığı ekran

\- \*\*Elle Ekleme (ManualEntryPage)\*\* — API'de bulunamayan ürünler için manuel giriş formu



\## 🚀 Kurulum



```bash

git clone https://github.com/yusufemrekimsesiz/Shelfy.git

cd Shelfy

dotnet restore

dotnet build

```



Android cihazda çalıştırmak için:

```bash

dotnet build -t:Run -f net10.0-android

```



\## 📋 Gerekli İzinler



\- Kamera (barkod tarama için)

\- İnternet (OpenFoodFacts API için)

\- Bildirim (SKT hatırlatmaları için)



\## 🗂️ Proje Yapısı

Shelfy/

├── Models/ # PantryItem, ProductInfo

├── Services/ # DatabaseService, ProductApiService, NotificationService

├── ViewModels/ # MVVM ViewModel sınıfları

├── Views/ # XAML sayfaları

├── Converters/ # Değer dönüştürücüler

└── Platforms/ # Platform özel yapılandırmalar

\## 📄 Lisans



Bu proje kişisel kullanım amacıyla geliştirilmiştir

