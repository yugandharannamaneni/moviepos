using BoxOffice.Model;
using BoxOffice.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace BoxOfficeUI.Util
{
    public class Helper
    {
        private static string[] alphabetArray = { string.Empty, "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public static IEnumerable<string> alphaList = alphabetArray.Cast<string>();

        public static string IntToAA(int value)
        {
            while (Helper.alphaList.Count() - 1 < value)
            {
                Helper.IncreaseList();
            }

            return Helper.alphaList.ElementAt(value);
        }

        private static void IncreaseList()
        {
            Helper.alphaList = Helper.alphabetArray.Take(1).Union(
                Helper.alphaList.SelectMany(currentLetter =>
                   Helper.alphabetArray.Skip(1).Select(innerLetter => currentLetter + innerLetter)
                )
            );
        }

        public static void NumericTextbox(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        public static bool TimeTextbox(string text)
        {
            return Regex.IsMatch(text, @"[0-9]+(?:-[0-9]+)?(,[0-9]+(?:-[0-9]+)?)*");
        }

        public static void DecimalTextbox(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, @"^[0-9]* (\.[0-9]{1,4})?$");
        }

        public static void LoadDropDownSource(Selector objSelector, IEnumerable source, string displayMember, string selectedValue, bool selectDefaultValue = true)
        {
            try
            {
                int selectedIndex = objSelector.SelectedIndex;
                objSelector.ItemsSource = source;

                objSelector.DisplayMemberPath = !string.IsNullOrEmpty(displayMember) ? displayMember : objSelector.DisplayMemberPath;
                objSelector.SelectedValuePath = !string.IsNullOrEmpty(selectedValue) ? selectedValue : objSelector.SelectedValuePath;

                objSelector.SelectedIndex = selectDefaultValue ? 0 : selectedIndex;

                if (selectedIndex == -1)
                    objSelector.SelectedItem = new object();
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        public static string ReturnColor(string color)
        {
            switch (color)
            {
                case "ACTIVE":
                    return "#DCDCDC";
                case "INACTIVE":
                    return "#FFFFFF";
                case "RESERVED":
                    return "#FF6347";
                case "HELD":
                    return "#FFA500";
                default:
                    return color;
            }
        }

        private static List<Seat> AllScreenSeats = new List<Seat>();

        public static IEnumerable<Seat> GetScreenSeats(int screenId)
        {
            try
            {
                if (Helper.AllScreenSeats == null || Helper.AllScreenSeats.Count() == 0 || AllScreenSeats.Where(w => w.Screen_Id == screenId).Count() == 0)
                {
                    AllScreenSeats = new SeatLayoutConfig().GetScreeSeats(0, 0, 0).ToList();
                }
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }

            return AllScreenSeats.Where(w => w.Screen_Id == screenId);
        }

        public static string Theater { get; set; }

        public static string UserName { get; set; }
    }

    public class ControlItems
    {
        public string Id { get; set; }

        public string Value { get; set; }

        public bool IsSelected { get; set; }
    }
}