// Copyright 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
//https://stackoverflow.com/questions/93650/apply-stroke-to-a-textblock-in-wpf
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace MHFZ_Overlay
{

    [ContentProperty("Text")]
    public class OutlinedTextBlock : FrameworkElement
    {
        /// <summary>
        /// Updates the pen.
        /// </summary>
        private void UpdatePen()
        {
            _Pen = new Pen(Stroke, StrokeThickness)
            {
                DashCap = PenLineCap.Round,
                EndLineCap = PenLineCap.Round,
                LineJoin = PenLineJoin.Round,
                StartLineCap = PenLineCap.Round
            };

            InvalidateVisual();
        }
        /// <summary>
        /// The fill property
        /// </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
      "Fill",
      typeof(Brush),
      typeof(OutlinedTextBlock),
      new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>
        /// The stroke property
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
      "Stroke",
      typeof(Brush),
      typeof(OutlinedTextBlock),
      new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender, StrokePropertyChangedCallback));

        private static void StrokePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            (dependencyObject as OutlinedTextBlock)?.UpdatePen();
        }
        /// <summary>
        /// The stroke thickness property
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
      "StrokeThickness",
      typeof(double),
      typeof(OutlinedTextBlock),
      new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsRender, StrokePropertyChangedCallback));
        /// <summary>
        /// The font family property
        /// </summary>
        public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(
      typeof(OutlinedTextBlock),
      new FrameworkPropertyMetadata(OnFormattedTextUpdated));
        /// <summary>
        /// The font size property
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(
      typeof(OutlinedTextBlock),
      new FrameworkPropertyMetadata(OnFormattedTextUpdated));
        /// <summary>
        /// The font stretch property
        /// </summary>
        public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(
      typeof(OutlinedTextBlock),
      new FrameworkPropertyMetadata(OnFormattedTextUpdated));
        /// <summary>
        /// The font style property
        /// </summary>
        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(
      typeof(OutlinedTextBlock),
      new FrameworkPropertyMetadata(OnFormattedTextUpdated));
        /// <summary>
        /// The font weight property
        /// </summary>
        public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(
      typeof(OutlinedTextBlock),
      new FrameworkPropertyMetadata(OnFormattedTextUpdated));
        /// <summary>
        /// The text property
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
      "Text",
      typeof(string),
      typeof(OutlinedTextBlock),
      new FrameworkPropertyMetadata(OnFormattedTextInvalidated));
        /// <summary>
        /// The text alignment property
        /// </summary>
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
      "TextAlignment",
      typeof(TextAlignment),
      typeof(OutlinedTextBlock),
      new FrameworkPropertyMetadata(OnFormattedTextUpdated));
        /// <summary>
        /// The text decorations property
        /// </summary>
        public static readonly DependencyProperty TextDecorationsProperty = DependencyProperty.Register(
      "TextDecorations",
      typeof(TextDecorationCollection),
      typeof(OutlinedTextBlock),
      new FrameworkPropertyMetadata(OnFormattedTextUpdated));
        /// <summary>
        /// The text trimming property
        /// </summary>
        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register(
      "TextTrimming",
      typeof(TextTrimming),
      typeof(OutlinedTextBlock),
      new FrameworkPropertyMetadata(OnFormattedTextUpdated));
        /// <summary>
        /// The text wrapping property
        /// </summary>
        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(
      "TextWrapping",
      typeof(TextWrapping),
      typeof(OutlinedTextBlock),
      new FrameworkPropertyMetadata(TextWrapping.NoWrap, OnFormattedTextUpdated));

        private FormattedText _FormattedText;
        private Geometry _TextGeometry;
        private Pen _Pen;
        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        /// <value>
        /// The font family.
        /// </value>
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
        /// <summary>
        /// Gets or sets the font stretch.
        /// </summary>
        /// <value>
        /// The font stretch.
        /// </value>
        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }
        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        /// <value>
        /// The font style.
        /// </value>
        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }
        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        /// <value>
        /// The font weight.
        /// </value>
        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }
        /// <summary>
        /// Gets or sets the stroke.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        /// <value>
        /// The text alignment.
        /// </value>
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }
        /// <summary>
        /// Gets or sets the text decorations.
        /// </summary>
        /// <value>
        /// The text decorations.
        /// </value>
        public TextDecorationCollection TextDecorations
        {
            get { return (TextDecorationCollection)GetValue(TextDecorationsProperty); }
            set { SetValue(TextDecorationsProperty, value); }
        }
        /// <summary>
        /// Gets or sets the text trimming.
        /// </summary>
        /// <value>
        /// The text trimming.
        /// </value>
        public TextTrimming TextTrimming
        {
            get { return (TextTrimming)GetValue(TextTrimmingProperty); }
            set { SetValue(TextTrimmingProperty, value); }
        }
        /// <summary>
        /// Gets or sets the text wrapping.
        /// </summary>
        /// <value>
        /// The text wrapping.
        /// </value>
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="OutlinedTextBlock"/> class.
        /// </summary>
        public OutlinedTextBlock()
        {
            UpdatePen();
            TextDecorations = new TextDecorationCollection();
        }
        /// <summary>
        /// When overridden in a derived class, participates in rendering operations that are directed by the layout system. The rendering instructions for this element are not used directly when this method is invoked, and are instead preserved for later asynchronous use by layout and drawing.
        /// </summary>
        /// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            EnsureGeometry();

            drawingContext.DrawGeometry(null, _Pen, _TextGeometry);
            drawingContext.DrawGeometry(Fill, null, _TextGeometry);
        }
        /// <summary>
        /// When overridden in a derived class, measures the size in layout required for child elements and determines a size for the <see cref="T:System.Windows.FrameworkElement" />-derived class.
        /// </summary>
        /// <param name="availableSize">The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to whatever content is available.</param>
        /// <returns>
        /// The size that this element determines it needs during layout, based on its calculations of child element sizes.
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            EnsureFormattedText();

            // constrain the formatted text according to the available size

            double w = availableSize.Width;
            double h = availableSize.Height;

            // the Math.Min call is important - without this constraint (which seems arbitrary, but is the maximum allowable text width), things blow up when availableSize is infinite in both directions
            // the Math.Max call is to ensure we don't hit zero, which will cause MaxTextHeight to throw
            _FormattedText.MaxTextWidth = Math.Min(3579139, w);
            _FormattedText.MaxTextHeight = Math.Max(0.0001d, h);

            // return the desired size
            return new Size(Math.Ceiling(_FormattedText.Width), Math.Ceiling(_FormattedText.Height));
        }
        /// <summary>
        /// When overridden in a derived class, positions child elements and determines a size for a <see cref="T:System.Windows.FrameworkElement" /> derived class.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
        /// <returns>
        /// The actual size used.
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            EnsureFormattedText();

            // update the formatted text with the final size
            _FormattedText.MaxTextWidth = finalSize.Width;
            _FormattedText.MaxTextHeight = Math.Max(0.0001d, finalSize.Height);

            // need to re-generate the geometry now that the dimensions have changed
            _TextGeometry = null;

            return finalSize;
        }
        /// <summary>
        /// Called when [formatted text invalidated].
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnFormattedTextInvalidated(DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
        {
            var outlinedTextBlock = (OutlinedTextBlock)dependencyObject;
            outlinedTextBlock._FormattedText = null;
            outlinedTextBlock._TextGeometry = null;

            outlinedTextBlock.InvalidateMeasure();
            outlinedTextBlock.InvalidateVisual();
        }
        /// <summary>
        /// Called when [formatted text updated].
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnFormattedTextUpdated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var outlinedTextBlock = (OutlinedTextBlock)dependencyObject;
            outlinedTextBlock.UpdateFormattedText();
            outlinedTextBlock._TextGeometry = null;

            outlinedTextBlock.InvalidateMeasure();
            outlinedTextBlock.InvalidateVisual();
        }
        /// <summary>
        /// Ensures the formatted text.
        /// </summary>
        private void EnsureFormattedText()
        {
            if (_FormattedText != null)
            {
                return;
            }

            _FormattedText = new FormattedText(
              Text ?? "",
              CultureInfo.CurrentUICulture,
              FlowDirection,
              new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
              FontSize,
              Brushes.Black);

            UpdateFormattedText();
        }
        /// <summary>
        /// Updates the formatted text.
        /// </summary>
        private void UpdateFormattedText()
        {
            if (_FormattedText == null)
            {
                return;
            }

            _FormattedText.MaxLineCount = TextWrapping == TextWrapping.NoWrap ? 1 : int.MaxValue;
            _FormattedText.TextAlignment = TextAlignment;
            _FormattedText.Trimming = TextTrimming;

            _FormattedText.SetFontSize(FontSize);
            _FormattedText.SetFontStyle(FontStyle);
            _FormattedText.SetFontWeight(FontWeight);
            _FormattedText.SetFontFamily(FontFamily);
            _FormattedText.SetFontStretch(FontStretch);
            _FormattedText.SetTextDecorations(TextDecorations);
        }
        /// <summary>
        /// Ensures the geometry.
        /// </summary>
        private void EnsureGeometry()
        {
            if (_TextGeometry != null)
            {
                return;
            }

            EnsureFormattedText();
            _TextGeometry = _FormattedText.BuildGeometry(new Point(0, 0));
        }
    }
}