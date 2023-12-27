using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly DataContext _dataContext;
        public OgrenciController(DataContext dataContext)
        {

            _dataContext = dataContext;

        }
        public async Task<IActionResult> Index() 
        {
            var ogrenciler=await _dataContext.Ogrenciler.ToListAsync();
            return View(ogrenciler);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ogrenci model)
        {
            _dataContext.Ogrenciler.Add(model);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id) 
        {
            if(id == null) 
            {
                return NotFound();
            }
           
            var model=await _dataContext.Ogrenciler.Include(o=>o.KursKayitlari).ThenInclude(o=>o.Kurs).FirstOrDefaultAsync(o=>o.OgrenciId == id);

            if(model == null) { return NotFound(); }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Ogrenci model) 
        {
            if(id != model.OgrenciId) { return NotFound(); }

            if (ModelState.IsValid) 
            {
                try 
                {
                    _dataContext.Update(model);
                    await _dataContext.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException) 
                {
                    if(!_dataContext.Ogrenciler.Any(o=>o.OgrenciId == model.OgrenciId)) 
                    {
                        return NotFound();
                    }
                    else { throw; }
                }
            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id) 
        {
            if (id == null) { return NotFound(); }

            var model=await _dataContext.Ogrenciler.FindAsync(id);
            if (model == null) { return NotFound(); }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id) 
        {
            var model = await _dataContext.Ogrenciler.FindAsync(id);
            if(model == null) { return NotFound(); }    
            _dataContext.Ogrenciler.Remove(model);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
