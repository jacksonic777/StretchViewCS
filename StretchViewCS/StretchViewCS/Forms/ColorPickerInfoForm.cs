using System;
using System.Drawing;
using System.Windows.Forms;
using StretchViewCS.Utils;

namespace StretchViewCS.Forms
{
    /// <summary>
    /// カラーピッカー利用中に、カーソル位置の色情報を表示する専用フォーム。
    /// </summary>
    public class ColorPickerInfoForm : Form
    {
        private const int FormWidth = 240;
        private const int FormHeight = 138;
        private const int MarginFromCursor = 18;
        private const int ScreenMargin = 8;

        private readonly Panel _previewPanel;
        private readonly Label _titleLabel;
        private readonly Label _positionLabel;
        private readonly Label _hexLabel;
        private readonly Label _rgbLabel;
        private readonly Label _instructionLabel;

        public ColorPickerInfoForm()
        {
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;
            MinimizeBox = false;
            MaximizeBox = false;
            ControlBox = false;
            Size = new Size(FormWidth, FormHeight);
            Text = LocalizationManager.Text("ColorPicker.InfoTitle");

            _titleLabel = new Label
            {
                AutoSize = false,
                Location = new Point(12, 8),
                Size = new Size(210, 20),
                Font = new Font(Font, FontStyle.Bold),
                Text = LocalizationManager.Text("ColorPicker.InfoTitle")
            };

            _previewPanel = new Panel
            {
                Location = new Point(14, 36),
                Size = new Size(48, 48),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.Black
            };

            _positionLabel = CreateValueLabel(72, 34);
            _hexLabel = CreateValueLabel(72, 56);
            _rgbLabel = CreateValueLabel(72, 78);

            _instructionLabel = new Label
            {
                AutoSize = false,
                Location = new Point(12, 104),
                Size = new Size(212, 20),
                Text = LocalizationManager.Text("ColorPicker.InfoInstruction")
            };

            Controls.Add(_titleLabel);
            Controls.Add(_previewPanel);
            Controls.Add(_positionLabel);
            Controls.Add(_hexLabel);
            Controls.Add(_rgbLabel);
            Controls.Add(_instructionLabel);
        }

        public void UpdateInfo(Point screenPoint, Color color, Rectangle pickerBounds)
        {
            _previewPanel.BackColor = color;
            _positionLabel.Text = $"{LocalizationManager.Text("ColorPicker.PositionLabel")}: X={screenPoint.X}, Y={screenPoint.Y}";
            _hexLabel.Text = $"{LocalizationManager.Text("ColorPicker.HexLabel")}: #{color.R:X2}{color.G:X2}{color.B:X2}";
            _rgbLabel.Text = $"{LocalizationManager.Text("ColorPicker.RgbLabel")}: R={color.R} G={color.G} B={color.B}";
            Location = CalculateLocation(screenPoint, pickerBounds);
        }

        private static Label CreateValueLabel(int x, int y)
        {
            return new Label
            {
                AutoSize = false,
                Location = new Point(x, y),
                Size = new Size(150, 20),
                Font = new Font("Consolas", 9f)
            };
        }

        private static Point CalculateLocation(Point cursorPoint, Rectangle pickerBounds)
        {
            int x = cursorPoint.X + MarginFromCursor;
            int y = cursorPoint.Y + MarginFromCursor;

            if (x + FormWidth > pickerBounds.Right)
            {
                x = cursorPoint.X - FormWidth - MarginFromCursor;
            }

            if (y + FormHeight > pickerBounds.Bottom)
            {
                y = cursorPoint.Y - FormHeight - MarginFromCursor;
            }

            if (x < pickerBounds.Left)
            {
                x = pickerBounds.Left + ScreenMargin;
            }

            if (y < pickerBounds.Top)
            {
                y = pickerBounds.Top + ScreenMargin;
            }

            return new Point(x, y);
        }
    }
}
