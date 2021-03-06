﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EmotionPlatzi.Web.Models
{
    public class EmotionPlatziWebContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public EmotionPlatziWebContext() : base("name=EmotionPlatziWebContext")
        {
            //Database.SetInitializer<EmotionPlatziWebContext>(new DropCreateDatabaseAlways<EmotionPlatziWebContext>());

            // Si ya hay una versión de producción, no se puede usar esta estrategia, sino por ejemplo, algo de este tipo:
            //Database.SetInitializer<EmotionPlatziWebContext>(new MigrateDatabaseToLatestVersion)......
        }

        public DbSet<EmoPicture> EmoPictures { get; set; }
        public DbSet<EmoFace> EmoFaces { get; set; }
        public DbSet<EmoEmotion> EmoEmotions { get; set; }

        public System.Data.Entity.DbSet<EmotionPlatzi.Web.Models.Home> Homes { get; set; }
    }
}
