﻿using EditorModel.Common;
using System;

namespace EditorModel.Geometry
{
    /// <summary>
    /// Допустимые операции над геометрией
    /// </summary>
    [Serializable]
    [Flags]
    public enum AllowedOperations : uint
    {
        None = 0x0,         // ничего нельзя
        Scale = 0x1,        // может быть растянут
        Size = 0x2,         // может изменять размер по горизонтали и вертикали
        Rotate = 0x4,       // может быть повёрнут
        Select = 0x8,       // может быть выбран
        Skew = 0x10,        // может быть искажён
        Vertex = 0x20,      // может изменять внутренние вершины
        // новые режимы добавлять здесь

        All = 0xffffffff,   // всё можно
    }
    
    /// <summary>
    /// Класс-основа для задания геометрии фигуры
    /// </summary>
    [Serializable]
    public abstract class Geometry
    {
        public string Name { get; set; }

        /// <summary>
        /// Предоставление пути для рисования фигуры
        /// </summary>
        public abstract SerializableGraphicsPath Path { get; }

        /// <summary>
        /// Допустимые операции над геометрией
        /// </summary>
        public abstract AllowedOperations AllowedOperations { get; }

        //public abstract GraphicsPath GetTransformedPath(Figure fig);

        //public abstract RectangleF GetTransformedBounds(Figure fig);

    }
}
