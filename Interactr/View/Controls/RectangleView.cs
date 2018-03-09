﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Interactr.Reactive;
using Interactr.View.Framework;

namespace Interactr.View.Controls
{
    /// <summary>
    /// A view that displays rectangles.
    /// </summary>
    public class RectangleView : UIElement
    {
        #region BorderColor
        /// <summary>
        /// The color of the border of the rectangle.
        /// </summary>
        public Color BorderColor
        {
            get => _borderColor.Value;
            set => _borderColor.Value = value;
        }
        private readonly ReactiveProperty<Color> _borderColor = new ReactiveProperty<Color>();
        public IObservable<Color> BorderColorChanged => _borderColor.Changed;
        #endregion

        #region BackgroundColor
        /// <summary>
        /// The color that is used to fill the background of the rectangle.
        /// </summary>
        public Color BackgroundColor
        {
            get => _backgroundColor.Value;
            set => _backgroundColor.Value = value;
        }
        private readonly ReactiveProperty<Color> _backgroundColor = new ReactiveProperty<Color>();
        public IObservable<Color> BackgroundColorChanged => _backgroundColor.Changed;
        #endregion
        
        #region BorderWidth
        /// <summary>
        /// The width of the border in pixels.
        /// </summary>
        public float BorderWidth
        {
            get => _borderWidth.Value;
            set => _borderWidth.Value = value;
        }
        private readonly ReactiveProperty<float> _borderWidth = new ReactiveProperty<float>();
        public IObservable<float> BorderWidthChanged => _borderWidth.Changed;
        #endregion

        public RectangleView()
        {
            // Default values.
            BorderColor = Color.Black;
            BackgroundColor = Color.Transparent;
            BorderWidth = 1;

            SetupObservables();
        }

        private void SetupObservables()
        {
            // When a property changes, repaint.
            Observable.Merge(
                BorderColorChanged.Select(_ => Unit.Default),
                BackgroundColorChanged.Select(_ => Unit.Default),
                BorderWidthChanged.Select(_ => Unit.Default)
            ).Subscribe(_ => Repaint());
        }

        public override void PaintElement(Graphics g)
        {
            g.DrawRectangle(
                new Pen(BorderColor, BorderWidth), 
                BorderWidth / 2, BorderWidth / 2, 
                Width - BorderWidth, Height - BorderWidth
            );
        }
    }
}