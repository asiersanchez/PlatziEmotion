using EmotionPlatzi.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmotionPlatzi.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            // VieBag es un tipo de datos dinámico y le puedo agregar el campo que quiera, inventármelo
            ViewBag.WelcomeMessage = "Hola mundo";
            ViewBag.ValorEntero = 1;

            return View();
        }

        public ActionResult IndexAlt()
        {
            // en lugar de usar el ViewBag usamos un modelo
            var modelo = new Home();
            modelo.WelcomeMessage = "Hola mundo desde el modelo";

            // le pasamos el modelo como parámetro a la vista
            return View(modelo);
        }
    }
}