using efcoreApp.Data;
using efcoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class KursController : Controller
    {
        private readonly DataContext _dataContext;

        public KursController(DataContext context)
        {
            _dataContext = context;
            
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Ogretmenler=new SelectList(await _dataContext.Ogretmenler.ToListAsync(),"OgretmenId","AdSoyad");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(KursViewModel model) 
        {
            if (ModelState.IsValid)
            {
                _dataContext.Kurslar.Add(new Kurs() { KursId = model.KursId, Baslik = model.Baslik, OgretmenId = model.OgretmenId });
                await _dataContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Ogretmenler = new SelectList(await _dataContext.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");

            return View(model);
         
        }

        public async Task<IActionResult> Index()
        {
            var kurslar=await _dataContext.Kurslar.Include(k=>k.Ogretmen).ToListAsync();

            return View(kurslar);
        }

        public async Task<IActionResult> Edit(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await _dataContext.Kurslar.Include(k=>k.KursKayitlari).ThenInclude(k=>k.Ogrenci).Select(k=>new KursViewModel 
            {
                KursId=k.KursId,
                Baslik=k.Baslik,
                OgretmenId=k.OgretmenId,
                KursKayitlari=k.KursKayitlari
            }).FirstOrDefaultAsync(m => m.KursId == id);
            if (model == null) { return NotFound(); }
            ViewBag.Ogretmenler = new SelectList(await _dataContext.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");

            return View(model); 

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,KursViewModel model)
        {
            if (id != model.KursId) { return View(model); }
            if (ModelState.IsValid) 
            {
                try
                {
                    _dataContext.Update(new Kurs() { KursId=model.KursId,Baslik=model.Baslik,OgretmenId=model.OgretmenId});
                    await _dataContext.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException) 
                {
                    if (!_dataContext.Kurslar.Any(k => k.KursId == model.KursId)) 
                    {
                        return NotFound();

                    }
                    else { throw; }
                   
                }
                return RedirectToAction("Index");
            }
            ViewBag.Ogretmenler = new SelectList(await _dataContext.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");

            return View(model);
            

        }

        public async Task<IActionResult> Delete(int? id) 
        {
            if (id == null) { return NotFound(); }

            var model = await _dataContext.Kurslar.FindAsync(id);
            if(model == null) { return NotFound(); }
            return View(model);

        }


        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id) 
        {
            var model = await _dataContext.Kurslar.FindAsync(id);
            if(model == null) { return NotFound(); };
            _dataContext.Kurslar.Remove(model);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
