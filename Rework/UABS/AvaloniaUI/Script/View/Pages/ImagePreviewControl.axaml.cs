using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using UABS.Util;

namespace UABS.AvaloniaUI
{
    public partial class ImagePreviewControl : UserControl
    {
        private Avalonia.Point _lastPointer;
        private bool _isDragging = false;
        private double _scale = 10;
        private const double MinScale = 1;
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

                ResetImageState();

                ForegroundImage.PropertyChanged += (_, e) =>
                {
                    if (e.Property == Image.SourceProperty)
                    {
                        Log.Info("Reset image state");
                        ResetImageState();
                    }
                };
            };
        }

        private void ResetImageState()
        {
            _lastPointer = new Avalonia.Point(0, 0);
            _isDragging = false;
            // _scale = CalculateFitScale();

            // Reset transform
            Canvas.SetLeft(ForegroundImage, 0);
            Canvas.SetTop(ForegroundImage, 0);
            ForegroundImage.RenderTransformOrigin = new RelativePoint(0, 0, RelativeUnit.Absolute);
            // ForegroundImage.RenderTransform = new ScaleTransform(_scale, _scale);

            // Center image in canvas
            CenterAndFitAfterLayout();
        }

        private void CenterAndFitAfterLayout()
        {
            void Handler(object? s, EventArgs e)
            {
                // Make sure canvas has a real size
                if (ForegroundCanvas.Bounds.Width <= 0 || ForegroundCanvas.Bounds.Height <= 0)
                    return;

                // Compute adaptive scale
                _scale = CalculateFitScale();

                // Reset transform
                ForegroundImage.RenderTransform = new ScaleTransform(_scale, _scale);

                ForegroundCanvas.LayoutUpdated -= Handler;

                CenterBySimulatedZoom();
            }

            ForegroundCanvas.LayoutUpdated += Handler;
        }

        private void CenterBySimulatedZoom()
        {
            var center = new Avalonia.Point(
                ForegroundCanvas.Bounds.Width / 2,
                ForegroundCanvas.Bounds.Height / 2);

            // Force recalculation without visual jump
            ApplyZoom(_scale * 0.8, center);
            ApplyZoom(_scale, center);

            ClampImagePosition();
        }

        private void ApplyZoom(double newScale, Avalonia.Point anchor)
        {
            var oldScale = _scale;
            _scale = Math.Clamp(newScale, MinScale, MaxScale);

            var left = Canvas.GetLeft(ForegroundImage);
            var top  = Canvas.GetTop(ForegroundImage);

            ForegroundImage.RenderTransform = new ScaleTransform(_scale, _scale);

            Canvas.SetLeft(
                ForegroundImage,
                left - (anchor.X - left) * (_scale / oldScale - 1));

            Canvas.SetTop(
                ForegroundImage,
                top - (anchor.Y - top) * (_scale / oldScale - 1));
        }

        private double CalculateFitScale()
        {
            if (ForegroundCanvas.Bounds.Width <= 0 || ForegroundCanvas.Bounds.Height <= 0)
                return 1.0; // fallback

            double canvasW = ForegroundCanvas.Bounds.Width;
            double canvasH = ForegroundCanvas.Bounds.Height;

            double imgW = ForegroundImage.Bounds.Width;
            double imgH = ForegroundImage.Bounds.Height;

            double scaleX = canvasW / imgW;
            double scaleY = canvasH / imgH;

            Log.Info($"{scaleX}, {scaleY}");

            double scale = Math.Min(scaleX, scaleY);

            // Optional: leave some margin
            scale *= 0.95;

            // Clamp scale to reasonable min/max
            scale = Math.Clamp(scale, MinScale, MaxScale);

            return scale;
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

            ClampImagePosition();

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
            ApplyZoom(_scale * (1.0 + e.Delta.Y * 0.1),
                e.GetPosition(ForegroundCanvas));

            ClampImagePosition();
        }

        private void ClampImagePosition()
        {
            if (ForegroundImage.Source is null)
                return;

            var canvasW = ForegroundCanvas.Bounds.Width;
            var canvasH = ForegroundCanvas.Bounds.Height;

            var imgW = ForegroundImage.Bounds.Width * _scale;
            var imgH = ForegroundImage.Bounds.Height * _scale;

            var left = Canvas.GetLeft(ForegroundImage);
            var top  = Canvas.GetTop(ForegroundImage);

            double minLeft, maxLeft;
            double minTop, maxTop;

            if (imgW <= canvasW)
            {
                minLeft = maxLeft = (canvasW - imgW) / 2;
            }
            else
            {
                minLeft = canvasW - imgW;
                maxLeft = 0;
            }

            if (imgH <= canvasH)
            {
                minTop = maxTop = (canvasH - imgH) / 2;
            }
            else
            {
                minTop = canvasH - imgH;
                maxTop = 0;
            }

            Canvas.SetLeft(ForegroundImage, Math.Clamp(left, minLeft, maxLeft));
            Canvas.SetTop(ForegroundImage, Math.Clamp(top, minTop, maxTop));
        }
    }
}