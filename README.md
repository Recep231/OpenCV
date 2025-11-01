# OpenCV Yüz Tanıma Uygulaması

C# WinForm ile OpenCV kullanarak gerçek zamanlı yüz tanıma uygulaması.

## 🚀 Özellikler

- ✅ Gerçek zamanlı yüz tespiti
- ✅ Göz tespiti
- ✅ Kamera kontrolü (Başlat/Durdur)
- ✅ Görüntü yakalama ve kaydetme
- ✅ Özelleştirilebilir Haar cascade sınıflandırıcılar

## 📋 Gereksinimler

- .NET Framework 4.8+
- OpenCvSharp4 NuGet paketleri
- Web kamerası

## 🔧 Kurulum

### 1. Projeyi Clone'la

```bash
git clone https://github.com/kullanici-adi/opencv-yuz-tanima.git
cd opencv-yuz-tanima
```

### 2. NuGet Paketlerini Yükle

Package Manager Console'da:

```powershell
Install-Package OpenCvSharp4
Install-Package OpenCvSharp4.runtime.win
Install-Package OpenCvSharp4.Extensions
```

### 3. Haar Cascade Dosyalarını Ekleyin

**Önemli:** Haar cascade dosyaları projeye manuel eklenmelidir:

1. [OpenCV GitHub](https://github.com/opencv/opencv/tree/master/data/haarcascades) adresinden şu dosyaları indirin:
   - `haarcascade_frontalface_default.xml`
   - `haarcascade_eye.xml`

2. İndirdiğiniz dosyaları proje dizinindeki `haarcascades/` klasörüne kopyalayın:

```
opencv-yuz-tanima/
├── haarcascades/
│   ├── haarcascade_frontalface_default.xml
│   └── haarcascade_eye.xml
├── FaceRecognitionForm.cs
└── Program.cs
```

### 4. Derleme ve Çalıştırma

Visual Studio'da:
1. Projeyi açın
2. `Build > Build Solution` seçeneğini tıklayın
3. `Debug > Start Debugging` ile çalıştırın

## 🎮 Kullanım

1. **Kamerayı Başlat**: "Kamera Başlat" butonuna tıklayın
2. **Yüz Tanıma**: Yeşil dikdörtgenler yüzleri, mavi daireler gözleri gösterir
3. **Görüntü Yakala**: "Görüntü Yakala" butonu ile anlık görüntüyü kaydedin
4. **Sınıflandırıcı Yükle**: Farklı Haar cascade dosyalarını yükleyin

## 📁 Proje Yapısı

```
FaceRecognitionApp/
├── FaceRecognitionForm.cs     # Ana form ve işlevler
├── Program.cs                 # Uygulama giriş noktası
├── haarcascades/              # Haar cascade dosyaları
│   ├── haarcascade_frontalface_default.xml
│   └── haarcascade_eye.xml
└── packages.config            # NuGet paketleri
```

## ⚙️ Teknik Detaylar

- **Dil**: C#
- **GUI**: Windows Forms
- **Görüntü İşleme**: OpenCV 4.8
- **Yüz Tespiti**: Haar Cascade Classifier
- **Kamera**: DirectShow arayüzü

## 🐛 Sorun Giderme

### Kamera Açılmıyorsa:
- Kameranın bağlı olduğundan emin olun
- Başka bir uygulamanın kamerayı kullanmadığından emin olun

### Haar Cascade Dosyaları Bulunamıyorsa:
- Dosyaların doğru konumda olduğunu kontrol edin
- Dosya isimlerinin doğru olduğundan emin olun

### Performans Sorunları:
- Daha düşük kamera çözünürlüğü deneyin
- Gereksiz uygulamaları kapatın

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

## 🤝 Katkıda Bulunma

1. Fork edin
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Commit edin (`git commit -m 'Add amazing feature'`)
4. Push edin (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## 📞 İletişim

Proje ile ilgili sorularınız için [issues](https://github.com/kullanici-adi/opencv-yuz-tanima/issues) sayfasını kullanın.

---

**Not:** Bu proje eğitim amaçlıdır ve gerçek zamanlı yüz tanıma sistemleri geliştirmek için temel oluşturur.
