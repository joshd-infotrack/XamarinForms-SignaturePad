using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using Acr.XamForms.SignaturePad;
using Acr.XamForms.SignaturePad.iOS;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using NativeView = SignaturePad.SignaturePadView;

[assembly: ExportRenderer(typeof(SignaturePadView), typeof(SignaturePadRenderer))]


namespace Acr.XamForms.SignaturePad.iOS {
    
    public class SignaturePadRenderer : ViewRenderer<SignaturePadView, NativeView> {

        protected override void OnElementChanged(ElementChangedEventArgs<SignaturePadView> e) {
            base.OnElementChanged(e);

            if (e.OldElement != null || this.Element == null)
                return;

            var view = new NativeView();
            var el = e.NewElement;
            
            if (el.BackgroundColor != Color.Default)
                view.BackgroundColor = el.BackgroundColor.ToUIColor();

            if (!String.IsNullOrWhiteSpace(el.CaptionText))
                view.Caption.Text = el.CaptionText;

            if (el.CaptionTextColor != Color.Default)
                view.Caption.TextColor = el.CaptionTextColor.ToUIColor();

            if (!String.IsNullOrWhiteSpace(el.ClearText))
                view.ClearLabel.SetTitle(el.ClearText, UIControlState.Normal);

            if (el.ClearTextColor != Color.Default)
                view.ClearLabel.SetTitleColor(el.ClearTextColor.ToUIColor(), UIControlState.Normal);

            if (!String.IsNullOrWhiteSpace(el.PromptText))
                view.SignaturePrompt.Text = el.PromptText;

            if (el.PromptTextColor != Color.Default)
                view.SignaturePrompt.TextColor = el.PromptTextColor.ToUIColor();

            if (el.SignatureLineColor != Color.Default)
                view.SignatureLineColor = el.SignatureLineColor.ToUIColor();

            if (el.StrokeColor != Color.Default)
                view.StrokeColor = el.StrokeColor.ToUIColor();

            if (el.StrokeWidth > 0)
                view.StrokeWidth = el.StrokeWidth;


            this.Element.SetInternals(
                this.GetImageStream,
                () => view.Points.Select(x => new DrawPoint((float)x.X, (float)x.Y)),
                x => view.LoadPoints(x.Select(y => new CGPoint(y.X, y.Y)).ToArray()),
                () => view.IsBlank,
                () => {
                    view.Clear();
                    return true;
                }
            );

            //  ORIGINAL IMPLEMENTATION

            //this.Element.SetInternals(
            //    imgFormat => imgFormat == ImageFormatType.Jpg
            //            ? view.GetImage().AsJPEG().AsStream()
            //            : view.GetImage().AsPNG().AsStream(),
            //    () => view.Points.Select(x => new DrawPoint((float)x.X, (float)x.Y)), 
            //    x => view.LoadPoints(x.Select(y => new CGPoint(y.X, y.Y)).ToArray()),
            //    () => view.IsBlank
            //);

            this.SetNativeControl(view);
        }

        private byte[] GetImageStream(ImageFormatType imgFormat)
        {
            var image = this.Control.GetImage();

            using (NSData imageData = imgFormat == ImageFormatType.Jpg ? 
                                        image.AsJPEG() : image.AsPNG())
            {
                Byte[] myByteArray = new Byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, 
                    Convert.ToInt32(imageData.Length));

                return myByteArray;
            }
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
            base.OnElementPropertyChanged(sender, e);
            
            if (this.Element == null || this.Control == null)
                return;

            var el = this.Element;
            if (e.PropertyName == SignaturePadView.BackgroundColorProperty.PropertyName)
                this.Control.BackgroundColor = el.BackgroundColor.ToUIColor();

            else if (e.PropertyName == SignaturePadView.CaptionTextProperty.PropertyName)
                this.Control.Caption.Text = el.CaptionText;

            else if (e.PropertyName == SignaturePadView.CaptionTextColorProperty.PropertyName)
                this.Control.Caption.TextColor = el.CaptionTextColor.ToUIColor();

            else if (e.PropertyName == SignaturePadView.ClearTextProperty.PropertyName)
                this.Control.ClearLabel.SetTitle(el.ClearText, UIControlState.Normal);

            else if (e.PropertyName == SignaturePadView.ClearTextColorProperty.PropertyName)
                this.Control.ClearLabel.SetTitleColor(el.ClearTextColor.ToUIColor(), UIControlState.Normal);

            else if (e.PropertyName == SignaturePadView.PromptTextProperty.PropertyName)
                this.Control.SignaturePrompt.Text = el.PromptText;

            else if (e.PropertyName == SignaturePadView.PromptTextColorProperty.PropertyName)
                this.Control.SignaturePrompt.TextColor = el.PromptTextColor.ToUIColor();

            else if (e.PropertyName == SignaturePadView.SignatureLineColorProperty.PropertyName)
                this.Control.SignatureLineColor = el.SignatureLineColor.ToUIColor();

            else if (e.PropertyName == SignaturePadView.StrokeColorProperty.PropertyName)
                this.Control.StrokeColor = el.StrokeColor.ToUIColor();

            else if (e.PropertyName == SignaturePadView.StrokeWidthProperty.PropertyName)
                this.Control.StrokeWidth = el.StrokeWidth;
        }
    }
}