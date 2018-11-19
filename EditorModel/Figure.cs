﻿using System;
using System.Drawing.Drawing2D;

namespace EditorModel
{
    /// <summary>
    /// Класс для наследования фигур
    /// </summary>
    [Serializable]
    public class Figure
    {
        /// <summary>
        /// Свойство трансформера фигуры
        /// </summary>
        public Matrix Transform { get; protected set; }

        /// <summary>
        /// Свойство источника геометрии фигуры
        /// </summary>
        public Geometry Geometry { get; set; }

        /// <summary>
        /// Свойство стиля рисования фигуры
        /// </summary>
        public Style Style { get; private set; }

        /// <summary>
        /// Свойство рисовальщика фигуры
        /// </summary>
        public Renderer Renderer { get; private set; }

        /// <summary>
        /// Конструктор фигуры для задания свойств по умолчанию
        /// </summary>
        public Figure()
        {
            Transform = new Matrix();
            Style = new Style();
            Renderer = new Renderer();
        }

        /// <summary>
        /// Предоставление трансформированной геометрии для рисования
        /// </summary>
        /// <returns>Путь для рисования</returns>
        public GraphicsPath GetTransformedPath()
        {
            // создаём копию геометрии фигуры
            var path = (GraphicsPath)Geometry.Path.Clone();
            // трансформируем её при помощи Трансформера
            path.Transform(Transform);
            return path;
        }

    }

}