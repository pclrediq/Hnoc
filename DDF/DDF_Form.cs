using System;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using System.Data;
using NationalInstruments;
using NationalInstruments.DAQmx;

namespace DDF
{
    public partial class DDF_Form : Form
    {
        public string[] NMCDesc = {
                                "NMC2_220S"
                                ,"NMC2_420S"
                                ,"NMC2_620S"
                                ,"NMC2_820S"
                                ,"NMC2_220_DIO32"
                                ,"NMC2_220_DIO64"
                                ,"NMC2_420_DIO32"
                                ,"NMC2_420_DIO64"
                                ,"NMC2_820_DIO32"
                                ,"NMC2_820_DIO64"
                                ,"NMC2_DIO32"
                                ,"NMC2_DIO64"
                                ,"NMC2_DIO96"
                                ,"NMC2_DIO128"
                                ,"NMC2_220"
                                ,"NMC2_420"
                                ,"NMC2_620"
                                ,"NMC2_820"
                                ,"NMC2_620_DIO32"
                                ,"NMC2_620_DIO64"
                                ,null
                                };
        SerialPort serialport = new SerialPort();
        CRCTable CRCcal = new CRCTable();

        public static byte[] Sendbuff = new byte[10];
        public static byte[] Readbuff = new byte[10];
        static long value = 0;
        static long value_hi = 0;
        static long value_low = 0;
        static byte[] Data = new byte[2];

        public static double XRatio = 0.0000179;
        public static double XStartSpeed = 1;
        public static double XAcc = 6.278;
        public static double XDec = 6.278;
        public static double XMax = 10.417;
        public static double YRatio = 0.0000358;
        public static double YStartSpeed = 1;
        public static double YAcc = 10.455;
        public static double YDec = 10.455;
        public static double YMax = 16.667;

        public bool flag_ch1_set = false;
        public bool flag_ch2_set = false;
        public bool flag_ch3_set = false;
        public bool flag_ch4_set = false;
        public bool flag_disconnect = false;

        public int ch1_goal_value;
        public int ch2_goal_value;
        public int ch3_goal_value;
        public int ch4_goal_value;



        PaixMotion PaixMotion;
        Thread TdWatchSensor;
        Thread TdTempControl;

        DDF.NMC2.NMCAXESEXPR NmcData = new DDF.NMC2.NMCAXESEXPR(); // 모션 제어 라이브러리(8축의 현재 센서입력 상태, 위치값, 보간 정보 등을 확인하는 구조체)

        public DDF_Form()
        {
            InitializeComponent();

            Console.WriteLine("============================================= DDF Control Form 팝업 완료 =============================================");

            PaixMotion = new PaixMotion();
            TdWatchSensor = new Thread(new ThreadStart(watchSensor));
            TdTempControl = new Thread(new ThreadStart(tempControl));
            
            button_XJogLeft.Enabled = false;
            button_XJogRight.Enabled = false;
            button_XStop.Enabled = false;
            button_XHome.Enabled = false;
            button_YJogLeft.Enabled = false;
            button_YJogRight.Enabled = false;
            button_YStop.Enabled = false;
            label_XCmdVal.Enabled = false;
            label_YCmdVal.Enabled = false;
            label_XEncVal.Enabled = false;
            label_YEncVal.Enabled = false;
            comboBox_XServo.Enabled = false;
            comboBox_YServo.Enabled = false;
            btn_motioncontrol.Enabled = false;

            Ch1Goal_numericUpDown.Enabled = false;
            Ch2Goal_numericUpDown.Enabled = false;
            Ch3Goal_numericUpDown.Enabled = false;
            Ch4Goal_numericUpDown.Enabled = false;
            button_Ch1_Set.Enabled = false;
            button_Ch2_Set.Enabled = false;
            button_Ch3_Set.Enabled = false;
            button_Ch4_Set.Enabled = false;

            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
            if (physicalChannelComboBox.Items.Count > 0)
                physicalChannelComboBox.SelectedIndex = 0;
        }

        private void btn_TempConnect_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Temp 시리얼 포트 연결을 시작합니다.");
            if (!serialport.IsOpen)
            {
                // 시리얼 포트 설정(NI MAX에서 통신 포트 확인 후 정확하게 입력해야함)
                serialport.PortName = "COM3";
                serialport.BaudRate = 9600;
                serialport.DataBits = 8;
                serialport.StopBits = StopBits.One;
                serialport.Parity = Parity.None;

                serialport.DataReceived += new SerialDataReceivedEventHandler(serialport_DataReceived);

                try
                {
                    // 시리얼 포트 오픈    
                    serialport.Open();
                    serialport.DiscardInBuffer();
                    Console.WriteLine("Temp 시리얼 포트 연결을 완료하였습니다.");
                }
                catch (Exception ex1)
                {
                    Console.WriteLine("Temp 온도 시리얼 포트 연결에 실패하였습니다.");
                    MessageBox.Show(ex1.Message, "Error");
                }

                //SetTimer();

                switch (TdTempControl.ThreadState)
                {
                    case ThreadState.Stopped:
                        TdTempControl = new Thread(new ThreadStart(tempControl));
                        break;
                    case ThreadState.Unstarted:
                        break;
                    default:
                        TdTempControl.Abort();
                        while (TdTempControl.ThreadState != ThreadState.Stopped) { }
                        break;
                }

                TdTempControl.Start();

                Console.WriteLine(TdTempControl.ThreadState); //Running이 나옴
                Console.WriteLine("Temp 시리얼 포트가 연결 되어있는가? : '" + serialport.IsOpen +"'"); //False, True가 나옴

                btn_TempConnect.Enabled = false;
                btn_TempDisconnect.Enabled = true;
            }
        }

        private delegate void delegateUpdateTmpCtrl();

        public void tempControl()
        {
            Console.WriteLine("bye");
            try
            {
                while (true)
                {
                    Console.WriteLine("TempControl Thread 구동");
                    System.Threading.Thread.Sleep(2000);
                    Console.WriteLine(TdTempControl.ThreadState);
                    this.Invoke(new delegateUpdateTmpCtrl(updateTempCtrl));
                }
            }
            catch (ThreadInterruptedException e)
            {
                e.ToString(); 
                Console.WriteLine("TempControl Thread 에러");
            }
            finally
            {
                Console.WriteLine("Temp finally 절 구동");
            }
        }

        private void updateTempCtrl() // 온도 컨트롤
        {
            // function 3 : Ch의 설정 온도를 불러옴. function 4 : Ch의 현재 온도를 불러옴. function 6 : Ch에 목표 온도를 설정함.

            ///////////////////////////////////////////
            //               채널 1 루틴
            ///////////////////////////////////////////
            //                현재 온도
            ///////////////////////////////////////////

            Sendbuff[0] = 0x01;
            Sendbuff[1] = 0x04;
            Sendbuff[2] = 0x03;
            Sendbuff[3] = 0xE8;
            Sendbuff[4] = 0x00;
            Sendbuff[5] = 0x01;
            Sendbuff[6] = 0xB1;
            Sendbuff[7] = 0xBA;  

            fn_Send(Sendbuff, 0, 8);
            Thread.Sleep(20);
            Readbuff[0] = Convert.ToByte(serialport.ReadByte());
            Readbuff[1] = Convert.ToByte(serialport.ReadByte());
            Readbuff[2] = Convert.ToByte(serialport.ReadByte());

            if (Readbuff[0] == 0x01 && Readbuff[1] == 0x04 && Readbuff[2] == 0x02) // 수신 데이터가 정상일 경우
            {
                Console.WriteLine("'Ch1 현재 온도 출력 시작");
                Readbuff[3] = Convert.ToByte(serialport.ReadByte());
                Readbuff[4] = Convert.ToByte(serialport.ReadByte());
                Readbuff[5] = Convert.ToByte(serialport.ReadByte());
                Readbuff[6] = Convert.ToByte(serialport.ReadByte());

                Data[0] = Readbuff[3]; // 256의 자리
                Data[1] = Readbuff[4]; // 1~255의 자리
                value_hi = Data[0] * 256;
                value_low = Data[1];
                value = value_hi + value_low;                           // 계산한 온도 값
                textBox_Ch1_Current.Text = value + "'C";                // main form 화면의 Ch1_Current 텍스트에 출력
                Console.WriteLine("Ch1 현재 온도 출력 중...'");
            }
            else // 수신 데이터의 오류 발생시
            {
                Console.WriteLine("err - Ch1 현재 온도 출력 오류");
                textBox_Ch1_Current.Text = "err";
                serialport.DiscardInBuffer();
            }

            ///////////////////////////////////////////
            //           Ch1 설정 온도 출력
            ///////////////////////////////////////////

            Sendbuff[0] = 0x01;
            Sendbuff[1] = 0x03;
            Sendbuff[2] = 0x00;
            Sendbuff[3] = 0x00;
            Sendbuff[4] = 0x00;
            Sendbuff[5] = 0x01;
            Sendbuff[6] = 0x84;
            Sendbuff[7] = 0x0A;
            fn_Send(Sendbuff, 0, 8);
            Thread.Sleep(20);

            Readbuff[0] = Convert.ToByte(serialport.ReadByte());
            Readbuff[1] = Convert.ToByte(serialport.ReadByte());
            Readbuff[2] = Convert.ToByte(serialport.ReadByte());

            if (Readbuff[0] == 0x01 && Readbuff[1] == 0x03)
            {
                Console.WriteLine("'Ch1 목표 온도 로드 시작");
                Readbuff[3] = Convert.ToByte(serialport.ReadByte());
                Readbuff[4] = Convert.ToByte(serialport.ReadByte());
                Readbuff[5] = Convert.ToByte(serialport.ReadByte());
                Readbuff[6] = Convert.ToByte(serialport.ReadByte());

                Data[0] = Readbuff[3]; // 256의 자리
                Data[1] = Readbuff[4]; // 1~255의 자리
                value_hi = Data[0] * 256;
                value_low = Data[1];
                value = value_hi + value_low;                                   // 목표온도값 계산
                textBox_Ch1_Setted.Text = value + "'C";                         // 목표온도값 출력
                Console.WriteLine("Ch1 목표 온도 로드 완료'");
            }
            else
            {
                Console.WriteLine("err - Ch1 설정 온도 로드 오류");
                textBox_Ch1_Setted.Text = "err";
                serialport.DiscardInBuffer();
            }

            ///////////////////////////////////////////
            //               채널 2 루틴
            ///////////////////////////////////////////
            //                현재 온도
            ///////////////////////////////////////////
            
            Sendbuff[0] = 0x01;
            Sendbuff[1] = 0x04;
            Sendbuff[2] = 0x03;
            Sendbuff[3] = 0xEE;
            Sendbuff[4] = 0x00;
            Sendbuff[5] = 0x01;
            Sendbuff[6] = 0x51;
            Sendbuff[7] = 0xBB;

            fn_Send(Sendbuff, 0, 8);
            Thread.Sleep(20);

            Readbuff[0] = Convert.ToByte(serialport.ReadByte());
            Readbuff[1] = Convert.ToByte(serialport.ReadByte());
            Readbuff[2] = Convert.ToByte(serialport.ReadByte());

            if (Readbuff[0] == 0x01 && Readbuff[1] == 0x04 && Readbuff[2] == 0x02)
            {
                Console.WriteLine("'Ch2 현재 온도 출력 시작");
                Readbuff[3] = Convert.ToByte(serialport.ReadByte());
                Readbuff[4] = Convert.ToByte(serialport.ReadByte());
                Readbuff[5] = Convert.ToByte(serialport.ReadByte());
                Readbuff[6] = Convert.ToByte(serialport.ReadByte());

                Data[0] = Readbuff[3]; // 256의 자리
                Data[1] = Readbuff[4]; // 1~255의 자리
                value_hi = Data[0] * 256;
                value_low = Data[1];
                value = value_hi + value_low;
                textBox_Ch2_Current.Text = value + "'C";
                Console.WriteLine("Ch2 현재 온도 출력 중...'");
            }
            else
            {
                Console.WriteLine("err - Ch2 현재 온도 출력 오류");
                textBox_Ch2_Current.Text = "err";
                serialport.DiscardInBuffer();
            }

            ///////////////////////////////////////////
            //           Ch2 설정 온도 출력
            ///////////////////////////////////////////

            Sendbuff[0] = 0x01;
            Sendbuff[1] = 0x03;
            Sendbuff[2] = 0x03;
            Sendbuff[3] = 0xE8;
            Sendbuff[4] = 0x00;
            Sendbuff[5] = 0x01;
            Sendbuff[6] = 0x04;
            Sendbuff[7] = 0x7A;

            fn_Send(Sendbuff, 0, 8);
            Thread.Sleep(20);

            Readbuff[0] = Convert.ToByte(serialport.ReadByte());
            Readbuff[1] = Convert.ToByte(serialport.ReadByte());
            Readbuff[2] = Convert.ToByte(serialport.ReadByte());

            if (Readbuff[0] == 0x01 && Readbuff[1] == 0x03)
            {
                Console.WriteLine("'Ch2 목표 온도 로드 시작");
                Readbuff[3] = Convert.ToByte(serialport.ReadByte());
                Readbuff[4] = Convert.ToByte(serialport.ReadByte());
                Readbuff[5] = Convert.ToByte(serialport.ReadByte());
                Readbuff[6] = Convert.ToByte(serialport.ReadByte());

                Data[0] = Readbuff[3]; // 256의 자리
                Data[1] = Readbuff[4]; // 1~255의 자리
                value_hi = Data[0] * 256;
                value_low = Data[1];
                value = value_hi + value_low;
                textBox_Ch2_Setted.Text = value + "'C";
                Console.WriteLine("Ch2 목표 온도 로드 완료'");
            }
            else
            {
                Console.WriteLine("err - Ch2 목표 온도 로드 오류");
                textBox_Ch2_Setted.Text = "err";
                serialport.DiscardInBuffer();
            }



            ///////////////////////////////////////////
            //               채널 3 루틴
            ///////////////////////////////////////////
            //                현재 온도
            ///////////////////////////////////////////
            
            Sendbuff[0] = 0x01;
            Sendbuff[1] = 0x04;
            Sendbuff[2] = 0x03;
            Sendbuff[3] = 0xF4;
            Sendbuff[4] = 0x00;
            Sendbuff[5] = 0x01;
            Sendbuff[6] = 0x70;
            Sendbuff[7] = 0x7C;

            fn_Send(Sendbuff, 0, 8);
            Thread.Sleep(20);

            Readbuff[0] = Convert.ToByte(serialport.ReadByte());
            Readbuff[1] = Convert.ToByte(serialport.ReadByte());
            Readbuff[2] = Convert.ToByte(serialport.ReadByte());

            if (Readbuff[0] == 0x01 && Readbuff[1] == 0x04 && Readbuff[2] == 0x02)
            {
                Console.WriteLine("'Ch3 현재 온도 출력 시작");
                Readbuff[3] = Convert.ToByte(serialport.ReadByte());
                Readbuff[4] = Convert.ToByte(serialport.ReadByte());
                Readbuff[5] = Convert.ToByte(serialport.ReadByte());
                Readbuff[6] = Convert.ToByte(serialport.ReadByte());

                Data[0] = Readbuff[3]; // 256의 자리
                Data[1] = Readbuff[4]; // 1~255의 자리
                value_hi = Data[0] * 256;
                value_low = Data[1];
                value = value_hi + value_low;
                textBox_Ch3_Current.Text = value + "'C";
                Console.WriteLine("Ch3 현재 온도 출력 중...'");
            }
            else
            {
                Console.WriteLine("err - Ch3 현재 온도 출력 오류");
                textBox_Ch3_Current.Text = "err";
                serialport.DiscardInBuffer();
            }

            ///////////////////////////////////////////
            //           Ch3 설정 온도 출력
            ///////////////////////////////////////////

            Sendbuff[0] = 0x01;
            Sendbuff[1] = 0x03;
            Sendbuff[2] = 0x07;
            Sendbuff[3] = 0xD0;
            Sendbuff[4] = 0x00;
            Sendbuff[5] = 0x01;
            Sendbuff[6] = 0x84;
            Sendbuff[7] = 0x87;

            fn_Send(Sendbuff, 0, 8);
            Thread.Sleep(20);

            Readbuff[0] = Convert.ToByte(serialport.ReadByte());
            Readbuff[1] = Convert.ToByte(serialport.ReadByte());
            Readbuff[2] = Convert.ToByte(serialport.ReadByte());

            if (Readbuff[0] == 0x01 && Readbuff[1] == 0x03)
            {
                Console.WriteLine("'Ch3 목표 온도 로드 시작");
                Readbuff[3] = Convert.ToByte(serialport.ReadByte());
                Readbuff[4] = Convert.ToByte(serialport.ReadByte());
                Readbuff[5] = Convert.ToByte(serialport.ReadByte());
                Readbuff[6] = Convert.ToByte(serialport.ReadByte());

                Data[0] = Readbuff[3]; // 256의 자리
                Data[1] = Readbuff[4]; // 1~255의 자리
                value_hi = Data[0] * 256;
                value_low = Data[1];
                value = value_hi + value_low;
                textBox_Ch3_Setted.Text = value + "'C";
                Console.WriteLine("Ch3 목표 온도 로드 완료'");
            }
            else
            {
                Console.WriteLine("err - Ch3 목표 온도 로드 오류");
                textBox_Ch3_Setted.Text = "err";
                serialport.DiscardInBuffer();
            }


            ///////////////////////////////////////////
            //               채널 4 루틴
            ///////////////////////////////////////////
            //                현재 온도
            ///////////////////////////////////////////
            
            Sendbuff[0] = 0x01;
            Sendbuff[1] = 0x04;
            Sendbuff[2] = 0x03;
            Sendbuff[3] = 0xFA;
            Sendbuff[4] = 0x00;
            Sendbuff[5] = 0x01;
            Sendbuff[6] = 0x11;
            Sendbuff[7] = 0xBF;

            fn_Send(Sendbuff, 0, 8);
            Thread.Sleep(20);

            Readbuff[0] = Convert.ToByte(serialport.ReadByte());
            Readbuff[1] = Convert.ToByte(serialport.ReadByte());
            Readbuff[2] = Convert.ToByte(serialport.ReadByte());

            if (Readbuff[0] == 0x01 && Readbuff[1] == 0x04 && Readbuff[2] == 0x02)
            {
                Console.WriteLine("'Ch4 현재 온도 출력 시작");
                Readbuff[3] = Convert.ToByte(serialport.ReadByte());
                Readbuff[4] = Convert.ToByte(serialport.ReadByte());
                Readbuff[5] = Convert.ToByte(serialport.ReadByte());
                Readbuff[6] = Convert.ToByte(serialport.ReadByte());

                Data[0] = Readbuff[3]; // 256의 자리
                Data[1] = Readbuff[4]; // 1~255의 자리
                value_hi = Data[0] * 256;
                value_low = Data[1];
                value = value_hi + value_low;
                textBox_Ch4_Current.Text = value + "'C";
                Console.WriteLine("Ch4 현재 온도 출력 중...'");
            }
            else
            {
                Console.WriteLine("err - Ch4 현재 온도 출력 오류");
                textBox_Ch4_Current.Text = "err";
                serialport.DiscardInBuffer();
            }

            ///////////////////////////////////////////
            //            Ch4 설정 온도 출력
            ///////////////////////////////////////////

            Sendbuff[0] = 0x01;
            Sendbuff[1] = 0x03;
            Sendbuff[2] = 0x0B;
            Sendbuff[3] = 0xB8;
            Sendbuff[4] = 0x00;
            Sendbuff[5] = 0x01;
            Sendbuff[6] = 0x06;
            Sendbuff[7] = 0x0B;

            fn_Send(Sendbuff, 0, 8);
            Thread.Sleep(20);

            Readbuff[0] = Convert.ToByte(serialport.ReadByte());
            Readbuff[1] = Convert.ToByte(serialport.ReadByte());
            Readbuff[2] = Convert.ToByte(serialport.ReadByte());

            if (Readbuff[0] == 0x01 && Readbuff[1] == 0x03)
            {
                Console.WriteLine("'Ch4 목표 온도 로드 시작");
                Readbuff[3] = Convert.ToByte(serialport.ReadByte());
                Readbuff[4] = Convert.ToByte(serialport.ReadByte());
                Readbuff[5] = Convert.ToByte(serialport.ReadByte());
                Readbuff[6] = Convert.ToByte(serialport.ReadByte());

                Data[0] = Readbuff[3]; // 256의 자리
                Data[1] = Readbuff[4]; // 1~255의 자리
                value_hi = Data[0] * 256;
                value_low = Data[1];
                value = value_hi + value_low;
                textBox_Ch4_Setted.Text = value + "'C";
                Console.WriteLine("Ch4 목표 온도 로드 완료'");
            }
            else
            {
                Console.WriteLine("err - Ch4 목표 온도 로드 오류");
                textBox_Ch4_Setted.Text = "err";
                serialport.DiscardInBuffer();
            }


            ///////////////////////////////////////////
            //               플래그 처리
            ///////////////////////////////////////////
            //               채널 1 처리
            ///////////////////////////////////////////

            if (flag_ch1_set == true)       // flag 값이 초기값(false)에서 목표값을 설정(클릭)한 뒤 true가 되었을때
            {
                ch1_goal_value = Decimal.ToInt32(Ch1Goal_numericUpDown.Value);
                Sendbuff[0] = 0x01;
                Sendbuff[1] = 0x06;
                Sendbuff[2] = 0x00;
                Sendbuff[3] = 0x34;
                Sendbuff[4] = Convert.ToByte(ch1_goal_value / 256);
                Sendbuff[5] = Convert.ToByte(ch1_goal_value % 256);
                Sendbuff[6] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[0];
                Sendbuff[7] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[1];

                fn_Send(Sendbuff, 0, 8);
                Thread.Sleep(20);

                Readbuff[0] = Convert.ToByte(serialport.ReadByte());
                Readbuff[1] = Convert.ToByte(serialport.ReadByte());
                Readbuff[2] = Convert.ToByte(serialport.ReadByte());
                if (Readbuff[0] == 0x01 && Readbuff[1] == 0x06)                 // 정상적으로 값을 처리
                {
                    Console.WriteLine("'Temp Ch1 목표 온도 설정 시작");
                    Readbuff[3] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[4] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[5] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[6] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[7] = Convert.ToByte(serialport.ReadByte());

                    flag_ch1_set = false;       // 원래 초기값으로 변경 (차후 목표값 다시 설정가능하기 위해)

                    Console.WriteLine("Temp Ch1 목표 온도 설정 완료'");
                    MessageBox.Show("Ch 1 목표 온도 설정 완료.", "Setting");
                }
                else                                                            // 그 외 경우
                {
                    Console.WriteLine("err - Ch1 목표 온도 설정 오류");
                    serialport.DiscardInBuffer();
                    flag_ch1_set = false;       // 원래 초기값으로 변경 (차후 목표값 다시 설정가능하기 위해)
                }
            }

            ///////////////////////////////////////////
            //               채널 2 처리
            if (flag_ch2_set == true)
            {
                ch2_goal_value = Decimal.ToInt32(Ch2Goal_numericUpDown.Value);
                Sendbuff[0] = 0x01;
                Sendbuff[1] = 0x06;
                Sendbuff[2] = 0x04;
                Sendbuff[3] = 0x1C;
                Sendbuff[4] = Convert.ToByte(ch2_goal_value / 256);
                Sendbuff[5] = Convert.ToByte(ch2_goal_value % 256);
                Sendbuff[6] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[0];
                Sendbuff[7] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[1];

                fn_Send(Sendbuff, 0, 8);
                Thread.Sleep(20);

                Readbuff[0] = Convert.ToByte(serialport.ReadByte());
                Readbuff[1] = Convert.ToByte(serialport.ReadByte());
                Readbuff[2] = Convert.ToByte(serialport.ReadByte());
                if (Readbuff[0] == 0x01 && Readbuff[1] == 0x06)
                {
                    Console.WriteLine("'Temp Ch2 목표 온도 설정 시작");
                    Readbuff[3] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[4] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[5] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[6] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[7] = Convert.ToByte(serialport.ReadByte());

                    flag_ch2_set = false;
                    Console.WriteLine("Temp Ch2 목표 온도 설정 완료'");
                    MessageBox.Show("Ch2 목표 온도 설정 완료.", "Setting");
                }
                else
                {
                    Console.WriteLine("err - Ch2 목표 온도 설정 오류");
                    serialport.DiscardInBuffer();
                    flag_ch2_set = false;
                }
            }

            ///////////////////////////////////////////
            //               채널 3 처리
            if (flag_ch3_set == true)
            {
                ch3_goal_value = decimal.ToInt32(Ch3Goal_numericUpDown.Value);
                Sendbuff[0] = 0x01;
                Sendbuff[1] = 0x06;
                Sendbuff[2] = 0x08;
                Sendbuff[3] = 0x04;
                Sendbuff[4] = Convert.ToByte(ch3_goal_value / 256);
                Sendbuff[5] = Convert.ToByte(ch3_goal_value % 256);
                Sendbuff[6] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[0];
                Sendbuff[7] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[1];

                fn_Send(Sendbuff, 0, 8);
                Thread.Sleep(20);

                Readbuff[0] = Convert.ToByte(serialport.ReadByte());
                Readbuff[1] = Convert.ToByte(serialport.ReadByte());
                Readbuff[2] = Convert.ToByte(serialport.ReadByte());
                if (Readbuff[0] == 0x01 && Readbuff[1] == 0x06)
                {
                    Console.WriteLine("'Temp Ch3 목표 온도 설정 시작");
                    Readbuff[3] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[4] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[5] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[6] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[7] = Convert.ToByte(serialport.ReadByte());

                    flag_ch3_set = false;
                    Console.WriteLine("Temp Ch3 목표 온도 설정 완료'");
                    MessageBox.Show("Ch3 목표 온도 설정 완료.", "Setting");
                }
                else
                {
                    Console.WriteLine("err - Ch3 목표 온도 설정 오류");
                    serialport.DiscardInBuffer();
                    flag_ch3_set = false;
                }
            }

            ///////////////////////////////////////////
            //               채널 4 처리
            if (flag_ch4_set == true)
            {
                ch4_goal_value = decimal.ToInt32(Ch4Goal_numericUpDown.Value);
                Sendbuff[0] = 0x01;
                Sendbuff[1] = 0x06;
                Sendbuff[2] = 0x0B;
                Sendbuff[3] = 0xEC;
                Sendbuff[4] = Convert.ToByte(ch4_goal_value / 256);
                Sendbuff[5] = Convert.ToByte(ch4_goal_value % 256);
                Sendbuff[6] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[0];
                Sendbuff[7] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[1];

                fn_Send(Sendbuff, 0, 8);
                Thread.Sleep(20);

                Readbuff[0] = Convert.ToByte(serialport.ReadByte());
                Readbuff[1] = Convert.ToByte(serialport.ReadByte());
                Readbuff[2] = Convert.ToByte(serialport.ReadByte());
                if (Readbuff[0] == 0x01 && Readbuff[1] == 0x06)
                {
                    Console.WriteLine("'Temp Ch4 목표 온도 설정 시작");
                    Readbuff[3] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[4] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[5] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[6] = Convert.ToByte(serialport.ReadByte());
                    Readbuff[7] = Convert.ToByte(serialport.ReadByte());

                    flag_ch4_set = false;
                    Console.WriteLine("Temp Ch4 목표 온도 설정 완료'");
                    MessageBox.Show("Ch4 목표 온도 설정 완료.", "Setting");
                }
                else
                {
                    Console.WriteLine("err - Ch3 목표 온도 설정 오류");
                    serialport.DiscardInBuffer();
                    flag_ch4_set = false;
                }
            }

            if (flag_disconnect == true)
            {
                ///////////////////////////////////////////
                //               채널 1 리셋
                Sendbuff[0] = 0x01;
                Sendbuff[1] = 0x06;
                Sendbuff[2] = 0x00;
                Sendbuff[3] = 0x34;
                Sendbuff[4] = 0x00;
                Sendbuff[5] = 0x00;
                Sendbuff[6] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[0];
                Sendbuff[7] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[1];

                fn_Send(Sendbuff, 0, 8);
                Thread.Sleep(20);

                serialport.DiscardInBuffer();

                ///////////////////////////////////////////
                //               채널 2 리셋
                Sendbuff[0] = 0x01;
                Sendbuff[1] = 0x06;
                Sendbuff[2] = 0x04;
                Sendbuff[3] = 0x1C;
                Sendbuff[4] = 0x00;
                Sendbuff[5] = 0x00;
                Sendbuff[6] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[0];
                Sendbuff[7] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[1];

                fn_Send(Sendbuff, 0, 8);
                Thread.Sleep(20);

                serialport.DiscardInBuffer();

                ///////////////////////////////////////////
                //               채널 3 리셋
                Sendbuff[0] = 0x01;
                Sendbuff[1] = 0x06;
                Sendbuff[2] = 0x08;
                Sendbuff[3] = 0x04;
                Sendbuff[4] = 0x00;
                Sendbuff[5] = 0x00;
                Sendbuff[6] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[0];
                Sendbuff[7] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[1];

                fn_Send(Sendbuff, 0, 8);
                Thread.Sleep(20);

                serialport.DiscardInBuffer();

                ///////////////////////////////////////////
                //               채널 4 리셋
                Sendbuff[0] = 0x01;
                Sendbuff[1] = 0x06;
                Sendbuff[2] = 0x0B;
                Sendbuff[3] = 0xEC;
                Sendbuff[4] = 0x00;
                Sendbuff[5] = 0x00;
                Sendbuff[6] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[0];
                Sendbuff[7] = CRCcal.fn_makeCRC16_byte(Sendbuff, 6)[1];

                fn_Send(Sendbuff, 0, 8);
                Thread.Sleep(20);

                serialport.DiscardInBuffer();


                //sequence.Dispose();

                flag_disconnect = false;
            }
        }

        private void btn_TempDisconnect_Click(object sender, EventArgs e)
        {
            // 수정 필요
            TdTempControl.Interrupt();
            TdTempControl.Join();
            while (TdTempControl.ThreadState != ThreadState.Stopped) { }
            serialport.Close();

            Console.WriteLine("Temp 시리얼포트가 연결되어있나요? : '" + serialport.IsOpen + "'");
            Console.WriteLine("Temp 시리얼포트가 연결 해제 되었습니다.");

            flag_disconnect = true;

            textBox_Ch1_Current.Text = "";
            textBox_Ch2_Current.Text = "";
            textBox_Ch3_Current.Text = "";
            textBox_Ch4_Current.Text = "";
            textBox_Ch1_Setted.Text = "";
            textBox_Ch2_Setted.Text = "";
            textBox_Ch3_Setted.Text = "";
            textBox_Ch4_Setted.Text = "";

            btn_TempConnect.Enabled = true;
            btn_TempDisconnect.Enabled = false;
        }

        public void fn_Send(byte[] buffer, int offset, int count)
        {
            if (btn_TempConnect.Enabled == false)
            {
                //보낼데이터(버퍼값), 번지수, 데이터 개수)
                serialport.Write(buffer, offset, count);
            }
        }

        private void serialport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(MySerialReceived));
        }

        private void MySerialReceived(object s, EventArgs e)
        {
            int ReveiveData = serialport.ReadByte();
        }

        private void button_Ch1_Set_Click(object sender, EventArgs e)
        {
            flag_ch1_set = true;
            Console.WriteLine("Temp Ch1 목표온도 전달 : " + Ch1Goal_numericUpDown.Value + " ℃");
            Ch1Goal_numericUpDown.Value = 0;
        }
        private void button_Ch2_Set_Click(object sender, EventArgs e)
        {
            flag_ch2_set = true;
            Console.WriteLine("Temp Ch2 목표온도 전달 : " + Ch2Goal_numericUpDown.Value + " ℃");
            Ch2Goal_numericUpDown.Value = 0;
        }
        private void button_Ch3_Set_Click(object sender, EventArgs e)
        {
            flag_ch3_set = true;
            Console.WriteLine("Temp Ch3 목표온도 전달 : " + Ch3Goal_numericUpDown.Value + " ℃");
            Ch3Goal_numericUpDown.Value = 0;
        }
        private void button_Ch4_Set_Click(object sender, EventArgs e)
        {
            flag_ch4_set = true;
            Console.WriteLine("Temp Ch4 목표온도 전달 : " + Ch4Goal_numericUpDown.Value + " ℃");
            Ch4Goal_numericUpDown.Value = 0;
        }

        /// <summary>
        /// Motion Control
        /// </summary>
        private delegate void delegateUpdateCmdEnc();

        public void watchSensor()
        {
            //PaixMotion.NMC2.NMC_AXES_EXPR NmcData;
            try
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(10);
                    this.Invoke(new delegateUpdateCmdEnc(updateCmdEnc));
                    Console.WriteLine("watch sensor threading!");
                }
            }
            catch (ThreadInterruptedException ex) { ex.ToString(); }
            finally { Console.WriteLine("watch sensor finally절 구동"); }
        }

        private void button_Connection_Click(object sender, EventArgs e)
        {

            short devId = Convert.ToInt16(textBox_DevNo.Text);

            if (button_Connection.Text == "Connect")
            {
                try
                {
                    Console.WriteLine("Paix NMC2 시리얼포트 연결을 시작합니다.");
                    Cursor.Current = Cursors.WaitCursor;

                    PaixMotion.Open(devId);

                    Console.WriteLine("Paix NMC2 시리얼포트 연결을 완료하였습니다.");                  
                    Cursor.Current = Cursors.Default;

                    MessageBox.Show("Paix NMC2 Motor Connect complete!", "Connect");
                }
                catch (Exception ex3)
                {
                    Console.WriteLine("Paix NMC2 시리얼포트 연결에 실패하였습니다.");
                    MessageBox.Show(ex3.Message, "Error");
                }

                switch (TdWatchSensor.ThreadState)
                {
                    case ThreadState.Stopped:
                        TdWatchSensor = new Thread(new ThreadStart(watchSensor));
                        break;
                    case ThreadState.Unstarted:
                        break;
                    default:
                        TdWatchSensor.Abort();
                        while (TdWatchSensor.ThreadState != ThreadState.Stopped) { }
                        break;
                }

                TdWatchSensor.Start();

                //x축, y축 조작변수 초기화
                //x축
                PaixMotion.SetUnitPulse(0, XRatio);
                PaixMotion.ServoOn(0, comboBox_XServo.SelectedIndex);
                PaixMotion.SetSpeedPPS(0, XStartSpeed, XAcc, XDec, XMax);
                PaixMotion.SetHomeSpeed(0, XMax, XMax - XDec, XMax - 2 * XDec);

                //y축
                PaixMotion.SetUnitPulse(1, YRatio);
                PaixMotion.ServoOn(1, comboBox_YServo.SelectedIndex);
                PaixMotion.SetSpeedPPS(1, YStartSpeed, YAcc, YDec, YMax);
                PaixMotion.SetHomeSpeed(1, YMax, YMax - YDec, YMax - 2 * YDec);
                Console.WriteLine("Paix x축, y축 조작변수 초기화 완료.");

                button_XJogLeft.Enabled = true;
                button_XJogRight.Enabled = true;
                button_XStop.Enabled = true;
                button_XHome.Enabled = true;
                button_YJogLeft.Enabled = true;
                button_YJogRight.Enabled = true;
                button_YStop.Enabled = true;
                label_XCmdVal.Enabled = true;
                label_YCmdVal.Enabled = true;
                label_XEncVal.Enabled = true;
                label_YEncVal.Enabled = true;
                comboBox_XServo.Enabled = true;
                comboBox_YServo.Enabled = true;
                button_Connection.Text = "Disconnect";
            }
            else if (button_Connection.Text == "Disconnect" && PaixMotion.Close())
            {
                Console.WriteLine("Paix NMC2 시리얼포트 연결 해제를 시작합니다.");

                TdWatchSensor.Interrupt();
                TdWatchSensor.Join();

                while (TdWatchSensor.ThreadState != ThreadState.Stopped) { }
                PaixMotion.ServoOn(0, 0);
                PaixMotion.ServoOn(1, 0);

                Console.WriteLine("Paix NMC2 시리얼포트 연결 해제를 완료하였습니다.");
                MessageBox.Show("Paix NMC2 Motor Disconnect complete!", "Disconnect");
                button_Connection.Text = "Connect";
            }
        }
        private void Servo_SelectedIndexChanged(object sender, EventArgs e) // 서보 모터 ON/OFF 변경 시 실행
        {
            PaixMotion.ServoOn(0, comboBox_XServo.SelectedIndex);
            PaixMotion.ServoOn(1, comboBox_YServo.SelectedIndex);
        }

        private void getintosetting() // Setting button을 활성화 시키려면 모든 Servo를 OFF 시켜야함.
        {
            if (comboBox_XServo.SelectedIndex == 0 && comboBox_YServo.SelectedIndex == 0)
            {
                btn_motioncontrol.Enabled = true;
            }
        }
        private void btn_motioncontrol_Click(object sender, EventArgs e) // 모션 컨트롤 폼 팝업
        {
            MotionControl motioncontrol = new MotionControl();
            motioncontrol.Show();
            Console.WriteLine("Motion Control 팝업 완료");
        }

        private void updateCmdEnc() // motion controller(Paix)
        {
            if (PaixMotion.GetNmcStatus(ref NmcData) == false)
                return;

            label_XCmdVal.Text = NmcData.dCmd[0].ToString();
            label_YCmdVal.Text = NmcData.dCmd[1].ToString();

            label_XEncVal.Text = NmcData.dEnc[0].ToString();
            label_YEncVal.Text = NmcData.dEnc[1].ToString();

            panel_Emergency.BackColor = NmcData.nEmer[0] == 1 ? Color.Red : Color.Gainsboro;
            panel_XBusy.BackColor = NmcData.nBusy[0] == 1 ? Color.Red : Color.Gainsboro;
            panel_YBusy.BackColor = NmcData.nBusy[1] == 1 ? Color.Red : Color.Gainsboro;

            panel_XNear.BackColor = NmcData.nNear[0] == 1 ? Color.Red : Color.Gainsboro;
            panel_YNear.BackColor = NmcData.nNear[1] == 1 ? Color.Red : Color.Gainsboro;

            panel_XMinusLimit.BackColor = NmcData.nMLimit[0] == 1 ? Color.Red : Color.Gainsboro;
            panel_YMinusLimit.BackColor = NmcData.nMLimit[1] == 1 ? Color.Red : Color.Gainsboro;

            panel_XPlusLimit.BackColor = NmcData.nPLimit[0] == 1 ? Color.Red : Color.Gainsboro;
            panel_YPlusLimit.BackColor = NmcData.nPLimit[1] == 1 ? Color.Red : Color.Gainsboro;

            panel_XAlarm.BackColor = NmcData.nAlarm[0] == 1 ? Color.Red : Color.Gainsboro;
            panel_YAlarm.BackColor = NmcData.nAlarm[1] == 1 ? Color.Red : Color.Gainsboro;


            panel_XEncZ.BackColor = NmcData.nEncZ[0] == 1 ? Color.Red : Color.Gainsboro;
            panel_YEncZ.BackColor = NmcData.nEncZ[1] == 1 ? Color.Red : Color.Gainsboro;
        }

        private void stop(short nAxis) // 모터 정지
        {
            PaixMotion.Stop(nAxis);
        }

        private void button_XJogLeft_MouseDown(object sender, MouseEventArgs e)
        {
            PaixMotion.JogMove(0, 0);
            if (panel_XPlusLimit.BackColor == Color.Red)
            {
                stop(0);
            }
        }
        private void button_XJogLeft_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBox_JogCount.Checked)
            {
                stop(0);
            }
            else if (panel_XPlusLimit.BackColor == Color.Red)
            {
                stop(0);
            }
        }
        private void button_XJogRight_MouseDown(object sender, MouseEventArgs e)
        {
            PaixMotion.JogMove(0, 1);
            if (panel_XMinusLimit.BackColor == Color.Red)
            {
                stop(0);
            }
        }
        private void button_XJogRight_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBox_JogCount.Checked)
            {
                stop(0);
            }
            else if (panel_XMinusLimit.BackColor == Color.Red)
            {
                stop(0);
            }
        }
        private void button_YJogLeft_MouseDown(object sender, MouseEventArgs e)
        {
            PaixMotion.JogMove(1, 0);
        }
        private void button_YJogLeft_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBox_JogCount.Checked)
            {
                stop(1);
            }
        }
        private void button_YJogRight_MouseDown(object sender, MouseEventArgs e)
        {
            PaixMotion.JogMove(1, 1);
        }
        private void button_YJogRight_MouseUp(object sender, MouseEventArgs e)
        {
            if (!checkBox_JogCount.Checked)
            {
                stop(1);
            }
        }
        private void button_XStop_Click(object sender, EventArgs e)
        {
            stop(0);
        }
        private void button_YStop_Click(object sender, EventArgs e)
        {
            stop(1);
        }
        private void button_XHome_Click(object sender, EventArgs e)
        {
            PaixMotion.HomeMove(0, 1);
        }
        private void label_XCmdVal_Click(object sender, EventArgs e)
        {
            PaixMotion.SetCmd(0, 0);
        }
        private void label_YCmdVal_Click(object sender, EventArgs e)
        {
            PaixMotion.SetCmd(1, 0);
        }

        private void DDF_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            PaixMotion.ServoOn(0, 0);
            PaixMotion.ServoOn(1, 0);
            PaixMotion.Close();
            if (TdWatchSensor.IsAlive)
            {
                TdWatchSensor.Abort();
                //TdWatchSensor.Join();
            }
            if (TdTempControl.IsAlive)
            {
                TdTempControl.Abort();
                //TdWatchSensor.Join();
            }
        }

        /// <summary>
        /// DAQ Pressure Control
        /// </summary>

        private void btn_openPressure_Click(object sender, EventArgs e) // 압력(Daq) 컨트롤 Form 열기
        {
            DaqPressure daqpressure = new DaqPressure();
            daqpressure.Show();

            Console.WriteLine("DAQ Pressure 팝업 완료");
        }

        private void btn_close_Click(object sender, EventArgs e) // DDF Form 닫기
        {
            PaixMotion.ServoOn(0, 0);
            PaixMotion.ServoOn(1, 0);
            this.Close();
        }  
    }

}
