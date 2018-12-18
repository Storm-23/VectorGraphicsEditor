using System;

namespace EditorModel.Renderers
{
    /// <summary>
    /// ����� ���������� ��� ������������ ������
    /// </summary>
    [Serializable]
    public abstract class RendererDecorator : Renderer
    {
        private readonly Renderer _renderer;

        protected RendererDecorator(Renderer renderer)
        {
            _renderer = renderer;
        }

        /// <summary>
        /// ���� ��� ���������� � ������� ������������ �����������
        /// </summary>
        /// <param name="renderer">������ �� �������� ������</param>
        /// <param name="decoratorType">��� ���������� ��� ������</param>
        /// <returns>True - ��� � ������� ������</returns>
        public static bool ContainsType(Renderer renderer, Type decoratorType)
        {
            var rendererDecorator = renderer as RendererDecorator;
            if (rendererDecorator == null) return false;
            return rendererDecorator.GetType() == decoratorType || 
                ContainsType(rendererDecorator._renderer, decoratorType);
        }

        public static Renderer GetDecorator(Renderer renderer, Type decoratorType)
        {
            var rendererDecorator = renderer as RendererDecorator;
            if (rendererDecorator == null) return null;
            if (rendererDecorator.GetType() == decoratorType) return renderer;
            return GetDecorator(rendererDecorator._renderer, decoratorType);            
        }

        /// <summary>
        /// ���������� ��������� ��������, �� ������� ������������� ����������
        /// </summary>
        /// <param name="renderer">�������� ��� ������������</param>
        /// <returns>��������� ��������</returns>
        public static Renderer GetBaseRenderer(Renderer renderer)
        {
            var rendererDecorator = renderer as RendererDecorator;
            return rendererDecorator == null ? renderer : GetBaseRenderer(rendererDecorator._renderer);
        }

        public static bool ContainsAnyDecorator(Renderer renderer)
        {
            return renderer as RendererDecorator != null;
        }
    }
}