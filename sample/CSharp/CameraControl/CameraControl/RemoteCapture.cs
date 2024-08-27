/******************************************************************************
*                                                                             *
*   PROJECT : Eos Digital camera Software Development Kit EDSDK               *
*                                                                             *
*   Description: This is the Sample code to show the usage of EDSDK.          *
*                                                                             *
*                                                                             *
*******************************************************************************
*                                                                             *
*   Written and developed by Canon Inc.                                       *
*   Copyright Canon Inc. 2018 All Rights Reserved                             *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using Python.Runtime;
using System.Xml;
using Newtonsoft.Json;


namespace CameraControl
{
    public partial class RemoteCapture : Form
    {

        private CameraController _controller = null;

        private ActionSource _actionSource = null;

        private List<IObserver> _observerList = new List<IObserver>();

        Rectangle _clip;

        // Updated by Jeff 08/07/2024

        private Double x_position = 0.0
            , y_position = 0.0;
        private Double x_1, y_1;
        private Double x_2, y_2;
        double xStepLength, yStepLength = 0.0;
        private Boolean isPaused = false;
        private Boolean isStopped = false;


        public RemoteCapture(ref CameraController controller, ref ActionSource actionSource)
        {
            InitializeComponent();

            _controller = controller;

            _actionSource = actionSource;

            CameraEvent e;

            _observerList.Add((IObserver)aeMode1);
            _observerList.Add((IObserver)av1);
            _observerList.Add((IObserver)evfPictureBox1);
            _observerList.Add((IObserver)tv1);
            _observerList.Add((IObserver)iso1);
            _observerList.Add((IObserver)meteringMode1);
            _observerList.Add((IObserver)exposureComp1);
            _observerList.Add((IObserver)imageQuality1);
            _observerList.Add((IObserver)evfAFMode1);
            _observerList.Add((IObserver)driveMode1);
            _observerList.Add((IObserver)whiteBalance1);
            _observerList.Add((IObserver)availableShotLabel1);
            _observerList.Add((IObserver)batteryLebelLabel1);
            _observerList.Add((IObserver)zoom1);
            _observerList.Add((IObserver)afMode1);
            _observerList.Add((IObserver)flashMode1);
            _observerList.Add((IObserver)downloadProgressBar1);
            _observerList.Add((IObserver)tempStatusLabel1);
            _observerList.Add((IObserver)movieQuality1);
            _observerList.Add((IObserver)pictureStyle1);
            _observerList.Add((IObserver)aspect1);
            _observerList.Add((IObserver)movieHFR1);

            System.Threading.Thread.Sleep(1000);

            _observerList.ForEach(observer => _controller.GetModel().Add(ref observer));

            aeMode1.SetActionSource(ref _actionSource);
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_AEModeSelect);
            _controller.GetModel().NotifyObservers(e);

            av1.SetActionSource(ref _actionSource);
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_Av);
            _controller.GetModel().NotifyObservers(e);

            evfPictureBox1.SetActionSource(ref _actionSource);

            tv1.SetActionSource(ref _actionSource);
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_Tv);
            _controller.GetModel().NotifyObservers(e);


            iso1.SetActionSource(ref _actionSource);
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_ISOSpeed);
            _controller.GetModel().NotifyObservers(e);


            meteringMode1.SetActionSource(ref _actionSource);
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_MeteringMode);
            _controller.GetModel().NotifyObservers(e);


            exposureComp1.SetActionSource(ref _actionSource);
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_ExposureCompensation);
            _controller.GetModel().NotifyObservers(e);


            imageQuality1.SetActionSource(ref _actionSource);
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_ImageQuality);
            _controller.GetModel().NotifyObservers(e);


            evfAFMode1.SetActionSource(ref _actionSource);
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_Evf_AFMode);
            _controller.GetModel().NotifyObservers(e);


            driveMode1.SetActionSource(ref _actionSource);
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_DriveMode);
            _controller.GetModel().NotifyObservers(e);


            whiteBalance1.SetActionSource(ref _actionSource);
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_WhiteBalance);
            _controller.GetModel().NotifyObservers(e);

            e = new CameraEvent(CameraEvent.Type.PROPERTY_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_AFMode);
            _controller.GetModel().NotifyObservers(e);

            flashMode1.SetActionSource(ref _actionSource);
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_DC_Strobe);
            _controller.GetModel().NotifyObservers(e);

            movieQuality1.SetActionSource(ref _actionSource);
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_MovieParam);
            _controller.GetModel().NotifyObservers(e);

            movieHFR1.SetActionSource(ref _actionSource);
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_MovieHFRSetting);
            _controller.GetModel().NotifyObservers(e);

            pictureStyle1.SetActionSource(ref _actionSource);
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_PictureStyle);
            _controller.GetModel().NotifyObservers(e);

            aspect1.SetActionSource(ref _actionSource);
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_Aspect);
            _controller.GetModel().NotifyObservers(e);

            zoom1.SetActionSource(ref _actionSource);
            label16.Text = zoom1.Value.ToString();
            e = new CameraEvent(CameraEvent.Type.PROPERTY_DESC_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_DC_Zoom);
            _controller.GetModel().NotifyObservers(e);

            actionButton1.SetActionSource(ref _actionSource);
            actionButton1.Command = ActionEvent.Command.TAKE_PICTURE;

            actionButton3.SetActionSource(ref _actionSource);
            actionButton3.Command = ActionEvent.Command.PRESS_COMPLETELY;

            actionButton2.SetActionSource(ref _actionSource);
            actionButton2.Command = ActionEvent.Command.PRESS_HALFWAY;

            actionButton4.SetActionSource(ref _actionSource);
            actionButton4.Command = ActionEvent.Command.PRESS_OFF;

            actionButton5.SetActionSource(ref _actionSource);
            actionButton5.Command = ActionEvent.Command.START_EVF;

            actionButton6.SetActionSource(ref _actionSource);
            actionButton6.Command = ActionEvent.Command.END_EVF;

            actionButton10.SetActionSource(ref _actionSource);
            actionButton10.Command = ActionEvent.Command.FOCUS_NEAR3;

            actionButton11.SetActionSource(ref _actionSource);
            actionButton11.Command = ActionEvent.Command.FOCUS_NEAR2;

            actionButton12.SetActionSource(ref _actionSource);
            actionButton12.Command = ActionEvent.Command.FOCUS_NEAR1;

            actionButton13.SetActionSource(ref _actionSource);
            actionButton13.Command = ActionEvent.Command.FOCUS_FAR1;

            actionButton14.SetActionSource(ref _actionSource);
            actionButton14.Command = ActionEvent.Command.FOCUS_FAR2;

            actionButton15.SetActionSource(ref _actionSource);
            actionButton15.Command = ActionEvent.Command.FOCUS_FAR3;

            actionButton7.SetActionSource(ref _actionSource);
            actionButton7.Command = ActionEvent.Command.EVF_AF_ON;

            actionButton8.SetActionSource(ref _actionSource);
            actionButton8.Command = ActionEvent.Command.EVF_AF_OFF;

            actionButton9.SetActionSource(ref _actionSource);
            actionButton9.Command = ActionEvent.Command.ZOOM_FIT;

            actionButton16.SetActionSource(ref _actionSource);
            actionButton16.Command = ActionEvent.Command.ZOOM_ZOOM;

            actionButton17.SetActionSource(ref _actionSource);
            actionButton17.Command = ActionEvent.Command.POSITION_UP;

            actionButton18.SetActionSource(ref _actionSource);
            actionButton18.Command = ActionEvent.Command.POSITION_LEFT;

            actionButton19.SetActionSource(ref _actionSource);
            actionButton19.Command = ActionEvent.Command.POSITION_RIGHT;

            actionButton20.SetActionSource(ref _actionSource);
            actionButton20.Command = ActionEvent.Command.POSITION_DOWN;

            actionButton21.SetActionSource(ref _actionSource);
            actionButton21.Command = ActionEvent.Command.REC_START;

            actionButton22.SetActionSource(ref _actionSource);
            actionButton22.Command = ActionEvent.Command.REC_END;

            actionButton23.SetActionSource(ref _actionSource);
            actionButton23.Command = ActionEvent.Command.ROLLPITCH;
            actionButton23.Text = "Roll Pitch On";

            actionButton24.SetActionSource(ref _actionSource);
            actionButton24.Command = ActionEvent.Command.CLICK_WB;

            actionButton25.SetActionSource(ref _actionSource);
            actionButton25.Command = ActionEvent.Command.CLICK_AF_POINT;

            actionButton26.SetActionSource(ref _actionSource);
            actionButton26.Command = ActionEvent.Command.CLICK_FBS;

            actionRadioButton1.SetActionSource(ref _actionSource);
            actionRadioButton1.Command = ActionEvent.Command.PRESS_STILL;
            updateFixedMovie(_controller.GetModel().FixedMovie);

            actionRadioButton2.SetActionSource(ref _actionSource);
            actionRadioButton2.Command = ActionEvent.Command.PRESS_MOVIE;
            updateFixedMovie(_controller.GetModel().FixedMovie);

            actionRadioButton3.SetActionSource(ref _actionSource);
            actionRadioButton3.Command = ActionEvent.Command.MIRRORUP_ON;

            actionRadioButton4.SetActionSource(ref _actionSource);
            actionRadioButton4.Command = ActionEvent.Command.MIRRORUP_OFF;

            // Updated by Jeff 08/07/2024
            xPosition.Text = xPositionTrackBar.Value.ToString();
            yPosition.Text = yPositionTrackBar.Value.ToString();
            startingPositionButton.SetActionSource(ref _actionSource);
            endingPositionButton.SetActionSource(ref _actionSource);
            endingPositionButton.Enabled = false;
            autoScanningButton.SetActionSource(ref _actionSource);
            autoScanningButton.Enabled = false;
            pulseScanningButton.SetActionSource(ref _actionSource);
            pulseScanningButton.Enabled = false;
            stopScanningButton.SetActionSource(ref _actionSource);
            stopScanningButton.Enabled = false;
            SelectFolder.SetActionSource(ref _actionSource);




            // Check Mirror Up Setting.
            if (0xffffffff == _controller.GetModel().MirrorUpSetting || (_controller.GetModel().StartupEvfOutputDevice & EDSDKLib.EDSDK.EvfOutputDevice_TFT) != 0)
            {
                actionRadioButton3.Enabled = false;
                actionRadioButton4.Enabled = false;
            }
            updateMirrorLockUpState(_controller.GetModel().MirrorLockUpState);

            controlFocusButton((int)EDSDKLib.EDSDK.EdsEvfAFMode.Evf_AFMode_LiveFace != _controller.GetModel().EvfAFMode);

            // invalidate it in the DC
            label26.Enabled = _controller.GetModel().isTypeDS;
            actionButton9.Enabled = _controller.GetModel().isTypeDS;
            actionButton16.Enabled = _controller.GetModel().isTypeDS;
            label19.Enabled = _controller.GetModel().isTypeDS;
            // Processing inside updateFixedMovie
            //actionButton10.Enabled = _controller.GetModel().isTypeDS;
            //actionButton11.Enabled = _controller.GetModel().isTypeDS;
            //actionButton12.Enabled = _controller.GetModel().isTypeDS;
            //actionButton13.Enabled = _controller.GetModel().isTypeDS;
            //actionButton14.Enabled = _controller.GetModel().isTypeDS;
            //actionButton15.Enabled = _controller.GetModel().isTypeDS;

            // invalidate it in the DS
            label15.Enabled = !_controller.GetModel().isTypeDS;
            zoom1.Enabled = false; // At the time of start, ZoomBar is off
            label16.Enabled = !_controller.GetModel().isTypeDS;

            e = new CameraEvent(CameraEvent.Type.PROPERTY_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_Evf_AFMode);
            _controller.GetModel().NotifyObservers(e);

            e = new CameraEvent(CameraEvent.Type.PROPERTY_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_AvailableShots);
            _controller.GetModel().NotifyObservers(e);

            e = new CameraEvent(CameraEvent.Type.PROPERTY_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_BatteryLevel);
            _controller.GetModel().NotifyObservers(e);

            e = new CameraEvent(CameraEvent.Type.PROPERTY_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_TempStatus);
            _controller.GetModel().NotifyObservers(e);

            e = new CameraEvent(CameraEvent.Type.PROPERTY_CHANGED, (IntPtr)EDSDKLib.EDSDK.PropID_FixedMovie);
            _controller.GetModel().NotifyObservers(e);

            if (!_controller.GetModel().isTypeDS)
            {
                _actionSource.FireEvent(ActionEvent.Command.REMOTESHOOTING_START, IntPtr.Zero);
            }

            updateAngleInfoLabel("-", "-", "-");

            //Updated by Jeff 08/12/2024
            // Start the exe file

            // startTango();
            MessageBox.Show("Please Connect Manually" +
                "\nMake Sure EOS Utility Is NOT On" , "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // await Task.Delay(500);

            EnableTools(false);

            try
            {
                string[] PortNames = System.IO.Ports.SerialPort.GetPortNames();
                for (int ii = 0; ii < 20; ii++) comboBox1.Items.Add(PortNames[ii]);
            }
            catch
            {
            }

            //Updated by Jeff 08/22/2024
            // 添加选项到ComboBox
            checkBox2.Checked = true;
            SelectFolder2.Enabled = false;

            comboBox2.Items.Add("Blue");
            comboBox2.Items.Add("Red");
            comboBox2.Items.Add("Green");
            comboBox2.Items.Add("Gray");

            // 设置默认选项
            comboBox2.SelectedIndex = 0;  // 默认选择第一个选项

            // 设置NumericUpDown控件的属性
            numericUpDown1.Minimum = 0;
            numericUpDown1.Maximum = 500;
            numericUpDown1.Increment = 1;
            numericUpDown1.DecimalPlaces = 0;
            numericUpDown1.Value =100;

            numericUpDown2.Minimum = 0;
            numericUpDown2.Maximum = 500;
            numericUpDown2.Increment = 1;
            numericUpDown2.DecimalPlaces = 0;
            numericUpDown2.Value =170;

            numericUpDown3.Minimum = 0;
            numericUpDown3.Maximum = 10;
            numericUpDown3.Increment = 1;
            numericUpDown3.DecimalPlaces = 0;
            numericUpDown3.Value = 5;

        }

        private void RemoteCapture_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.DoEvents();
            _actionSource.FireEvent(ActionEvent.Command.END_ROLLPITCH, IntPtr.Zero);
            _actionSource.FireEvent(ActionEvent.Command.END_EVF, IntPtr.Zero);
            if (!_controller.GetModel().isTypeDS)
            {
                _actionSource.FireEvent(ActionEvent.Command.REMOTESHOOTING_STOP, IntPtr.Zero);
            }
            _actionSource.FireEvent(ActionEvent.Command.PRESS_OFF, IntPtr.Zero);
            _actionSource.FireEvent(ActionEvent.Command.EVF_AF_OFF, IntPtr.Zero);
            _observerList.ForEach(observer => _controller.GetModel().Remove(ref observer));

            // Updated by Jeff 08/25/2024
            PythonEngine.Shutdown(); // 关闭Python引擎
            closeTango();
        }

        private void zoom1_ValueChanged(object sender, EventArgs e)
        {
            label16.Text = zoom1.Value.ToString();
        }

        private void actionButton5_Click(object sender, EventArgs e)
        {
            if (!_controller.GetModel().isTypeDS)
            {
                zoom1.Enabled = true;
            }
        }

        private void actionButton6_Click(object sender, EventArgs e)
        {
            zoom1.Enabled = false;
        }

        public void controlFocusButton(bool onoff)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => controlFocusButton(onoff)), null);
            }
            else
            {
                actionButton17.Enabled = onoff;
                actionButton18.Enabled = onoff;
                actionButton19.Enabled = onoff;
                actionButton20.Enabled = onoff;
            }
        }

        public void updateAngleInfoLabel(string pos, string roll, string pitc)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => updateAngleInfoLabel(pos, roll, pitc)), null);
            }
            else
            {
                label22.Text = pos;
                label23.Text = roll;
                label25.Text = pitc;

                if (_controller.GetModel().RollPitch == 0)
                {
                    actionButton23.Text = "Roll Pitch Off";
                }
                else
                {
                    actionButton23.Text = "Roll Pitch On";
                }
            }
        }

        public void changeMouseCursor(bool onoff)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => changeMouseCursor(onoff)), null);
            }
            else
            {
                if (onoff)
                {
                    Cursor = Cursors.Cross;
                    _clip = Cursor.Clip;
                    Rectangle limitRect = evfPictureBox1.Bounds;
                    limitRect.X = limitRect.X + this.DesktopLocation.X + 10;
                    limitRect.Y = limitRect.Y + this.DesktopLocation.Y + 30;
                    Cursor.Clip = limitRect;
                }
                else
                {
                    Cursor = Cursors.Default;
                    Cursor.Clip = _clip;
                }
            }
        }

        private void evfPictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (Cursor == Cursors.Cross)
            {
                // JPEG L size
                int JpegLWidth = 6720;
                int JpegLHeight = 4480;
                if (_controller.GetModel().SizeJpegLarge.width != 0 && _controller.GetModel().SizeJpegLarge.height != 0)
                {
                    JpegLWidth = _controller.GetModel().SizeJpegLarge.width;
                    JpegLHeight = _controller.GetModel().SizeJpegLarge.height;
                }

                EDSDKLib.EDSDK.EdsPoint clickPoint;

                clickPoint.x = (int)((double)JpegLWidth / (double)evfPictureBox1.Width * (double)e.X);
                clickPoint.y = (int)((double)JpegLHeight / (double)evfPictureBox1.Height * (double)e.Y);
                if (clickPoint.x > JpegLWidth - 1)
                {
                    clickPoint.x = JpegLWidth - 1;
                }
                else if (clickPoint.x < 0)
                {
                    clickPoint.x = 0;
                }
                if (clickPoint.y > JpegLHeight - 1)
                {
                    clickPoint.y = JpegLHeight - 1;
                }
                else if (clickPoint.y < 0)
                {
                    clickPoint.y = 0;
                }

                _controller.GetModel().ClickPoint = clickPoint.x << 16 | clickPoint.y;
                _controller.GetModel().NotifyObservers(new CameraEvent(CameraEvent.Type.MOUSE_CURSOR, (IntPtr)0));
            }
        }

        public void updateFixedMovie(uint data)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => updateFixedMovie(data)), null);
            }
            else
            {
                if (data == 0)
                {
                    actionRadioButton1.Checked = true;

                    // Rec Button
                    actionButton21.Enabled = false;
                    actionButton22.Enabled = false;

                    // MovieQuality
                    movieQuality1.Enabled = false;

                    if (_controller.GetModel().isTypeDS)
                    {
                        actionButton10.Enabled = true;
                        actionButton11.Enabled = true;
                        actionButton12.Enabled = true;
                        actionButton13.Enabled = true;
                        actionButton14.Enabled = true;
                        actionButton15.Enabled = true;
                    }

                    // Clear EVF
                    this.evfPictureBox1.Image = null;
                    _actionSource.FireEvent(ActionEvent.Command.END_EVF, IntPtr.Zero);
                }
                else
                {
                    actionRadioButton2.Checked = true;

                    // Rec Button
                    actionButton21.Enabled = true;
                    actionButton22.Enabled = true;

                    // MovieQuality
                    movieQuality1.Enabled = true;

                    actionButton10.Enabled = false;
                    actionButton11.Enabled = false;
                    actionButton12.Enabled = false;
                    actionButton13.Enabled = false;
                    actionButton14.Enabled = false;
                    actionButton15.Enabled = false;
                }
            }
        }

        public void updateMirrorLockUpState(uint data)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => updateMirrorLockUpState(data)), null);
            }
            else
            {
                if (data != (uint)EDSDKLib.EDSDK.EdsMirrorLockupState.Disable)
                {
                    // Enable = 1, DuringShooting = 2
                    actionRadioButton3.Checked = true;
                }
                else
                {
                    // Disable = 0
                    actionRadioButton4.Checked = true;
                }
            }
        }

        // Updated by Jeff 08/07/2024
        private void zoom1_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void actionButton29_Click(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void xPositionTrackBarSlide(object sender, EventArgs e)
        {
            x_position = xPositionTrackBar.Value;
            xPosition.Text = x_position.ToString();
        }

        private void label35_Click_1(object sender, EventArgs e)
        {

        }

        private void yPositionTrackBarSlide(object sender, EventArgs e)
        {
            y_position = yPositionTrackBar.Value;
            yPosition.Text = y_position.ToString();
        }

        private async void StartingPositionButton_Click(object sender, EventArgs e)
        {
            Double xx, yy, zz, aa;
            try
            {
                LSX_GetPos(lLSID, out xx, out yy, out zz, out aa);
                x_1 = xx;
                y_1 = yy;

            }
            catch
            {

            }

            // Enable the ending position button only after setting the home position
            endingPositionButton.Enabled = true;

            Console.WriteLine($"The starting position is set to: ({x_1}, {y_1})");
        }


        private async void EndingPositionButton_Click(object sender, EventArgs e)
        {
            Double xx, yy, zz, aa;
            try
            {
                LSX_GetPos(lLSID, out xx, out yy, out zz, out aa);
                x_2 = xx;
                y_2 = yy;
               
            }
            catch
            {

            }
            
            // Enable the auto scanning button only after obtaining the ending position
            autoScanningButton.Enabled = true;

            Console.WriteLine($"The ending position is set to: ({x_2}, {y_2})");
        }


        private void actionButton3_Click(object sender, EventArgs e)
        {

        }

        private async void AutoScanningButton_Click(object sender, EventArgs e)
        {
            // Set home position
            LSX_MoveAbs(lLSID, x_1, y_1, 0.0, 0.0, 1);

            // Real x step length should be 0.0414 approximately 240um
            // Real y step length should be 0.0281 approximately 160um

            // in this case, we choose x step length to be 0.036
            // in this case, we choose y step length to be 0.024 instead
            //double xStepLength = 0.0360;
            //double yStepLength = 0.0240;

            //MAKE SURE THAT THE TEXT IN textBox1 AND textBox2 ARE DOUBLE!
            xStepLength = double.Parse(textBox1.Text);
            yStepLength = double.Parse(textBox2.Text);

            pulseScanningButton.Enabled = true;
            stopScanningButton.Enabled = true;
            AutoTakePicturesFunc(xStepLength, yStepLength);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public async void AutoTakePicturesFunc(double xStepLength, double yStepLength)
        {
            // 重置停止状态
            isStopped = false;

            // 将句柄转换为 IntPtr
           // IntPtr buttonHandle = new IntPtr(0x001F082A);

            // 发送 BM_CLICK 消息，模拟点击按钮
           // SendMessage(buttonHandle, 0x00F5, IntPtr.Zero, IntPtr.Zero);

            //Console.WriteLine("Button clicked.");

            Double x = x_1;
            Double y = y_1;
            Int32 xSteps = (int)((x_2 - x_1) / xStepLength) + 1;
            Int32 ySteps = (int)((y_2 - y_1) / yStepLength) + 1;
            Int32 count = 1;

            await Task.Run(async () =>
            {
                while (!isStopped && (y_2 - y >= 0 || x_2 - x >= 0))
                {
                    while (isPaused)
                    {
                        await Task.Delay(100); // 短暂的延迟以防止CPU占用过高
                    }
                    // Take the picture
                    _actionSource.FireEvent(ActionEvent.Command.PRESS_HALFWAY, IntPtr.Zero);
                    await Task.Delay(500);

                    _actionSource.FireEvent(ActionEvent.Command.PRESS_COMPLETELY, IntPtr.Zero);
                    await Task.Delay(1000);

                    _actionSource.FireEvent(ActionEvent.Command.PRESS_OFF, IntPtr.Zero);
                    await Task.Delay(500);

                    // Move the plate
                    // SetMeanderParameters((int)x, (int)y);

                    if (y + yStepLength < y_2)
                    {
                        y += yStepLength;
                        // Move along y axis
                        LSX_MoveRelSingleAxis(lLSID, 2, yStepLength, 1);
                        Console.WriteLine("y plused");
                    }
                    else if (x + xStepLength < x_2)
                    {
                        x += xStepLength;
                        y = y_1;
                        // y move back 
                        LSX_MoveAbsSingleAxis(lLSID, 2, y_1, 1);
                        // x move forward
                        LSX_MoveRelSingleAxis(lLSID, 1, xStepLength, 1);

                        Console.WriteLine("x plused");
                    }
                    else
                    {
                        MessageBox.Show("Scanning Finished!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Console.WriteLine("Auto Scanning Finished");
                        closeTango();
                        break;
                    }

                    // sendParametersToTango(x.ToString(), y.ToString());

                    Console.WriteLine($"Now at: {x},{y}");

                    await Task.Delay(500);
                }
                if (isStopped)
                {
                    // MessageBox.Show("Scanning Stopped", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Console.WriteLine("Scanning Stopped.");
                    // closeTango(); // 关闭设备或重置状态
                }
            });
        }

        private int GetPhotoCount()
        {
            double xStepLength = double.Parse(textBox1.Text);
            double yStepLength = double.Parse(textBox2.Text);
            Int32 count = (int)((y_2 - y_1) / (yStepLength) + 1) * (int)((x_2 - x_1) / (xStepLength) + 1);
            return count;
        }

        private void RemoteCapture_Load(object sender, EventArgs e)
        {
            x_1 = x_position;
            x_2 = x_position;
            y_1 = y_position;
            y_2 = y_position;
        }

        private void actionButton26_Click(object sender, EventArgs e)
        {
            var _focusBractingSetting = new FocusBractingSetting(ref _controller);
            _focusBractingSetting.ShowDialog(this);
            _focusBractingSetting.Dispose();

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

        }

        // Added by Jeff 08/08/2024
        private void startTango()
        {
            string exePath = @"C:\Program Files (x86)\SwitchBoard\SwitchBoard.exe";

            try
            {
                // Create new process start info
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = exePath;

                // In case we need to pass parameters to the process
                // startInfo.Arguments = ""; // 例如 "-arg1 -arg2"

                // Start
                Process process = Process.Start(startInfo);

            }
            catch (Exception ex)
            {
                // Catch error such as Files Not Found
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void closeTango()
        {
            string exeName = "WindowsFormsApplication1";

            try
            {
                // Get all processes in progress
                Process[] processes = Process.GetProcessesByName(exeName);

                // Iterate through them
                foreach (Process process in processes)
                {
                    // Kill the process
                    process.Kill();
                    process.WaitForExit(); //Wait till it exits
                    Console.WriteLine($"{exeName} has been closed.");
                }
            }
            catch (Exception ex)
            {
                // Catch errors
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void pulseScanningButton_Click(object sender, EventArgs e)
        {
            isPaused = !isPaused; // 切换暂停状态
            if (isPaused)
            {
                MessageBox.Show("Scanning Paused", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Console.WriteLine("Scanning Paused");
            }
            else
            {
                MessageBox.Show("Scanning Resumed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Console.WriteLine("Scanning Resumed");
            }
        }

        private async void stopScanningButton_Click(object sender, EventArgs e)
        {
            isStopped = true; // 设置停止状态
            isPaused = false; // 重置暂停状态，以防止无法退出暂停状态
            //await Task.Run(() => Home());
            Console.WriteLine("Scanning Stopped");
            MessageBox.Show("Scanning Stopped", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Updated by Jeff 08/23/2024 ######################################################

        //// Import necessary Win32 API functions
        //[DllImport("user32.dll", SetLastError = true)]
        //static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        //[DllImport("user32.dll", SetLastError = true)]
        //static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, StringBuilder lParam);

        //[DllImport("user32.dll", SetLastError = true)]
        //static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);

        //[DllImport("user32.dll", SetLastError = true)]
        //static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        //const uint WM_GETTEXT = 0x000D;
        //const uint WM_GETTEXTLENGTH = 0x000E;
        //const uint WM_SETTEXT = 0x000C;
        //const uint BM_CLICK = 0x00F5;

        //delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        //public double getXPosition()
        //{
        //    IntPtr hWndTextBoxX = new IntPtr(0x00051486);  // Handle from your screenshot

        //    if (hWndTextBoxX == IntPtr.Zero)
        //    {
        //        throw new Exception("Text box handle is invalid.");
        //    }

        //    int length = (int)SendMessage(hWndTextBoxX, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);

        //    if (length > 0)
        //    {
        //        StringBuilder sb = new StringBuilder(length + 1); // +1 for the null terminator
        //        SendMessage(hWndTextBoxX, WM_GETTEXT, (IntPtr)sb.Capacity, sb);

        //        if (double.TryParse(sb.ToString(), out double result))
        //        {
        //            return result;
        //        }
        //        else
        //        {
        //            throw new Exception("Failed to parse X Position to a double.");
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("X Position text box is empty.");
        //    }
        //}

        //public double getYPosition()
        //{
        //    IntPtr hWndTextBoxY = new IntPtr(0x00051488);  // Replace with your actual handle if different

        //    if (hWndTextBoxY == IntPtr.Zero)
        //    {
        //        throw new Exception("Text box handle is invalid.");
        //    }

        //    int length = (int)SendMessage(hWndTextBoxY, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);

        //    if (length > 0)
        //    {
        //        StringBuilder sb = new StringBuilder(length + 1); // +1 for the null terminator
        //        SendMessage(hWndTextBoxY, WM_GETTEXT, (IntPtr)sb.Capacity, sb);

        //        if (double.TryParse(sb.ToString(), out double result))
        //        {
        //            return result;
        //        }
        //        else
        //        {
        //            throw new Exception("Failed to parse Y Position to a double.");
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("Y Position text box is empty.");
        //    }
        //}

        //public void checkUnidirectional()
        //{
        //    IntPtr hWndButton = new IntPtr(0x001076C);  // The handle for the "Unidirectional" radio button

        //    if (hWndButton == IntPtr.Zero)
        //    {
        //        throw new Exception("Radio button handle is invalid.");
        //    }

        //    SendMessage(hWndButton, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        //    Console.WriteLine("Unidirectional option checked.");
        //}

        //public void setXStepLength(double stepLengthX)
        //{
        //    IntPtr hWndContainer = new IntPtr(0x001076E);  // The handle of the container (whole box)
        //    IntPtr hWndTextBox = IntPtr.Zero;

        //    EnumChildWindows(hWndContainer, (hWnd, lParam) =>
        //    {
        //        StringBuilder windowText = new StringBuilder(256);
        //        GetWindowText(hWnd, windowText, windowText.Capacity);

        //        if (windowText.ToString().Contains("Step length X"))
        //        {
        //            hWndTextBox = hWnd;
        //            return false;  // Stop enumeration
        //        }

        //        return true;  // Continue enumeration
        //    }, IntPtr.Zero);

        //    if (hWndTextBox != IntPtr.Zero)
        //    {
        //        SendMessage(hWndTextBox, WM_SETTEXT, IntPtr.Zero, stepLengthX.ToString("F4"));  // Formatting to 4 decimal places
        //        Console.WriteLine("Step length X set to: " + stepLengthX);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Step length X text box not found.");
        //    }
        //}

        //public void setYStepLength(double stepLengthY)
        //{
        //    IntPtr hWndContainer = new IntPtr(0x001076E);  // The handle of the container (whole box)
        //    IntPtr hWndTextBox = IntPtr.Zero;

        //    EnumChildWindows(hWndContainer, (hWnd, lParam) =>
        //    {
        //        StringBuilder windowText = new StringBuilder(256);
        //        GetWindowText(hWnd, windowText, windowText.Capacity);

        //        if (windowText.ToString().Contains("Step length Y"))
        //        {
        //            hWndTextBox = hWnd;
        //            return false;  // Stop enumeration
        //        }

        //        return true;  // Continue enumeration
        //    }, IntPtr.Zero);

        //    if (hWndTextBox != IntPtr.Zero)
        //    {
        //        SendMessage(hWndTextBox, WM_SETTEXT, IntPtr.Zero, stepLengthY.ToString("F4"));  // Formatting to 4 decimal places
        //        Console.WriteLine("Step length Y set to: " + stepLengthY);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Step length Y text box not found.");
        //    }
        //}

        //public void setAmountOfStepsX(int xSteps)
        //{
        //    IntPtr hWndContainer = new IntPtr(0x001076E);  // The handle of the container (whole box)
        //    IntPtr hWndTextBox = IntPtr.Zero;

        //    EnumChildWindows(hWndContainer, (hWnd, lParam) =>
        //    {
        //        StringBuilder windowText = new StringBuilder(256);
        //        GetWindowText(hWnd, windowText, windowText.Capacity);

        //        if (windowText.ToString().Contains("Amount of steps in X"))
        //        {
        //            hWndTextBox = hWnd;
        //            return false;  // Stop enumeration
        //        }

        //        return true;  // Continue enumeration
        //    }, IntPtr.Zero);

        //    if (hWndTextBox != IntPtr.Zero)
        //    {
        //        SendMessage(hWndTextBox, WM_SETTEXT, IntPtr.Zero, xSteps.ToString());
        //        Console.WriteLine("Amount of Steps in X set to: " + xSteps);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Amount of Steps in X text box not found.");
        //    }
        //}

        //public void setAmountOfStepsY(int ySteps)
        //{
        //    IntPtr hWndContainer = new IntPtr(0x001076E);  // The handle of the container (whole box)
        //    IntPtr hWndTextBox = IntPtr.Zero;

        //    EnumChildWindows(hWndContainer, (hWnd, lParam) =>
        //    {
        //        StringBuilder windowText = new StringBuilder(256);
        //        GetWindowText(hWnd, windowText, windowText.Capacity);

        //        if (windowText.ToString().Contains("Amount of steps in Y"))
        //        {
        //            hWndTextBox = hWnd;
        //            return false;  // Stop enumeration
        //        }

        //        return true;  // Continue enumeration
        //    }, IntPtr.Zero);

        //    if (hWndTextBox != IntPtr.Zero)
        //    {
        //        SendMessage(hWndTextBox, WM_SETTEXT, IntPtr.Zero, ySteps.ToString());
        //        Console.WriteLine("Amount of Steps in Y set to: " + ySteps);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Amount of Steps in Y text box not found.");
        //    }
        //}
        //private const uint BM_CLICK = 0x00F5;
        //private const int WM_SETTEXT = 0x000C;
        //private const uint WM_GETTEXT = 0x000D;
        //private const uint WM_GETTEXTLENGTH = 0x000E;

        //[DllImport("user32.dll", SetLastError = true)]
        //public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        //// Aliasing the SendMessage method for clicking
        //[DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        //public static extern bool SendMessageClick(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        //// Aliasing the SendMessage method for setting text
        //[DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        //private static extern int SendMessageSetText(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        //// Aliasing the SendMessage method for retrieving text
        //[DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        //private static extern int SendMessageGetText(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

        //// Aliasing the SendMessage method for retrieving text length
        //[DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        //private static extern int SendMessageGetTextLength(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        //private IntPtr mainWindowHandle = new IntPtr(0x00090CD6); // Handle from the "Set / Get" window

        //// Method to simulate pressing the "Set Zero" button
        //public void SetZero()
        //{
        //    // Find the "Set Zero" button within the main window
        //    IntPtr setZeroButtonHandle = FindWindowEx(mainWindowHandle, IntPtr.Zero, "Button", "Set Zero");

        //    // Send a click message to the "Set Zero" button
        //    if (setZeroButtonHandle != IntPtr.Zero)
        //    {
        //        SendMessageClick(setZeroButtonHandle, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        //        Console.WriteLine("Set Zero button clicked.");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Set Zero button not found.");
        //    }
        //}

        //// Method to simulate pressing the "Pos -> Home" button
        //public void SetHome()
        //{
        //    // Find the "Pos -> Home" button within the main window
        //    IntPtr setHomeButtonHandle = FindWindowEx(mainWindowHandle, IntPtr.Zero, "Button", "Pos -> Home");

        //    // Send a click message to the "Pos -> Home" button
        //    if (setHomeButtonHandle != IntPtr.Zero)
        //    {
        //        SendMessageClick(setHomeButtonHandle, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        //        Console.WriteLine("Pos -> Home button clicked.");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Pos -> Home button not found.");
        //    }
        //}

        //// Method to simulate pressing the "Relative" button
        //public void MoveRelatively()
        //{
        //    if (this.InvokeRequired)
        //    {
        //        this.Invoke(new Action(MoveRelatively));
        //    }
        //    else
        //    {
        //        // Find the "Relative" button within the main window
        //        IntPtr relativeButtonHandle = FindWindowEx(this.Handle, IntPtr.Zero, "Button", "Relative");

        //        // Send a click message to the "Relative" button
        //        SendMessageClick(relativeButtonHandle, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        //    }
        //}

        //// Method to simulate pressing the "Home" button
        //public void Home()
        //{
        //    if (this.InvokeRequired)
        //    {
        //        // If the current thread is not the UI thread, marshal the call to the UI thread
        //        this.Invoke(new Action(Home));
        //    }
        //    else
        //    {
        //        // Find the "Home" button within the main window
        //        IntPtr homeButtonHandle = FindWindowEx(this.Handle, IntPtr.Zero, "Button", "Home");

        //        // Send a click message to the "Home" button
        //        SendMessageClick(homeButtonHandle, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        //    }
        //}

        //// Method to write the X position
        //public void WriteXPosition(double xPosition)
        //{
        //    // Replace with the actual handle of the X textbox
        //    IntPtr hWndTextBoxX = new IntPtr(0x00061ADA);  // Example handle, replace with actual if different

        //    if (hWndTextBoxX == IntPtr.Zero)
        //    {
        //        throw new Exception("X Position text box handle is invalid.");
        //    }

        //    string textToWrite = xPosition.ToString("F4"); // Formatting to 4 decimal places

        //    SendMessageSetText(hWndTextBoxX, WM_SETTEXT, IntPtr.Zero, textToWrite);
        //}

        //// Method to write the Y position
        //public void WriteYPosition(double yPosition)
        //{
        //    // Replace with the actual handle of the Y textbox
        //    IntPtr hWndTextBoxY = new IntPtr(0x00061ADB);  // Example handle, replace with actual if different

        //    if (hWndTextBoxY == IntPtr.Zero)
        //    {
        //        throw new Exception("Y Position text box handle is invalid.");
        //    }

        //    string textToWrite = yPosition.ToString("F4"); // Formatting to 4 decimal places

        //    SendMessageSetText(hWndTextBoxY, WM_SETTEXT, IntPtr.Zero, textToWrite);
        //}

        //// Method to get the X position
        //public double getXPos()
        //{
        //    IntPtr hWndTextBox = new IntPtr(0x000D0A60);  // Handle from your screenshot

        //    if (hWndTextBox == IntPtr.Zero)
        //    {
        //        throw new Exception("Text box handle is invalid.");
        //    }

        //    StringBuilder sb = new StringBuilder(256); // Arbitrary large buffer size
        //    int textLength = SendMessageGetText(hWndTextBox, WM_GETTEXT, sb.Capacity, sb);

        //    if (textLength > 0)
        //    {
        //        if (double.TryParse(sb.ToString(), out double result))
        //        {
        //            return result;
        //        }
        //        else
        //        {
        //            throw new Exception("Failed to parse X Position to a double.");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Text box is empty or text could not be retrieved.");
        //        throw new Exception("X Position text box is empty.");
        //    }
        //}


        //// Method to get the Y position
        //public double getYPos()
        //{
        //    IntPtr hWndTextBox = new IntPtr(0x000D0A5C);  // Handle from your screenshot

        //    if (hWndTextBox == IntPtr.Zero)
        //    {
        //        throw new Exception("Text box handle is invalid.");
        //    }

        //    // Get the length of the text in the textbox
        //    int length = SendMessageGetTextLength(hWndTextBox, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);

        //    if (length > 0)
        //    {
        //        StringBuilder sb = new StringBuilder(length + 1); // +1 for the null terminator
        //        SendMessageGetText(hWndTextBox, WM_GETTEXT, sb.Capacity, sb);

        //        if (double.TryParse(sb.ToString(), out double result))
        //        {
        //            return result;
        //        }
        //        else
        //        {
        //            throw new Exception("Failed to parse Y Position to a double.");
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("Y Position text box is empty.");
        //    }
        //}

        [DllImport("User32.dll", SetLastError = true)]
        static extern Boolean MessageBeep(UInt32 beepType);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_CreateLSID(out Int32 plID);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_FreeLSID(Int32 ID);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_ConnectSimple(Int32 ID, Int32 lAnInterfaceType, String pcAComName, Int32 lABaudRate, Int32 bAShowProt);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_Disconnect(Int32 ID);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_SetShowProt(Int32 ID, Int32 bAShowProt);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_GetPos(Int32 ID, out Double pdX, out Double pdY, out Double pdZ, out Double pdA);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_Calibrate(Int32 ID);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_CalibrateEx(Int32 ID, Int32 iFlags);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_RMeasure(Int32 ID);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_RMeasureEx(Int32 ID, Int32 iFlags);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_MoveAbs(Int32 ID, Double dX, Double dY, Double dZ, Double dA, Int32 bWait);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_MoveAbsSingleAxis(Int32 ID, Int32 lAAxis, Double dPosition, Int32 bAWait);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_MoveRelSingleAxis(Int32 ID, Int32 lAAxis, Double dDelta, Int32 bAWait);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_GetTriggerPar(Int32 ID, out Int32 plAAxis, out Int32 plAMode, out Int32 plALength, out Double pdADistance);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_SetTriggerPar(Int32 ID, Int32 lAAxis, Int32 lAMode, Int32 lALength, Double dADistance);

        //[DllImport("Tango_dll.dll", SetLastError = true)]
        //static extern Int32 LSX_GetTriggerEncoder(Int32 ID, out Int32 plTrgEnc);

        //[DllImport("Tango_dll.dll", SetLastError = true)]
        //static extern Int32 LSX_SetTriggerEncoder(Int32 ID, Int32 lTrgEnc);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_SetTrigger(Int32 ID, Int32 lATrg);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_SetTrigCount(Int32 ID, Int32 lATrgCnt);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_GetTrigCount(Int32 ID, out Int32 plATrgCnt);

        // here use "unsafe" declaration because of the returned ASCII string
        // -----
        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_GetStageSN(Int32 ID, byte[] pcAStageSN, Int32 iMaxLen);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_GetSerialNr(Int32 ID, byte[] pcASerialNr, Int32 iMaxLen);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_GetDLLVersionString(Int32 ID, byte[] pcAStageSN, Int32 iMaxLen);

        //[DllImport(@"C:\Program Files (x86)\SwitchBoard\Tango_DLL.dll", SetLastError = true)]
        //static extern Int32 LSX_GetTangoVersion(Int32 ID, byte[] pcAStageSN, Int32 iMaxLen);

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern Int32 LSX_GetVersionStrDet(Int32 ID, byte[] pcAStageSN, Int32 iMaxLen);
        // -----


        // Here you may add all other required Tango_dll.dll functions
        // For more details how to use standard DLL with C# source please read
        // http://msdn.microsoft.com/en-us/magazine/cc164123.aspx  

        [DllImport("Tango_dll.dll", SetLastError = true)]
        static extern int LSX_MoveAbs(int lLSID, double dX, double dY, double dZ, double dA, Boolean bWait);


        public Int32 lLSID = 0; // Here in the example only one LSID (one connection) is used for the LSX_ instructions of the DLL

        private void bt_connect_Click(object sender, EventArgs e)
        {
            Int32 loc_err;
            byte[] loc_str = new byte[128]; // Reserve sufficient space to read the TANGO Version string

            try
            {
                if (comboBox1.Text.Length <= 0) // No COM Port selected
                {
                    MessageBox.Show("Please select a COM port from the Connect list!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 0. Remove possibly opened LSIDs to establish only one connection
                while (lLSID > 0)
                {
                    LSX_Disconnect(lLSID);  // Disconnect the port of the LSID
                    LSX_FreeLSID(lLSID);    // Remove the LSID
                    lLSID -= 1;             // Decrease and repeat until LSID = 0
                }

                // 1. Let the DLL create a free LSID
                loc_err = LSX_CreateLSID(out lLSID);
                if (loc_err != 0)
                {
                    MessageBox.Show("Error: " + loc_err.ToString(), "LSX_CreateLSID()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                label24.Text = "lLSID: " + lLSID.ToString();

                // 2. Read the DLL Version
                loc_err = LSX_GetDLLVersionString(lLSID, loc_str, loc_str.Length);
                if (loc_err != 0)
                {
                    MessageBox.Show("Error: " + loc_err.ToString(), "LSX_GetDLLVersionString()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string sDLLversion = System.Text.Encoding.UTF8.GetString(loc_str).Replace("\0", string.Empty);
                    label25.Text = sDLLversion;
                }

                // 3. Connect to the selected COM Port
                Int32 ShowProt = checkBox1.Checked ? 1 : 0;
                loc_err = LSX_ConnectSimple(lLSID, 1, comboBox1.Text, 57600, ShowProt);
                if (loc_err != 0)
                {
                    MessageBox.Show("ConnectSimple() Error: " + loc_err.ToString());
                    return;
                }

                EnableTools(true);

                // 4. When connected, readout information from the TANGO and display them on the labels

                //// Read TANGO version
                //loc_str.Initialize();
                //loc_err = LSX_GetTangoVersion(lLSID, loc_str, loc_str.Length);
                //if (loc_err != 0)
                //{
                //    MessageBox.Show("Error: " + loc_err.ToString(), "LSX_GetTangoVersion()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                //else
                //{
                //    string sTangoVersion = System.Text.Encoding.UTF8.GetString(loc_str).Replace("\0", string.Empty);
                //    label26.Text = sTangoVersion;
                //}

                // Read Serial Number
                loc_str.Initialize();
                loc_err = LSX_GetSerialNr(lLSID, loc_str, loc_str.Length);
                if (loc_err != 0)
                {
                    MessageBox.Show("Error: " + loc_err.ToString(), "LSX_GetSerialNr()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string sTangoSN = System.Text.Encoding.UTF8.GetString(loc_str).Replace("\0", string.Empty);
                    label47.Text = "Tango SN = " + sTangoSN;
                }

                // Read Stage Serial Number
                loc_str.Initialize();
                loc_err = LSX_GetStageSN(lLSID, loc_str, loc_str.Length);
                if (loc_err != 0)
                {
                    MessageBox.Show("Error: " + loc_err.ToString(), "LSX_GetStageSN()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string sStageSN = System.Text.Encoding.UTF8.GetString(loc_str).Replace("\0", string.Empty);
                    label48.Text = "Stage SN = " + sStageSN;
                }

                // Read Version String Detail
                //loc_str.Initialize();
                //loc_err = LSX_GetVersionStrDet(lLSID, loc_str, loc_str.Length);
                //if (loc_err != 0)
                //{
                //    MessageBox.Show("Error: " + loc_err.ToString(), "LSX_GetVersionStrDet()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                //else
                //{
                //    //You can process the version string detail here as needed
                //    string sDet = System.Text.Encoding.UTF8.GetString(loc_str).Replace("\0", string.Empty);
                //    int iDet = Int32.Parse(sDet);
                //    if ((iDet & 0x2000) == 0)
                //    {
                //        MessageBox.Show("The Tango option TRIGGER is disabled. Please contact sales at Marzhauser.", "Configuration Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }
                //    label49.Text = "DET = 0x" + iDet.ToString("X");
                //}

                // Get Trigger Encoder status
                //Int32 iTrgEnc;
                //loc_err = LSX_GetTriggerEncoder(lLSID, out iTrgEnc);
                //if (loc_err != 0)
                //{
                //    MessageBox.Show("Error: " + loc_err.ToString(), "GetTriggerEncoder()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                //checkBox3.Checked = iTrgEnc == 1;

                // Set and Get Trigger Parameters
                //Int32 aAxis = 1;         // X axis is used as LS_GetTriggerEncoder source
                //Int32 aMode = 6;         // Mode details per Tango command reference manual
                //Int32 aLength = 40;      // 40µs pulse width
                //Double aDistance = 1.0;  // Trigger distance

                Double aXStepLength = 0.036;
                Double aYStepLength = 0.024;
                //loc_err = LSX_SetTriggerPar(lLSID, aAxis, aMode, aLength, aDistance);
                //if (loc_err != 0)
                //{
                //    MessageBox.Show("Error: " + loc_err.ToString(), "SetTriggerPar()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}

                //// Read back parameters from controller
                //loc_err = LSX_GetTriggerPar(lLSID, out aAxis, out aMode, out aLength, out aDistance);
                //if (loc_err != 0)
                //{
                //    MessageBox.Show("Error: " + loc_err.ToString(), "GetTriggerPar()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}

                //textBox1.Text = aDistance.ToString("F4");
                //textBox2.Text = aLength.ToString();
                //textBox3.Text = aMode.ToString();
                //textBox4.Text = aAxis.ToString();
                textBox1.Text = aXStepLength.ToString();
                textBox2.Text = aYStepLength.ToString();

                // Get Trigger Count
                Int32 iTrgCnt;
                loc_err = LSX_GetTrigCount(lLSID, out iTrgCnt);
                if (loc_err != 0)
                {
                    MessageBox.Show("Error: " + loc_err.ToString(), "GetTrigCount()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                label23.Text = iTrgCnt.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                EnableTools(false);
            }
        }

        private void bt_getpos_Click(object sender, EventArgs e)
        {
            UpdatePosition();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Int32 ShowProt = 0;
                if (checkBox1.Checked == true) ShowProt = 1;
                LSX_SetShowProt(lLSID, ShowProt);
            }
            catch
            {
                MessageBeep(0);
            }
        }

        private void SelectFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select the folder where you want to save your photos";
                folderDialog.ShowNewFolderButton = true;

                // 显示对话框并获取用户选择的结果
                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    // 获取用户选择的路径
                    string selectedPath = folderDialog.SelectedPath;

                    // 获取用户信息、当前日期时间和拍摄图片个数
                    // string userID = GetUserID(); // 实际应用中，从用户数据中获取
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    int photoCount = GetPhotoCount(); // 实际应用中，获取拍摄的图片数量

                    // 生成固定命名格式的文件夹名称：用户ID_日期时间_图片数量Photos
                    string newFolderName = $"{timestamp}_{photoCount}Photos";
                    string newFolderPath = Path.Combine(selectedPath, newFolderName);

                    // 检查文件夹是否存在，如果不存在则创建它
                    if (!Directory.Exists(newFolderPath))
                    {
                        Directory.CreateDirectory(newFolderPath);
                    }

                    // 设置当前工作目录为新创建的文件夹路径
                    Directory.SetCurrentDirectory(newFolderPath);

                    // 可选：将新文件夹路径保存到控制器的模型中
                    _controller.GetModel().SaveDirectory = newFolderPath;

                    // 提示用户新创建的文件夹路径
                    MessageBox.Show("Selected Folder: " + newFolderPath, "Folder Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 更新UI中的Label文本为新创建的文件夹路径
                    folderPath.Text = "Selected Path: " + newFolderPath;
                    folderPath2.Text = newFolderPath;

                    // 创建一个 JSON 对象
                    var jsonObject = new
                    {
                        x_1 = x_1,
                        y_1 = y_1,
                        x_2 = x_2,
                        y_2 = y_2,
                        xStepLength = xStepLength,
                        yStepLength = yStepLength,
                        AEMode = aeMode1.SelectedItem,
                        TV = tv1.SelectedItem,
                        AV = av1.SelectedItem,
                        ISO = iso1.SelectedItem,
                        WhiteBalance = whiteBalance1.SelectedItem,
                        MeteringMode = meteringMode1.SelectedItem,
                        ExposureComp = exposureComp1.SelectedItem,
                        ImageQuality = imageQuality1.SelectedItem,
                        DriveMode = driveMode1.SelectedItem,
                        AFMode = afMode1.SelectedItem,
                        DCStrobe = flashMode1.SelectedItem,
                        MovieQuality = movieQuality1.SelectedItem,
                        MovieHFR = movieHFR1.SelectedItem,
                        PictureStyle = pictureStyle1.SelectedItem,
                        Aspect = aspect1.SelectedItem,
                    };

                    // 将 JSON 对象序列化为字符串
                    string jsonString = JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented);


                    // 定义 JSON 文件路径
                    string jsonFilePath = Path.Combine(newFolderPath, "data.json");

                    // 将 JSON 字符串写入文件
                    File.WriteAllText(jsonFilePath, jsonString);
                }
                else
                {
                    string defaultFolderPath = "C:\\Users\\nanouser\\AutoJeff\\RemoteControlWin\\sample\\CSharp\\CameraControl\\CameraControl\\Images";

                    // 设置当前工作目录为新创建的文件夹路径

                    Directory.SetCurrentDirectory(defaultFolderPath);

                    // 可选：将新文件夹路径保存到控制器的模型中
                    _controller.GetModel().SaveDirectory =defaultFolderPath;

                    // 更新UI中的Label文本为新创建的文件夹路径
                    folderPath.Text = "Selected Path: " + defaultFolderPath;
                    folderPath2.Text = defaultFolderPath;

                    MessageBox.Show("No folder selected. The default save location will be used.", "No Folder Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
        }





        private void actionButton1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                SelectFolder2.Enabled = false;
            }
            else
            {
                SelectFolder2.Enabled = true;
            }
        }

        private void SelectFolder2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select the folder where you want to save your photos";
                folderDialog.ShowNewFolderButton = true;

                // Show the dialog and get the result
                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    // 设置当前工作目录为用户选择的路径
                    Directory.SetCurrentDirectory(folderDialog.SelectedPath);

                    // 保存选择的路径到控制器的模型（可选）
                    _controller.GetModel().SaveDirectory = folderDialog.SelectedPath;

                    // 提示用户选择的路径
                    MessageBox.Show("Selected Folder: " + folderDialog.SelectedPath, "Folder Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 设置设计器中的Label文本为选择的路径
                    folderPath2.Text = folderDialog.SelectedPath;
                }
                else
                {
                    MessageBox.Show("No folder selected. The default save location will be used.", "No Folder Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void StartProcessingButton_Click(object sender, EventArgs e)
        {
            string pythonPath = @"C:\Users\nanouser\anaconda3\envs\Remote_Control_System"; // 请确保此路径指向你安装的Python目录
            Environment.SetEnvironmentVariable("PYTHONHOME", pythonPath);
            Environment.SetEnvironmentVariable("PYTHONPATH", pythonPath + @"\Lib");

            PythonEngine.Initialize(); // 初始化Python引擎
            string inputFolder = folderPath2.Text;
            string outputFolder = folderPath3.Text;

            if (string.IsNullOrWhiteSpace(inputFolder) || string.IsNullOrWhiteSpace(outputFolder))
            {
                MessageBox.Show("Please specify both input and output folder paths.");
                return;
            }

            // 调用Python脚本
            using (Py.GIL())
            {
                dynamic py = Py.Import("ImageProcess"); // 假设你的Python代码保存为process_images.py
                py.process_images_in_folder(inputFolder, outputFolder, new int[] { 100, 170 }, 5);
            }

            MessageBox.Show("Processing completed!");

        }

        private void SelectFolder3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select the folder where you want to save your photos";
                folderDialog.ShowNewFolderButton = true;

                // Show the dialog and get the result
                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    // 设置当前工作目录为用户选择的路径
                    Directory.SetCurrentDirectory(folderDialog.SelectedPath);

                    // 保存选择的路径到控制器的模型（可选）
                    _controller.GetModel().SaveDirectory = folderDialog.SelectedPath;

                    // 提示用户选择的路径
                    MessageBox.Show("Selected Folder: " + folderDialog.SelectedPath, "Folder Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 设置设计器中的Label文本为选择的路径
                    folderPath3.Text = folderDialog.SelectedPath;
                }
                else
                {
                    MessageBox.Show("No folder selected. The default save location will be used.", "No Folder Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void bt_disconnect_Click(object sender, EventArgs e)
        {
            if (lLSID > 0)
            {
                LSX_Disconnect(lLSID);
                LSX_FreeLSID(lLSID);
                lLSID = 0;
            }
        }

        private void EnableTools(Boolean bAEnable)
        {
            bt_getpos.Enabled = bAEnable;
            startingPositionButton.Enabled = bAEnable;
        }

        public void UpdatePosition()
        {
            Double xx, yy, zz, aa;
            try
            {
                LSX_GetPos(lLSID, out xx, out yy, out zz, out aa);
                label46.Text = xx.ToString("F4");
                label40.Text = yy.ToString("F4");
                label41.Text = zz.ToString("F4");
                label42.Text = aa.ToString("F4");
            }
            catch
            {
            }
        }



    }
}
