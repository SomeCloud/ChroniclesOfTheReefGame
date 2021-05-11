using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using CommonPrimitivesLibrary;

namespace GraphicsLibrary
{
    public interface ITexture: IDisposable
    {

        /// <summary>  
        /// Размер текстуры
        /// </summary> 
        public ASize Size { get; }
        /// <summary>  
        /// Затемненный экземпляр текстуры
        /// </summary> 
        public Texture2D DarkenedTexture { get; }
        public Texture2D Texture { get; }
        public GraphicsDevice GraphicsDevice { get; }

        /// <summary>  
        /// Отрисовка линии на текстуре
        /// </summary> 
        public void DrawLine(APoint A, APoint B, Color color);
        /// <summary>  
        /// Заполнение области указаным цветом
        /// </summary> 
        public void FillPixels(APoint centerPoint, Color fillColor);
        /// <summary>  
        /// Копия прямоугольной области от исходной текстуры
        /// </summary> 
        public Texture2D RectangleTexture(ARectangle rectangle);
        /// <summary>  
        /// Копия прямоугольной области от исходной затемненной текстуры
        /// </summary> 
        public Texture2D RectangleDarkenedTexture(ARectangle rectangle);
        /// <summary>  
        /// Копия исходной текстуры
        /// </summary> 
        public Texture2D Clone();
        /// <summary>  
        /// Заполнение исходной текстуры с указанного источника
        /// </summary> 
        public void FillBySource(Texture2D texture);
        /// <summary>  
        /// Заменить пиксели цвета источника на новые цвет
        /// </summary> 
        public void ReplacePixels(Color source, Color color);
        public new void Dispose();
    }
}
