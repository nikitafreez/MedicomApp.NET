using MedicomApp.NET.Models;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MedicomApp.NET
{
    /// <summary>
    /// Логика взаимодействия для StatWindow.xaml
    /// </summary>
    public partial class StatWindow : Window
    {
        public StatWindow()
        {
            InitializeComponent();
        }

        DBHelper DBHelper = new DBHelper();
        public PlotModel myModel { get; private set; }
        OxyPlot.Series.PieSeries seriesCount = new OxyPlot.Series.PieSeries { StrokeThickness = 4.0, InsideLabelPosition = 0.5, AngleSpan = 360, StartAngle = 0 };
        OxyPlot.Series.PieSeries seriesMoney = new OxyPlot.Series.PieSeries { StrokeThickness = 4.0, InsideLabelPosition = 0.5, AngleSpan = 360, StartAngle = 0 };

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            this.myModel = new PlotModel
            {
                Title = "Доходы по болезням"
            };

            var treatments = DBHelper.GetTreatmentsList();
            var diseases = DBHelper.GetDiseasesList();

            float sum = 0;

            foreach (Disease disease in diseases)
            {
                foreach (Treatment treatment in treatments)
                {
                    if (treatment.DiseaseName == disease.DiseaseName)
                        sum += treatment.TreatmentSum;
                }
                seriesMoney.Slices.Add(new OxyPlot.Series.PieSlice(disease.DiseaseName, sum) { IsExploded = false });
                sum = 0;
            }

            foreach (Disease disease in diseases)
            {
                foreach (Treatment treatment in treatments)
                {
                    if (treatment.DiseaseName == disease.DiseaseName)
                        sum++;
                }
                seriesCount.Slices.Add(new OxyPlot.Series.PieSlice(disease.DiseaseName, sum) { IsExploded = false });
                sum = 0;
            }

            this.myModel.Series.Add(seriesMoney);
            plotView.Model = this.myModel;
            plotView.InvalidatePlot(true);
        }

        private void countDiseaseButton_Click(object sender, RoutedEventArgs e)
        {
            this.myModel.Series.Clear();
            this.myModel.Series.Add(seriesCount);
            this.myModel.Title = "Кол-во болезней";
            plotView.Model = this.myModel;
            plotView.InvalidatePlot(true);
        }

        private void moneyDiseaseButton_Click(object sender, RoutedEventArgs e)
        {
            this.myModel.Series.Clear();
            this.myModel.Series.Add(seriesMoney);
            this.myModel.Title = "Доходы по болезням";
            plotView.Model = this.myModel;
            plotView.InvalidatePlot(true);
        }
    }
}
