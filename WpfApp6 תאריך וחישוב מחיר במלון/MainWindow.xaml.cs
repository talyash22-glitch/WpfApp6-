using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp6_תאריך_וחישוב_מחיר_במלון
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    
        public partial class MainWindow : Window
        {
            DateTime? d1;
            DateTime? d2;

            public MainWindow()
            {
                InitializeComponent();
            }

            // תאריך הגעה
            private void Calendar1_SelectedDatesChanged(
                object sender, SelectionChangedEventArgs e)
            {
                d1 = Calendar1.SelectedDate;
                UpdateBookingDetails();
            }

            // תאריך עזיבה
            private void Calendar2_SelectedDatesChanged(
                object sender, SelectionChangedEventArgs e)
            {
                d2 = Calendar2.SelectedDate;
                UpdateBookingDetails();
            }

            // חישוב מספר הלילות
            public static int HowManyNights(DateTime? d1, DateTime? d2)
            {
                if (d1 == null || d2 == null)
                    return 0;

                return (d2.Value.Date - d1.Value.Date).Days;
            }

            // החזרת תעריף הלילה לפי המלון והיום בשבוע
            private int GetNightPrice(DateTime date)
            {
                if (!(Combo3.SelectedItem is ComboBoxItem item))
                    return 0;

                string hotel = item.Content.ToString();

                bool isSaturday = date.DayOfWeek == DayOfWeek.Saturday;

                switch (hotel)
                {
                    case "Four Seasons":
                        return isSaturday ? 1200 : 500;

                    case "Hilton":
                        return isSaturday ? 1500 : 700;

                    case "Riu":
                        return isSaturday ? 1300 : 600;

                    default:
                        return 0;
                }
            }

            // חישוב המחיר הכולל
            private int CalculatePrice(DateTime? d1, DateTime? d2)
            {
                if (d1 == null || d2 == null || d2.Value <= d1.Value)
                    return 0;

                int sum = 0;

                for (DateTime d = d1.Value.Date;
                     d < d2.Value.Date;
                     d = d.AddDays(1))
                {
                    sum += GetNightPrice(d);
                }

                return sum;
            }

            // בחירת מלון
            private void Combo3_Selection(
                object sender, SelectionChangedEventArgs e)
            {
                UpdateBookingDetails();
            }

            // האירועים של שתי הרשימות האחרות
            private void Combo1_Selection(
                object sender, SelectionChangedEventArgs e)
            {
            }

            private void Combo2_Selection(
                object sender, SelectionChangedEventArgs e)
            {
            }

            // הצגת הפינוקים בהתאם לסוג החדר
            private void RoomType_Checked(
                object sender, RoutedEventArgs e)
            {
                if (TreatsText == null)
                    return;

                if (SingleRadio.IsChecked == true)
                {
                    TreatsText.Text =
                        "חדר יחיד: מיטת יחיד, מגבות ומוצרי רחצה.";
                }
                else if (DoubleRadio.IsChecked == true)
                {
                    TreatsText.Text =
                        "חדר זוגי: מיטה זוגית, מגבות, מוצרי רחצה " +
                        "וארוחת בוקר לזוג.";
                }
                else if (TripleRadio.IsChecked == true)
                {
                    TreatsText.Text =
                        "חדר טריפל: שלוש מיטות, מגבות, מוצרי רחצה " +
                        "וארוחת בוקר לשלושה.";
                }
                else if (QuadRadio.IsChecked == true)
                {
                    TreatsText.Text =
                        "חדר לארבעה: ארבע מיטות או מיטות למשפחה, " +
                        "מגבות, מוצרי רחצה וארוחת בוקר לארבעה.";
                }
            }

            // עדכון הפרטים בחלון
            private void UpdateBookingDetails()
            {
                if (d1 == null || d2 == null)
                {
                    NightsText.Text = "כמות לילות: -";
                    PriceText.Text = "מחיר: -";
                    return;
                }

                if (d2.Value.Date <= d1.Value.Date)
                {
                    NightsText.Text =
                        "תאריך העזיבה חייב להיות אחרי תאריך ההגעה";

                    PriceText.Text = "מחיר: -";
                    return;
                }

                int nights = HowManyNights(d1, d2);

                NightsText.Text = "כמות לילות: " + nights;

                if (Combo3.SelectedItem == null)
                {
                    PriceText.Text = "יש לבחור מלון";
                    return;
                }

                int price = CalculatePrice(d1, d2);

                PriceText.Text =
                    "הסכום לתשלום: " + price.ToString("N0") + " ש\"ח";
            }
        }
}



