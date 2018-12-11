﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EditorModel.Renderers;
using EditorModel.Selections;

namespace SimpleEditor.Controls
{
    public partial class ShadowStyleEditor : UserControl, IEditor<Selection>
    {
        private Selection _selection;
        private int _updating;

        public event EventHandler<ChangingEventArgs> StartChanging = delegate { };
        public event EventHandler<EventArgs> Changed = delegate { };

        public ShadowStyleEditor()
        {
            InitializeComponent();
        }

        public void Build(Selection selection)
        {
            // check visibility
            Visible = selection.ForAll(f => RendererDecorator.ContainsType(f.Renderer, typeof(ShadowRenderer))); 
            if (!Visible) return; // do not build anything

            // remember editing object
            _selection = selection;

            // get list of objects
            var shadowStyles = _selection.Select(f => 
                (ShadowRenderer)RendererDecorator.GetDecorator(f.Renderer, typeof(ShadowRenderer))).ToList();

            // copy properties of object to GUI
            _updating++;

            nudOpacity.Value = shadowStyles.GetProperty(f => (decimal)f.Opacity);
            nudOffsetX.Value = shadowStyles.GetProperty(f => (decimal)f.Offset.X);
            nudOffsetY.Value = shadowStyles.GetProperty(f => (decimal)f.Offset.Y);

            _updating--;
        }

        private void UpdateObject()
        {
            if (_updating > 0) return; // we are in updating mode

            // fire event
            StartChanging(this, new ChangingEventArgs("Shadow Fill Style"));

            // get list of objects
            var shadowStyles = _selection.Select(f =>
                (ShadowRenderer)RendererDecorator.GetDecorator(f.Renderer, typeof(ShadowRenderer))).ToList();

            // send values back from GUI to object
            shadowStyles.SetProperty(f => f.Opacity = (int)nudOpacity.Value);
            shadowStyles.SetProperty(f => f.Offset = new PointF((float)nudOffsetX.Value, f.Offset.Y));
            shadowStyles.SetProperty(f => f.Offset = new PointF(f.Offset.X, (float)nudOffsetY.Value));

            // fire event
            Changed(this, EventArgs.Empty);
        }

        private void nudOpacity_ValueChanged(object sender, EventArgs e)
        {
            UpdateObject();
        }
    }
}
