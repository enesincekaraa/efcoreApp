using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class OgretmenController : Controller
    {
        private readonly DataContext _dataContext;

        public OgretmenController(DataContext context)
        {
            _dataContext = context;
            
        }
        public async Task<IActionResult> Index()
        {

            return View(await _dataContext.Ogretmenler.ToListAsync());
        }

        public IActionResult Create() { return View(); }

        [HttpPost]
        public async Task<IActionResult> Create(Ogretmen model) 
        {
            _dataContext.Ogretmenler.Add(model);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) { return NotFound(); }
            var model = await _dataContext.Ogretmenler.FirstOrDefaultAsync(o => o.OgretmenId == id);

            if(model == null) { return NotFound(); }    
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id , Ogretmen model) 
        {
            if(id != model.OgretmenId) { return NotFound(); }

            if(ModelState.IsValid)
            {
                try 
                {
                    _dataContext.Update(model);
                    await _dataContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) 
                {
                 
                    if(!_dataContext.Ogretmenler.Any(o=>o.OgretmenId == model.OgretmenId)) { return NotFound(); }
                    else { throw; }
                
                }
                return RedirectToAction("Index");

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return NotFound(); }

            var model = await _dataContext.Ogretmenler.FindAsync(id);
            if (model == null) { return NotFound(); }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var model = await _dataContext.Ogretmenler.FindAsync(id);
            if (model == null) { return NotFound(); }
            _dataContext.Ogretmenler.Remove(model);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }

    }
}
