using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace UABS.AvaloniaUI
{
    public partial class ImagePreviewControl : UserControl
    {
        private Avalonia.Point _lastPointer;
        private bool _isDragging = false;
        private double _scale = 10;
        private const double MinScale = 5;
        private const double MaxScale = 100.0;

        public ImagePreviewControl()
        {
            InitializeComponent();

            this.AttachedToVisualTree += (_, __) =>
            {
                ForegroundCanvas.PointerPressed += OnPointerPressed;
                ForegroundCanvas.PointerMoved += OnPointerMoved;
                ForegroundCanvas.PointerReleased += OnPointerReleased;
                ForegroundCanvas.PointerWheelChanged += OnPointerWheelChanged;

                // Initial placement
                Canvas.SetLeft(ForegroundImage, 0);
                Canvas.SetTop(ForegroundImage, 0);
                ForegroundImage.RenderTransformOrigin = new RelativePoint(0, 0, RelativeUnit.Absolute);
                ForegroundImage.RenderTransform = new ScaleTransform(_scale, _scale);
            };
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            _isDragging = true;
            _lastPointer = e.GetPosition(ForegroundCanvas);
            ForegroundCanvas.Cursor = new Cursor(StandardCursorType.Hand);
            e.Pointer.Capture(ForegroundCanvas);
        }

        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (!_isDragging) return;

            var pos = e.GetPosition(ForegroundCanvas);
            var delta = pos - _lastPointer;

            Canvas.SetLeft(ForegroundImage, Canvas.GetLeft(ForegroundImage) + delta.X);
            Canvas.SetTop(ForegroundImage, Canvas.GetTop(ForegroundImage) + delta.Y);

            _lastPointer = pos;
        }

        private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            _isDragging = false;
            ForegroundCanvas.Cursor = new Cursor(StandardCursorType.Arrow);
            e.Pointer.Capture(null);
        }

        private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            var oldScale = _scale;
            _scale *= 1.0 + e.Delta.Y * 0.1;
            _scale = Math.Clamp(_scale, MinScale, MaxScale);

            var mousePos = e.GetPosition(ForegroundCanvas);
            var left = Canvas.GetLeft(ForegroundImage);
            var top = Canvas.GetTop(ForegroundImage);

            ForegroundImage.RenderTransform = new ScaleTransform(_scale, _scale);

            Canvas.SetLeft(ForegroundImage, left - (mousePos.X - left) * (_scale / oldScale - 1));
            Canvas.SetTop(ForegroundImage, top - (mousePos.Y - top) * (_scale / oldScale - 1));
        }
    }
}