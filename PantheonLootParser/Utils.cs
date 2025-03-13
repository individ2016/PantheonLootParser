using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantheonLootParser
{
	internal class Utils
	{
		public ChatWindowSettings? ChatWindowSettings { get; set; }
		private Boolean _isDisposing = false;

		private Rectangle _chatWindowRect;
		public Rectangle ChatWindowRect
		{
			get {
				if(_chatWindowRect.IsEmpty)
					_chatWindowRect = this.FindChatWindowRect();
				return _chatWindowRect;
			}
		}

		public Rectangle FindChatWindowRect()
		{
			Rectangle rect = new Rectangle();
			rect.Width = (int)(ChatWindowSettings.W * ChatWindowSettings.CustomScale); 
			rect.Height = (int)(ChatWindowSettings.H * ChatWindowSettings.CustomScale);

			Point center = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);

			rect.X = center.X + ChatWindowSettings.X;
			rect.Y = center.Y - ChatWindowSettings.Y;
			return rect;
		}

		public Mat Source { get; private set; }

		private Mat _hsvSource = new Mat();

		public Mat HsvSource
		{
			get {
				if(!_isDisposing && _hsvSource.IsEmpty && this.ChatWindowRect != null)
					CvInvoke.CvtColor(Source, _hsvSource, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

				return _hsvSource;
			}
		}

		private static Emgu.CV.OCR.Tesseract? _tesseractEngine;
		private static Emgu.CV.OCR.Tesseract OCREngine
		{
			get {
				if(_tesseractEngine == null)
				{
					_tesseractEngine = new Emgu.CV.OCR.Tesseract("tessdata", "eng", Emgu.CV.OCR.OcrEngineMode.Default);
					_tesseractEngine.SetVariable("tessedit_char_whitelist", "[:'. ]ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
				}
				return _tesseractEngine;
			}
		}

		public String RecognizeStrings(Mat mat)
		{
			OCREngine.SetImage(mat);
			OCREngine.Recognize();
			return OCREngine.GetUTF8Text();
		}

		public static nint GetPantheonHwnd()
		{
			return Win32.FindWindow("UnityWndClass", "Pantheon");
		}

		public Mat? GetPantheonScreen()
		{
			nint hwnd = GetPantheonHwnd();
			if(hwnd != IntPtr.Zero)
			{
				using(Bitmap src = this.CopyScreen())
				{
					Mat result = src.ToMat();
					CvInvoke.CvtColor(result, result, ColorConversion.Bgra2Bgr);
					return result;
				}
			}
			return null;
		}

		public Bitmap CopyScreen()
		{
			var screenBmp = new Bitmap(ChatWindowRect.Width, ChatWindowRect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			using(var bmpGraphics = Graphics.FromImage(screenBmp))
			{
				bmpGraphics.CopyFromScreen(ChatWindowRect.Left, ChatWindowRect.Top, 0, 0, new System.Drawing.Size(ChatWindowRect.Width, ChatWindowRect.Height));
				return screenBmp;
			}
		}
	}
}
