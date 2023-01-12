using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace DDF
{
    public partial class MotionControl : Form
    {
        DDF_Form ddF;
        PaixMotion PaixMotion;

        // 모션 컨트롤 조작 변수 선언
        public static double xRatioChange;
        public static double yRatioChange;
        public static double dxstart;
        public static double dxacc;
        public static double dxmax;
        public static double dxdec;
        public static double dystart;
        public static double dyacc;
        public static double dymax;
        public static double dydec;
        public static int xservo;
        public static int yservo;
        public static double xalarm;
        public static double yalarm;
        public static double emer;

        public MotionControl()
        {
            InitializeComponent();

            num_XRatio.Value = Convert.ToDecimal(0.0000179);
            num_XStartSpeed.Value = Convert.ToDecimal(1);
            num_XAcc.Value = Convert.ToDecimal(6.728);
            num_XDec.Value = Convert.ToDecimal(6.728);
            num_XMax.Value = Convert.ToDecimal(10.417);

            num_YRatio.Value = Convert.ToDecimal(0.0000358);
            num_YStartSpeed.Value = Convert.ToDecimal(1);
            num_YAcc.Value = Convert.ToDecimal(10.445);
            num_YDec.Value = Convert.ToDecimal(10.445);
            num_YMax.Value = Convert.ToDecimal(16.667);
        }

        public void button_SetUp_Click(object sender, EventArgs e) // motor 모션 조작 변수 변경
        {
            Cursor.Current = Cursors.WaitCursor;

            xRatioChange = decimal.ToDouble(num_XRatio.Value);
            dxstart = decimal.ToDouble(num_XStartSpeed.Value);
            dxacc = decimal.ToDouble(num_XAcc.Value);
            dxmax = decimal.ToDouble(num_XMax.Value);
            dxdec = decimal.ToDouble(num_XDec.Value);
            xalarm = Convert.ToDouble(comboBox_XAlarm.SelectedIndex);

            yRatioChange = decimal.ToDouble(num_YRatio.Value);
            dystart = decimal.ToDouble(num_YStartSpeed.Value);
            dyacc = decimal.ToDouble(num_YAcc.Value);
            dymax = decimal.ToDouble(num_YMax.Value);
            dydec = decimal.ToDouble(num_YDec.Value);
            yalarm = Convert.ToDouble(comboBox_YAlarm.SelectedIndex);

            emer = Convert.ToDouble(comboBox_Emergency.SelectedIndex);

            //x축
            PaixMotion.SetUnitPulse(0, xRatioChange);
            PaixMotion.SetSpeedPPS(0, dxstart, dxacc, dxdec, dxmax);
            PaixMotion.SetHomeSpeed(0, dxmax, dxmax - dxdec, dxmax - dxdec * 2);
            PaixMotion.SetAlarmLogic(0, xalarm == 0 ? (short)0 : (short)1);

            //y축
            PaixMotion.SetUnitPulse(1, yRatioChange);
            PaixMotion.SetSpeedPPS(1, dystart, dyacc, dydec, dymax);
            PaixMotion.SetHomeSpeed(1, dymax, dymax - dydec, dymax - dydec * 2);
            PaixMotion.SetAlarmLogic(1, yalarm == 0 ? (short)0 : (short)1);

            PaixMotion.SetEmerLogic(emer == 0 ? (short)0 : (short)1);

            Console.WriteLine("변수값 수정 완료 / XAcc : " + dxacc + ", XDec : " + dxdec);
            Cursor.Current = Cursors.Default;

            MessageBox.Show("SetUp Complete!", "Set Up");
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            num_XRatio.Value = Convert.ToDecimal(0.0000179);
            num_XStartSpeed.Value = Convert.ToDecimal(1);
            num_XAcc.Value = Convert.ToDecimal(6.728);
            num_XDec.Value = Convert.ToDecimal(6.728);
            num_XMax.Value = Convert.ToDecimal(10.417);

            num_YRatio.Value = Convert.ToDecimal(0.0000358);
            num_YStartSpeed.Value = Convert.ToDecimal(1);
            num_YAcc.Value = Convert.ToDecimal(10.445);
            num_YDec.Value = Convert.ToDecimal(10.445);
            num_YMax.Value = Convert.ToDecimal(16.667);
        }

        private void btn_close_Click(object sender, EventArgs e) // 모션컨트롤 폼 팝업 종료
        {
            this.Close();
            Console.WriteLine("Motion Control 팝업 종료");
        }
    }
}
