// ****************************************************************************
// Copyright Swordfish Computing Australia 2006                              **
// http://www.swordfish.com.au/                                              **
//                                                                           **
// Filename: Swordfish\WinFX\Charts\TestPage.xaml.cs                         **
// Authored by: John Stewien of Swordfish Computing                          **
// Date: April 2006                                                          **
//                                                                           **
// - Change Log -                                                            **
//*****************************************************************************

using System.Windows;

namespace Swordfish.NET.Charts
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class TestPage : Window
    {
        public TestPage()
        {
            InitializeComponent();
            ChartUtilities.AddTestLines(xyLineChart);
            xyLineChart.SubNotes = new string[] { "Масштабирование колесом мыши, перемещение при нажатии левой кнопки мыши, отмена действий - двойной клик правой кнопкой" };
            copyToClipboard.DoCopyToClipboard = ChartUtilities.CopyChartToClipboard;
        }

        public ChartControl XYLineChart
        {
            get
            {
                return xyLineChart;
            }
        }

    }
}