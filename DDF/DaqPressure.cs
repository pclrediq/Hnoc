using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using NationalInstruments;
using NationalInstruments.DAQmx;

namespace DDF
{
    public partial class DaqPressure : Form
    {
        private NationalInstruments.DAQmx.Task myTask;
        private Task runningTask;
        private DataTable dataTable;
        private DataColumn[] dataColumn;
        private AnalogMultiChannelReader reader;
        private AsyncCallback callback;
        private AnalogWaveform<double>[] waveform;

        private System.ComponentModel.Container component = null; //이거 뭐임?

        public DaqPressure()
        {
            InitializeComponent();

            dataTable = new DataTable();
            maximumValueNumericUpDown.Value = 0;
            minimumValueNumericUpDown.Value = 0;
            pressureunitsComboBox.SelectedIndex = 0;

            bridgeConfigurationComboBox.SelectedIndex = 0;
            excitationSourceComboBox.SelectedIndex = 0;
            excitationValueNumericUpDown.Value = 0;

            btn_Start.Enabled = false;
            btn_Stop.Enabled = false;

            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
            System.Console.WriteLine("PhysicalChannel Load Complete.");

            if (physicalChannelComboBox.Items.Count > 0)
            {
                physicalChannelComboBox.SelectedIndex = 0;
                btn_Start.Enabled = true;
            }
        }

        /*protected override void Dispose(bool disposing)
        {
           if (disposing)
           {
              if (component != null)
              {
                 component.Dispose();
              }
              if (myTask != null)
              {
                 runningTask = null;
                 myTask.Dispose();
              }
           }
              base.Dispose(disposing);
        }*/

        [STAThread]
        static void main()
        {
            Application.EnableVisualStyles();
            Application.DoEvents();
            Application.Run(new DaqPressure());
        }

        //여기까지는 문제 없을 것으로 보임.

        private void startButton_Click(object sender, System.EventArgs e)
        {
            System.Console.WriteLine("{Pressure 측정을 시작합니다.");
            Cursor.Current = Cursors.WaitCursor;
            btn_Start.Enabled = false;
            btn_Stop.Enabled = true;

            try
            {
                // Bridge Configuration 설정값 받아오기
                AIBridgeConfiguration bridgeConfiguration;
                if (bridgeConfigurationComboBox.SelectedIndex == 0)
                    bridgeConfiguration = AIBridgeConfiguration.FullBridge;
                else if (bridgeConfigurationComboBox.SelectedIndex == 1)
                    bridgeConfiguration = AIBridgeConfiguration.HalfBridge;
                else if (bridgeConfigurationComboBox.SelectedIndex == 2)
                    bridgeConfiguration = AIBridgeConfiguration.NoBridge;
                else
                    bridgeConfiguration = AIBridgeConfiguration.QuarterBridge;


                // Excitation Source 설정값 받아오기
                AIExcitationSource excitationSource;
                if (excitationSourceComboBox.SelectedIndex == 0)
                    excitationSource = AIExcitationSource.Internal;
                else if (excitationSourceComboBox.SelectedIndex == 1)
                    excitationSource = AIExcitationSource.External;
                else
                    excitationSource = AIExcitationSource.None;


                // Unit 변환 설정값 받아오기
                AIPressureUnits units;
                if (pressureunitsComboBox.SelectedIndex == 0)
                    units = AIPressureUnits.Bar;
                else if (pressureunitsComboBox.SelectedIndex == 1)
                    units = AIPressureUnits.Pascals;
                else
                    units = AIPressureUnits.PoundsPerSquareInch;

                AIBridgeElectricalUnits electricalUnits;
                if (electricalUnitsComboBox.SelectedIndex == 0)
                    electricalUnits = AIBridgeElectricalUnits.MillivoltsPerVolt;
                else
                    electricalUnits = AIBridgeElectricalUnits.VoltsPerVolt;

                AIBridgePhysicalUnits physicalUnits;
                if (physicalUnitsComboBox.SelectedIndex == 0)
                    physicalUnits = AIBridgePhysicalUnits.Pounds;
                else if (physicalUnitsComboBox.SelectedIndex == 1)
                    physicalUnits = AIBridgePhysicalUnits.KilogramForce;
                else
                    physicalUnits = AIBridgePhysicalUnits.Newtons;


                // AI Channel 만들고 설정하기
                myTask = new Task();
                AIChannel myAIChannel;

                if (sensorScalingTabControl.SelectedIndex == 0)
                {
                    myAIChannel = myTask.AIChannels.CreatePressureBridgeTwoPointLinearChannel(physicalChannelComboBox.Text, "", Convert.ToDouble(minimumValueNumericUpDown.Value),
                        Convert.ToDouble(maximumValueNumericUpDown.Value), units, bridgeConfiguration, excitationSource,
                        Convert.ToDouble(excitationValueNumericUpDown.Value), Convert.ToDouble(nomGageResNumericUpDown.Value),
                        Convert.ToDouble(firstElectricalValueNumericUpDown.Value),
                        Convert.ToDouble(secondElectricalValueNumericUpDown.Value),
                        electricalUnits,
                        Convert.ToDouble(firstPhysicalValueNumericUpDown.Value),
                        Convert.ToDouble(secondPhysicalValueNumericUpDown.Value),
                        physicalUnits);
                }
                else if (sensorScalingTabControl.SelectedIndex == 1)
                {
                    double[] electricalValues = new double[tableDataGridView.Rows.Count - 1];
                    double[] physicalValues = new double[tableDataGridView.Rows.Count - 1];
                    for (int i = 0; i < electricalValues.Length; i++)
                    {
                        electricalValues[i] = Convert.ToDouble(tableDataGridView.Rows[i].Cells[0].Value);
                        physicalValues[i] = Convert.ToDouble(tableDataGridView.Rows[i].Cells[1].Value);
                    }
                    myAIChannel = myTask.AIChannels.CreateForceBridgeTableChannel(physicalChannelComboBox.Text, "",
                        Convert.ToDouble(minimumValueNumericUpDown.Value),
                        Convert.ToDouble(maximumValueNumericUpDown.Value),
                        null, bridgeConfiguration, excitationSource,
                        Convert.ToDouble(excitationValueNumericUpDown.Value),
                        Convert.ToDouble(nomGageResNumericUpDown.Value),
                        electricalValues, electricalUnits,
                        physicalValues, physicalUnits);
                }
                else
                {
                    double[] coefficients = new double[polynomialDataGrid.Rows.Count - 1];
                    double[] forward;
                    double[] reverse;
                    for (int i = 0; i < coefficients.Length; i++)
                    {
                        coefficients[i] = Convert.ToDouble(polynomialDataGrid.Rows[i].Cells[0].Value);
                    }
                    if (electToPhysRadioButton.Checked)
                    {
                        forward = coefficients;
                        PolynomialScale scale = new PolynomialScale("scale",
                            PolynomialDirection.Forward,
                            forward, Convert.ToDouble(minimumNumericUpDown.Value),
                            Convert.ToDouble(maximumNumericUpDown.Value));
                        reverse = scale.ReverseCoefficients;
                    }
                    else
                    {
                        reverse = coefficients;
                        PolynomialScale scale = new PolynomialScale("scale",
                            PolynomialDirection.Reverse,
                            reverse, Convert.ToDouble(minimumNumericUpDown.Value),
                            Convert.ToDouble(maximumNumericUpDown.Value));
                        forward = scale.ForwardCoefficients;
                    }

                    myAIChannel = myTask.AIChannels.CreateForceBridgePolynomialChannel(physicalChannelComboBox.Text, "",
                        Convert.ToDouble(minimumValueNumericUpDown.Value),
                        Convert.ToDouble(maximumValueNumericUpDown.Value),
                        null, bridgeConfiguration, excitationSource,
                        Convert.ToDouble(excitationValueNumericUpDown.Value),
                        Convert.ToDouble(nomGageResNumericUpDown.Value),
                        forward, reverse, electricalUnits, physicalUnits);
                }
                // Timing Parameters 설정값 받아오기
                SampleQuantityMode AcquisitionMode;
                if (AcquisitionModeComboBox.SelectedIndex == 0)
                    AcquisitionMode = SampleQuantityMode.HardwareTimedSinglePoint;
                else if (AcquisitionModeComboBox.SelectedIndex == 1)
                    AcquisitionMode = SampleQuantityMode.FiniteSamples;
                else
                    AcquisitionMode = SampleQuantityMode.ContinuousSamples;

                // Sample Clock 설정하기
                myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(timingRateNumeric.Value), SampleClockActiveEdge.Rising, AcquisitionMode);

                // Task 확인하기
                myTask.Control(TaskAction.Verify);

                InitializeDataTable(ref dataTable);
                acquisitionDataGrid.DataSource = dataTable;

                StartTask();

                reader = new AnalogMultiChannelReader(myTask.Stream);
                callback = new AsyncCallback(AnalogCallBack);

                myTask.Start();

                reader.SynchronizeCallbacks = true;
                reader.BeginReadWaveform(Convert.ToInt32(timingSamplesNumeric.Value),
                    callback, myTask);

                System.Console.WriteLine("Pressure 측정 중입니다.");
            }
            catch (DaqException exception)
            {
                HandleExceptions(exception);
            }

            Cursor.Current = Cursors.Default;
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            System.Console.WriteLine("Pressure 측정을 종료합니다.");
            if (runningTask != null)
            {
                runningTask = null;
                StopTask();
                System.Console.WriteLine("Pressure 측정을 종료하였습니다.");
            }
        }

        private void StartTask()
        {
            runningTask = myTask;
            btn_Stop.Enabled = true;
            btn_Start.Enabled = false;

            physicalChannelComboBox.Enabled = false;
            maximumValueNumericUpDown.Enabled = false;
            minimumValueNumericUpDown.Enabled = false;
            excitationSourceComboBox.Enabled = false;
            excitationValueNumericUpDown.Enabled = false;
            nomGageResNumericUpDown.Enabled = false;
            timingRateNumeric.Enabled = false;
            timingSamplesNumeric.Enabled = false;
        }
        private void StopTask()
        {
            runningTask = null;
            myTask.Dispose();
            btn_Stop.Enabled = false;
            btn_Start.Enabled = true;

            physicalChannelComboBox.Enabled = true;
            maximumValueNumericUpDown.Enabled = true;
            minimumValueNumericUpDown.Enabled = true;
            excitationSourceComboBox.Enabled = true;
            excitationValueNumericUpDown.Enabled = true;
            nomGageResNumericUpDown.Enabled = true;
            timingRateNumeric.Enabled = true;
            timingSamplesNumeric.Enabled = true;
        }

        private void AnalogCallBack(IAsyncResult asyncResult)
        {
            try
            {
                if (runningTask != null && runningTask == asyncResult.AsyncState)
                {
                    // Read the available data from the channels
                    AnalogWaveform<double>[] data = reader.EndReadWaveform(asyncResult);

                    // Plot your data here
                    dataToDataTable(data, ref dataTable);

                    // Set up a new callback
                    reader.BeginMemoryOptimizedReadWaveform(Convert.ToInt32(timingSamplesNumeric.Value),
                        callback, myTask, data);
                }
            }
            catch (DaqException exception)
            {
                HandleExceptions(exception);
            }
        }

        private void dataToDataTable(AnalogWaveform<double>[] sourceArray, ref DataTable dataTable)
        {
            // Iterate over channels
            int currentLineIndex = 0;
            foreach (AnalogWaveform<double> waveform in sourceArray)
            {
                for (int sample = 0; sample < waveform.Samples.Count; ++sample)
                {
                    if (sample == 15)
                        break;

                    dataTable.Rows[sample][currentLineIndex] = waveform.Samples[sample].Value;
                }
                currentLineIndex++;
            }
        }

        private void InitializeDataTable(ref DataTable data)
        {
            int numOfLines = Convert.ToInt32(myTask.AIChannels.Count);
            data.Rows.Clear();
            data.Columns.Clear();
            dataColumn = new DataColumn[numOfLines];
            int numOfRows = 15;

            for (int currentLineIndex = 0; currentLineIndex < numOfLines; currentLineIndex++)
            {
                dataColumn[currentLineIndex] = new DataColumn();
                dataColumn[currentLineIndex].DataType = typeof(double);
                dataColumn[currentLineIndex].ColumnName = myTask.AIChannels[currentLineIndex].PhysicalName;
            }
            data.Columns.AddRange(dataColumn);

            for (int currentDataIndex = 0; currentDataIndex < numOfRows; currentDataIndex++)
            {
                object[] rowArr = new object[numOfLines];
                data.Rows.Add(rowArr);

            }
        }

        private void HandleExceptions(DaqException exception)
        {
            MessageBox.Show(exception.Message);

            runningTask = null;
            myTask.Dispose();
            btn_Start.Enabled = true;
            btn_Stop.Enabled = false;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
            Console.WriteLine("DAQ Pressure 팝업 종료");
        }
    }
}
