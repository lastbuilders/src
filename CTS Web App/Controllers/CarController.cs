using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace CTS_Web_App.Controllers
{
    public class CarController : Controller
    {
        public string Cars { get; set; }

        // GET: Car
        public ActionResult Index()
        {

            XElement element = LoadCarsFromFile();

            IEnumerable<Car> cars = LoadCars(element);

            return View(cars);
        }


        // GET: Applications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var element = LoadCarsFromFile();
            var cars = from a in element.Elements().Where(x => x.Element("ID").Value == id.ToString())
                       select new Car
                       {
                           Id = Convert.ToInt32(a.Element("ID").Value),
                           Name = a.Element("Name").Value,
                           Type = a.Element("Type").Value,
                           Year = a.Element("Year").Value,
                       };

            if (cars == null)
            {
                return HttpNotFound();
            }

            return View("Details", cars.FirstOrDefault());
        }

        // GET: Applications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Car car)
        {
            if (ModelState.IsValid)
            {
                var maxId = 1;
                if (car == null)
                {
                    return HttpNotFound();
                }
                var element = LoadCarsFromFile();

                if (element.Descendants("Car").Count() > 0 )
                {
                    maxId = element.Descendants("Car").Max(x => Convert.ToInt32(x.Element("ID").Value)) + 1;
                }
               
                element.Add(
                    new XElement("Car",
                    new XElement("ID", maxId),
                    new XElement("Type", car.Type),
                    new XElement("Name", car.Name),
                    new XElement("Year", car.Year)
                    )
                    );

                SaveCars(element);


                return RedirectToAction("Index");
            }

            return View(car);
        }

        // GET: Applications/Edit/5
        public ActionResult Edit(int? id)
        {
            return GetCar(id);
        }

        private ActionResult GetCar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var element = LoadCarsFromFile();
            var cars = from a in element.Elements().Where(x => x.Element("ID").Value == id.ToString())
                       select new Car
                       {
                           Id = Convert.ToInt32(a.Element("ID").Value),
                           Name = a.Element("Name").Value,
                           Type = a.Element("Type").Value,
                           Year = a.Element("Year").Value,
                       };

            if (cars == null)
            {
                return HttpNotFound();
            }
            return View(cars.FirstOrDefault());
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Car car)
        {
            if (ModelState.IsValid)
            {
                var element = LoadCarsFromFile();
                XElement foundCar = SelectCar(car.Id, element);
                foundCar.Element("Name").Value = car.Name;
                foundCar.Element("Type").Value = car.Type;
                foundCar.Element("Year").Value = car.Year;
                SaveCars(element);
                return RedirectToAction("Index");
            }
            return View(car);
        }



        // GET: Applications/Delete/5
        public ActionResult Delete(int? id)
        {
            return GetCar(id);
        }

        // POST: UserTenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var element = LoadCarsFromFile();
            var itemsToRemove = SelectCar(id, element);


            if (itemsToRemove == null)
            {
                return HttpNotFound();
            }

            itemsToRemove.Remove();

            SaveCars(element);

            return RedirectToAction("Index");
        }


        #region Helper Routines
        private void SaveCars(XElement element)
        {
            if (!string.IsNullOrEmpty(Cars))
            {
                Cars = element.ToString();
            }
            else
            {
                element.Save(Server.MapPath("~/Cars.xml"));
            }
        }

        private static IEnumerable<Car> LoadCars(XElement element)
        {
            return from a in element.Descendants("Car")
                   select new Car
                   {
                       Id = Convert.ToInt32(a.Element("ID").Value),
                       Name = a.Element("Name").Value,
                       Type = a.Element("Type").Value,
                       Year = a.Element("Year").Value,
                   };
        }

        private static XElement SelectCar(int Id, XElement element)
        {
            return element.Elements().Where(x => x.Element("ID").Value == Id.ToString()).FirstOrDefault();
        }

        private XElement LoadCarsFromFile()
        {
            if (!string.IsNullOrEmpty(Cars))
            {
                return XElement.Parse(Cars);
            }

            return XElement.Load(Server.MapPath("~/Cars.xml"));
        }
        #endregion
    }
}