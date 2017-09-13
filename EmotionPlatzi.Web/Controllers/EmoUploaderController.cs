using EmotionPlatzi.Web.Models;
using EmotionPlatzi.Web.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmotionPlatzi.Web.Controllers
{
    public class EmoUploaderController : Controller
    {
        //Para no dejarlo quemado en el código no le asigno valor
        // creamos un constructor para el controler y leemos el valor desde appsettings
        string serverFolderPath;
        
        // para enviar el archivo a la api, usamo emotionHelper
        EmotionHelper emoHelper;
        string key;

        //creamos el datacontext para guardar el emoPicture en la BD
        EmotionPlatziWebContext db = new EmotionPlatziWebContext();

        public EmoUploaderController()
        {
            // este parámetro está en web.config, en appsettings
            serverFolderPath = ConfigurationManager.AppSettings["UPLOAD_DIR"];
            key = ConfigurationManager.AppSettings["EMOTION_KEY"];
            emoHelper = new EmotionHelper(key);
        }

        // GET: EmoUploader
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Index(HttpPostedFileBase file)
        {
            // con la interrogación le añado la condición de que no sea nulo como si pusiera (file =! null && file.ContentLength > 0)
            if (file?.ContentLength > 0)
            {
                // como el nombre de archi que nos pasan puede ser muy común, se podría sobreescribir
                // para evitar esto generamos cademas aleatorias preservando la extensión del archivo
                var pictureName = Guid.NewGuid().ToString();
                pictureName = pictureName + Path.GetExtension(file.FileName);

                // ahora concatenamos el nombre con la ruta
                // como la ruta física puede ser cualquier parte del equipo usamos server.MapPath que devuelve una ruta absoluta pasándole una ruta relativa (ruta web)
                var route = Server.MapPath(serverFolderPath);
                route = route + "\\" + pictureName;

                file.SaveAs(route);

                var emoPicture =  await emoHelper.DetectAndExtracFacesAsync(file.InputStream);
                emoPicture.Name = file.FileName;
                emoPicture.Path = serverFolderPath + "/" + pictureName;

                try
                {
                    db.EmoPictures.Add(emoPicture);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }

                try
                {
                    return RedirectToAction("Details", "EmoPictures", new { id = emoPicture.Id });
                    //return RedirectToAction("Index", "EmoPictures");
                }
                catch (Exception)
                {

                    throw;
                }
                
            }
            return View();
        }
    }
}