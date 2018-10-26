using Microsoft.VisualStudio.TestTools.UnitTesting;
using CTS_Web_App.Controllers;
using System.Web.Mvc;
using System.Xml.Linq;
using CTS_Web_App;
using System.Collections.Generic;
using System.Linq;

namespace CTS.WebApp.Tests
{
    [TestClass]
    public class ControllerTests
    {
        public string CarXMLFragment
        {
            get
            {
                return LoadCarsFromFile().ToString();
            }
        }

        [TestMethod]
        public void TestDetailsView()
        {
            var controller = new CarController();
            controller.Cars = CarXMLFragment;
            var result = controller.Details(2) as ViewResult;
            Assert.AreEqual("Details", result.ViewName);

        }


        [TestMethod]
        public void TestDetailsViewData()
        {
            var controller = new CarController();
            controller.Cars = CarXMLFragment;
            var result = controller.Details(2) as ViewResult;
            var car = (Car)result.ViewData.Model;
            Assert.AreEqual("3", car.Name);
        }


        [TestMethod]
        public void TestCreateViewData()
        {
            var controller = new CarController();
            XElement carsBefore = XElement.Parse(CarXMLFragment);

            var numCarsBefore = carsBefore.Descendants("Car").Count();
            controller.Cars = carsBefore.ToString();
            var newCar = new Car() { Name = "New Car", Year = "1111", Type = "Saloon" };
            var result = controller.Create(newCar) as ViewResult;
            XElement carsAfter = XElement.Parse(controller.Cars);
            var numCarsAfter = carsAfter.Descendants("Car").Count();

            Assert.AreEqual(1, numCarsAfter - numCarsBefore);
        }

        private XElement LoadCarsFromFile()
        {
            return XElement.Load("Cars.xml");
        }
    }
}
