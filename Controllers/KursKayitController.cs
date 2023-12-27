using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class KursKayitController : Controller
    {
        private readonly DataContext _dataContext;

        public KursKayitController(DataContext context)
        {
            _dataContext = context;
            
        }
        public async Task<IActionResult> Index() 
        {
            var kursKayitlari= await _dataContext.KursKayitlari.Include(x=>x.Ogrenci).Include(x=>x.Kurs).ToListAsync();
            return View(kursKayitlari);
        }
       public async Task<IActionResult> Create() 
       {
            ViewBag.Ogrenciler =new SelectList(await _dataContext.Ogrenciler.ToListAsync(),"OgrenciId","AdSoyad");
            ViewBag.Kurslar = new SelectList(await _dataContext.Kurslar.ToListAsync(), "KursId", "Baslik");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KursKayit model)
        {
            model.KayitTarihi = DateTime.Now;
           _dataContext.KursKayitlari.Add(model);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }



    }
}
