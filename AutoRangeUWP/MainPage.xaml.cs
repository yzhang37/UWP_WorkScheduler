using AutoRange.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AutoRangeUWP.Models;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace AutoRangeUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public partial class MainPage : Page
    {
        private ObservableCollection<UserData> myUserData;
        private int selectedIndex;
        private ObservableCollection<DisplayDataClass> myDisplayData;

        public MainPage()
        {
            myUserData = new ObservableCollection<UserData>();
            myDisplayData = new ObservableCollection<DisplayDataClass>();
            selectedIndex = -1;
            InitializeComponent();
        }

        private void cmdAddNewUser_Click(object sender, RoutedEventArgs e)
        {
            myUserData.Add(new UserData(tbxAddNewUser.Text));
            tbxAddNewUser.Text = "";
            tbxAddNewUser.Focus(FocusState.Programmatic);
            cmdAddNewUser.IsEnabled = false;
            lstUserSelect.SelectedIndex = lstUserSelect.Items.Count - 1;
            clearResult();
        }

        private void cmdClearAllUser_Click(object sender, RoutedEventArgs e)
        {
            myUserData.Clear();
            cmdAddNewUser.IsEnabled = false;
            selectedIndex = -1;
            tbxAddNewUser.Focus(FocusState.Programmatic);
            clearResult();
        }

        private void tbxAddNewUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxAddNewUser.Text.Length > 0)
                cmdAddNewUser.IsEnabled = true;
            else
                cmdAddNewUser.IsEnabled = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cmdAddNewUser.IsEnabled = false;
            clearResult();
            enableChecks(false);
        }

        private void tbxAddNewUser_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && cmdAddNewUser.IsEnabled == true)
                cmdAddNewUser_Click(null, null);
        }

        private void enableChecks(bool arg)
        {
            if (arg == false || selectedIndex < 0)
            {
                day1.IsEnabled = day2.IsEnabled = day3.IsEnabled =
                    day4.IsEnabled = day5.IsEnabled = false;
                day1.IsChecked = day2.IsChecked = day3.IsChecked =
                    day4.IsChecked = day5.IsChecked = false;
                cmdExecute.IsEnabled = false;
            }
            else
            {
                day1.IsEnabled = day2.IsEnabled = day3.IsEnabled =
                day4.IsEnabled = day5.IsEnabled = true;
                day1.IsChecked = myUserData[selectedIndex].AvailableWeekday.Monday;
                day2.IsChecked = myUserData[selectedIndex].AvailableWeekday.Tuesday;
                day3.IsChecked = myUserData[selectedIndex].AvailableWeekday.Wednesday;
                day4.IsChecked = myUserData[selectedIndex].AvailableWeekday.Thursday;
                day5.IsChecked = myUserData[selectedIndex].AvailableWeekday.Friday;
                cmdExecute.IsEnabled = true;
            }
        }

        private void lstUserSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedIndex = lstUserSelect.SelectedIndex;
            enableChecks(true);
        }

        private void day1_Checked(object sender, RoutedEventArgs e)
        {
            myUserData[selectedIndex].AvailableWeekday.Monday = (bool)day1.IsChecked;
        }

        private void day2_Checked(object sender, RoutedEventArgs e)
        {
            myUserData[selectedIndex].AvailableWeekday.Tuesday = (bool)day2.IsChecked;
        }

        private void day3_Checked(object sender, RoutedEventArgs e)
        {
            myUserData[selectedIndex].AvailableWeekday.Wednesday = (bool)day3.IsChecked;
        }

        private void day4_Checked(object sender, RoutedEventArgs e)
        {
            myUserData[selectedIndex].AvailableWeekday.Thursday = (bool)day4.IsChecked;
        }

        private void day5_Checked(object sender, RoutedEventArgs e)
        {
            myUserData[selectedIndex].AvailableWeekday.Friday = (bool)day5.IsChecked;
        }

        private void clearResult()
        {
            lstVwResult.Visibility = Visibility.Collapsed;
            tblNoResult.Visibility = Visibility.Collapsed;
        }

        private void cmdExecute_Click(object sender, RoutedEventArgs e)
        {
            clearResult();
            int a, b, c;
            a = myUserData.Count;
            b = (int)sliDays.Value;
            if (tgsSingleDoubleDiff.IsOn)
                b *= 2;
            c = 5 * b;
            Problem myProblem = new Problem(a, c);
            
            List<string> Elements = new List<string>();
            List<string> week = new List<string>();
            week.Add("星期一");
            week.Add("星期二");
            week.Add("星期三");
            week.Add("星期四");
            week.Add("星期五");
            if (tgsSingleDoubleDiff.IsOn)
            {
                for (int i = 0; i < 5; ++i)
                {
                    for (int j = 0; j < (int)sliDays.Value; ++j)
                    {
                        Elements.Add(String.Format("{0}{1}", week[i], "(单周)"));
                    }
                    for (int j = 0; j < (int)sliDays.Value; ++j)
                    {
                        Elements.Add(String.Format("{0}{1}", week[i], "(双周)"));
                    }
                }
            }
            else
            {
                for (int i = 0; i < 5; ++i)
                {
                    for (int j = 0; j < (int)sliDays.Value; ++j)
                        Elements.Add(week[i]);
                }
            }
            for (int i = 0; i < a; ++i)
            {
                int j;
                if (myUserData[i].AvailableWeekday.Monday)
                {
                    for (j = 0; j < b; ++j)
                        myProblem.NewRelative(i, j);
                }
                if (myUserData[i].AvailableWeekday.Tuesday)
                {
                    for (j = b; j < 2 * b; ++j)
                        myProblem.NewRelative(i, j);
                }
                if (myUserData[i].AvailableWeekday.Wednesday)
                {
                    for (j = 2 * b; j < 3 * b; ++j)
                        myProblem.NewRelative(i, j);
                }
                if (myUserData[i].AvailableWeekday.Thursday)
                {
                    for (j = 3 * b; j < 4 * b; ++j)
                        myProblem.NewRelative(i, j);
                }
                if (myUserData[i].AvailableWeekday.Friday)
                {
                    for (j = 4 * b; j < 5 * b; ++j)
                        myProblem.NewRelative(i, j);
                }
            }
            List<int> answer;
            try
            {
                answer = myProblem.Solve();
                myDisplayData.Clear();
                for (int i = 0; i< a; ++i)
                {
                    myDisplayData.Add(new DisplayDataClass(i + 1, myUserData[i].UserName, Elements[answer[i]]));
                }
                lstVwResult.Visibility = Visibility.Visible;
            }
            catch (ArgumentException Err)
            {
                tblNoResult.Text = "没有找到可用的结果。";
                tblNoResult.Visibility = Visibility.Visible;
            }
            catch (Exception Err)
            {
                tblNoResult.Text = String.Format("出现异常错误：%#x000A;{0}", Err.Message);
                tblNoResult.Visibility = Visibility.Visible;
            }
        }
    }

}
