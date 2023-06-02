using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementSystem_BMS.Controllers
{
    public class ChapterController : Controller
    {
        // GET: ChapterController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ChapterController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ChapterController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChapterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChapterController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ChapterController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChapterController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ChapterController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
