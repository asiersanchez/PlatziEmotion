using EmotionPlatzi.Web.Models;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;

namespace EmotionPlatzi.Web.Util
{
    public class EmotionHelper
    {
        EmotionServiceClient emoClient;

        public EmotionHelper(string key)
        {
            emoClient = new EmotionServiceClient(key);
        }

        public async Task<EmoPicture> DetectAndExtracFacesAsync(Stream imageStream)
        {
            Emotion[] emotions = await emoClient.RecognizeAsync(imageStream);

            var emoPicture = new EmoPicture();
            emoPicture.Faces = ExtractFaces(emotions, emoPicture);

            return emoPicture;
        }

        private ObservableCollection<EmoFace> ExtractFaces(Emotion[] emotions, EmoPicture emoPicture)
        {
            var listaFaces = new ObservableCollection<EmoFace>();

            foreach (var emotion in emotions)
            {
                var emoface = new EmoFace()
                {
                    X = emotion.FaceRectangle.Left,
                    Y = emotion.FaceRectangle.Top,
                    Widht = emotion.FaceRectangle.Width,
                    Height = emotion.FaceRectangle.Height,
                    Picture = emoPicture
                };

                emoface.Emotions = ProcessEmotions(emotion.Scores, emoface);
                listaFaces.Add(emoface);
            }

            return listaFaces;
        }

        private ObservableCollection<EmoEmotion> ProcessEmotions(EmotionScores scores, EmoFace emoface)
        {
            // usamos reflection

            var emotionList = new ObservableCollection<EmoEmotion>();

            //sacamos todas las propiedades que nos interesan de scores. las que son públicas o que son instancias (los campos que no son estáticos)
            var properties = scores.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //con linq le pido lo que sea float
            var filterProperties = from p in properties
                                   where p.PropertyType == typeof(float)
                                   select p;

            var emoType = EmoEmotionEnum.Undetermined;
            foreach (var prop in filterProperties)
            {
                if (Enum.TryParse<EmoEmotionEnum>(prop.Name, out emoType) == false)
                {
                    emoType = EmoEmotionEnum.Undetermined;
                }
                var emoEmotion = new EmoEmotion();
                emoEmotion.Score = (float)prop.GetValue(scores);
                emoEmotion.EmotionType = emoType;
                emoEmotion.Face = emoface;

                emotionList.Add(emoEmotion);
            }

            return emotionList;

        }
    }
}