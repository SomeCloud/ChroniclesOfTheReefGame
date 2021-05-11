using Microsoft.Xna.Framework;
using System;

namespace GraphicsLibrary
{
    public interface IPrimitiveTexture: ITexture, IDisposable
    {

        /// <summary>  
        /// Цвет заливки текстуры
        /// </summary> 
        public Color FillColor { get; set; }
        /// <summary>  
        /// Цвет границ текстуры
        /// </summary> 
        public Color BorderColor { get; set; }

        /// <summary>  
        /// Параметр отрисовки текстуры
        /// </summary> 
        public bool IsDraw { get; set; }
        /// <summary>  
        /// Параметр заливки текстуры
        /// </summary> 
        public bool IsFill { get; set; }

        /// <summary>  
        /// Заполнение текстуры указанным цветом
        /// </summary> 
        public void FillTextureByColor(Color fillColor);
        /// <summary>  
        /// Отрисовка границ текстуры указанным цветом
        /// </summary> 
        public void DrawTextureBorder(Color borderColor);
        public new void Dispose();
    }
}
