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

            startTango();
            MessageBox.Show("Please Connect Manually Following the Instruction:" +
                "\n 1. Select COM5. " +
                "\n 2. Click \"connect\".", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // await Task.Delay(500);
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
            
            // Updated by Jeff 08/12/2024
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
            // Set the position to zero asynchronously
            await Task.Run(() => SetZero());

            // Ensure that the position is set before getting the X and Y positions
            x_1 = await Task.Run(() => getXPos());
            y_1 = await Task.Run(() => getYPos());

            // Set the home position asynchronously
            await Task.Run(() => SetHome());

            // Enable the ending position button only after setting the home position
            endingPositionButton.Enabled = true;

            Console.WriteLine($"The starting position is set to: ({x_1}, {y_1})");
        }


        private async void EndingPositionButton_Click(object sender, EventArgs e)
        {
            // Ensure that the position is set before getting the X and Y positions
            x_2 = await Task.Run(() => getXPos());
            y_2 = await Task.Run(() => getYPos());

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
            await Task.Run(() => Home());

            // Real x step length should be 0.0414 approximately 240um
            // Real y step length should be 0.0281 approximately 160um

            // in this case, we choose x step length to be 0.036
            // in this case, we choose y step length to be 0.024 instead
            double xStepLength = 0.0360;
            double yStepLength = 0.0240;

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
                        WriteYPosition(y);
                        MoveRelatively();
                        Console.WriteLine("y plused");
                    }
                    else if (x + xStepLength < x_2)
                    {
                        x += xStepLength;
                        y = y_1;
                        WriteXPosition(x);
                        WriteYPosition(0.0000);
                        MoveRelatively();
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
            await Task.Run(() => Home());
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
        private const uint BM_CLICK = 0x00F5;
        private const int WM_SETTEXT = 0x000C;
        private const uint WM_GETTEXT = 0x000D;
        private const uint WM_GETTEXTLENGTH = 0x000E;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        // Aliasing the SendMessage method for clicking
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        public static extern bool SendMessageClick(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        // Aliasing the SendMessage method for setting text
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        private static extern int SendMessageSetText(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        // Aliasing the SendMessage method for retrieving text
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SendMessageGetText(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

        // Aliasing the SendMessage method for retrieving text length
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        private static extern int SendMessageGetTextLength(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private IntPtr mainWindowHandle = new IntPtr(0x00090CD6); // Handle from the "Set / Get" window

        // Method to simulate pressing the "Set Zero" button
        public void SetZero()
        {
            // Find the "Set Zero" button within the main window
            IntPtr setZeroButtonHandle = FindWindowEx(mainWindowHandle, IntPtr.Zero, "Button", "Set Zero");

            // Send a click message to the "Set Zero" button
            if (setZeroButtonHandle != IntPtr.Zero)
            {
                SendMessageClick(setZeroButtonHandle, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                Console.WriteLine("Set Zero button clicked.");
            }
            else
            {
                Console.WriteLine("Set Zero button not found.");
            }
        }

        // Method to simulate pressing the "Pos -> Home" button
        public void SetHome()
        {
            // Find the "Pos -> Home" button within the main window
            IntPtr setHomeButtonHandle = FindWindowEx(mainWindowHandle, IntPtr.Zero, "Button", "Pos -> Home");

            // Send a click message to the "Pos -> Home" button
            if (setHomeButtonHandle != IntPtr.Zero)
            {
                SendMessageClick(setHomeButtonHandle, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                Console.WriteLine("Pos -> Home button clicked.");
            }
            else
            {
                Console.WriteLine("Pos -> Home button not found.");
            }
        }

        // Method to simulate pressing the "Relative" button
        public void MoveRelatively()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(MoveRelatively));
            }
            else
            {
                // Find the "Relative" button within the main window
                IntPtr relativeButtonHandle = FindWindowEx(this.Handle, IntPtr.Zero, "Button", "Relative");

                // Send a click message to the "Relative" button
                SendMessageClick(relativeButtonHandle, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
            }
        }

        // Method to simulate pressing the "Home" button
        public void Home()
        {
            if (this.InvokeRequired)
            {
                // If the current thread is not the UI thread, marshal the call to the UI thread
                this.Invoke(new Action(Home));
            }
            else
            {
                // Find the "Home" button within the main window
                IntPtr homeButtonHandle = FindWindowEx(this.Handle, IntPtr.Zero, "Button", "Home");

                // Send a click message to the "Home" button
                SendMessageClick(homeButtonHandle, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
            }
        }

        // Method to write the X position
        public void WriteXPosition(double xPosition)
        {
            // Replace with the actual handle of the X textbox
            IntPtr hWndTextBoxX = new IntPtr(0x00061ADA);  // Example handle, replace with actual if different

            if (hWndTextBoxX == IntPtr.Zero)
            {
                throw new Exception("X Position text box handle is invalid.");
            }

            string textToWrite = xPosition.ToString("F4"); // Formatting to 4 decimal places

            SendMessageSetText(hWndTextBoxX, WM_SETTEXT, IntPtr.Zero, textToWrite);
        }

        // Method to write the Y position
        public void WriteYPosition(double yPosition)
        {
            // Replace with the actual handle of the Y textbox
            IntPtr hWndTextBoxY = new IntPtr(0x00061ADB);  // Example handle, replace with actual if different

            if (hWndTextBoxY == IntPtr.Zero)
            {
                throw new Exception("Y Position text box handle is invalid.");
            }

            string textToWrite = yPosition.ToString("F4"); // Formatting to 4 decimal places

            SendMessageSetText(hWndTextBoxY, WM_SETTEXT, IntPtr.Zero, textToWrite);
        }

        // Method to get the X position
        public double getXPos()
        {
            IntPtr hWndTextBox = new IntPtr(0x000D0A60);  // Handle from your screenshot

            if (hWndTextBox == IntPtr.Zero)
            {
                throw new Exception("Text box handle is invalid.");
            }

            StringBuilder sb = new StringBuilder(256); // Arbitrary large buffer size
            int textLength = SendMessageGetText(hWndTextBox, WM_GETTEXT, sb.Capacity, sb);

            if (textLength > 0)
            {
                if (double.TryParse(sb.ToString(), out double result))
                {
                    return result;
                }
                else
                {
                    throw new Exception("Failed to parse X Position to a double.");
                }
            }
            else
            {
                Console.WriteLine("Text box is empty or text could not be retrieved.");
                throw new Exception("X Position text box is empty.");
            }
        }


        // Method to get the Y position
        public double getYPos()
        {
            IntPtr hWndTextBox = new IntPtr(0x000D0A5C);  // Handle from your screenshot

            if (hWndTextBox == IntPtr.Zero)
            {
                throw new Exception("Text box handle is invalid.");
            }

            // Get the length of the text in the textbox
            int length = SendMessageGetTextLength(hWndTextBox, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);

            if (length > 0)
            {
                StringBuilder sb = new StringBuilder(length + 1); // +1 for the null terminator
                SendMessageGetText(hWndTextBox, WM_GETTEXT, sb.Capacity, sb);

                if (double.TryParse(sb.ToString(), out double result))
                {
                    return result;
                }
                else
                {
                    throw new Exception("Failed to parse Y Position to a double.");
                }
            }
            else
            {
                throw new Exception("Y Position text box is empty.");
            }
        }
    }
}
