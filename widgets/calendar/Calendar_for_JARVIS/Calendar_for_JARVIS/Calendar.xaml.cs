﻿using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

using System.IO;
using System.Threading;

namespace Calendar_for_JARVIS
{
    /// <summary>
    /// This class is for controling calendar widget.
    /// This class shows widget view and get schedules from Google Calendar.
    /// This class starts at initial time.
    /// 
    /// @version 2015-11-18
    /// @author NULLEDGE
    /// </summary>
    public partial class calendar_for_jarvis : UserControl
    {
        static public Events events;
        private int NUMBER_OF_GRID_ROW = 6;
        private int NUMBER_OF_GRID_COL = 7;

        #region Control type of highlight mark
        enum Highlight { TODAY, SCHEDULE }
        private Style HIGHLIGHT_TODAY, HIGHLIGHT_SCHEDULE;
        private Style highlight_style(Highlight type)
        {
            switch (type)
            {
                case Highlight.TODAY:
                    return HIGHLIGHT_TODAY;
                case Highlight.SCHEDULE:
                    return HIGHLIGHT_SCHEDULE;
                default:
                    return null;
            }
        }
        #endregion

        public calendar_for_jarvis()
        {
            InitializeComponent();

            HIGHLIGHT_TODAY = Resources["HighlightToday"] as Style;
            HIGHLIGHT_SCHEDULE = Resources["HighlightSchedule"] as Style;

            reset();
            refresh();
        }

        /// <summary>
        /// Reset content of calendar.
        /// </summary>
        private void reset()
        {
            Year.Text = "9999년";
            Date.Text = "99월 99일";
            for (int row = 0; row < NUMBER_OF_GRID_ROW; row++)
            {
                for (int col = 0; col < NUMBER_OF_GRID_COL; col++)
                {
                    TextBlock text = get<TextBlock>(row, col);
                    text.Text = "99";
                    text.Style = Resources["ThisMonth"] as Style;

                    if (is_highlighted(row, col, Highlight.TODAY)) unhighlight(row, col, Highlight.TODAY);
                    if (is_highlighted(row, col, Highlight.SCHEDULE)) unhighlight(row, col, Highlight.SCHEDULE);
                }
            }
        }

        /// <summary>
        /// Check if row and column is out of Grid_Calendar.
        /// </summary>
        /// <param name="row">Row of Grid_Calendar.</param>
        /// <param name="col">Column of Grid_Calendar.</param>
        /// <returns>True if row and column is out of Grid_Calendar.</returns>
        private bool is_out_of_grid(int row, int col)
        {
            return (row >= NUMBER_OF_GRID_ROW
                || row < 0
                || col < 0
                || col >= NUMBER_OF_GRID_COL);
        }

        #region Methods for get UI elements in Grid. Access using row and column.
        /// <summary>
        /// Get a UIElement of typename T in Grid_Calendar, specified with row and column.
        /// </summary>
        /// <typeparam name="T">typename of class inherit UIElement.</typeparam>
        /// <param name="row">Specified row of Grid_Calendar.</param>
        /// <param name="col">Specified column of Grid_Calendar.</param>
        /// <returns>UIElement</returns>
        private T get<T>(int row, int col) where T : UIElement
        {
            if (is_out_of_grid(row, col)) return default(T);
            row += 2;
            var elements = Grid_Calendar.Children.Cast<UIElement>().Where
                (e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);
            foreach (var element in elements)
                if (element is T)
                    return element as T;
            return default(T);
        }
        /// <summary>
        /// Get all UIElements of typename T in Grid_Calendar, specified with row and column.
        /// </summary>
        /// <typeparam name="T">typename of class inherit UIElement.</typeparam>
        /// <param name="row">Specified row of Grid_Calendar.</param>
        /// <param name="col">Specified column of Grid_Calendar.</param>
        /// <returns>List of UIElemenets.</returns>
        private IEnumerable<T> get_all<T>(int row, int col) where T : UIElement
        {
            if (is_out_of_grid(row, col)) return null;
            row += 2;
            var elements = Grid_Calendar.Children.Cast<UIElement>().Where
                (e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);
            List<T> _return = new List<T>();
            foreach (var element in elements)
                if (element is T)
                    _return.Add(element as T);
            return _return;
        }
        #endregion

        #region Methods for highlight mark.
        /// <summary>
        /// Check if cell is highlighted.
        /// </summary>
        /// <param name="row">Row of cell.</param>
        /// <param name="col">Column of cell.</param>
        /// <param name="highlight">Type of highlight.</param>ed
        /// <returns>True if cell is highlighted or not.</returns>
        private bool is_highlighted(int row, int col, Highlight highlight)
        {
            if (is_out_of_grid(row, col)) return false;
            IEnumerable<Ellipse> ellipses = get_all<Ellipse>(row, col);
            foreach (Ellipse ellipse in ellipses)
                if (ellipse.Style.Equals(highlight_style(highlight))) return true;
            return false;
        }
        /// <summary>
        /// Delete highlight of cell.
        /// </summary>
        /// <param name="row">Row of cell</param>
        /// <param name="col">Column of cell</param>
        /// <param name="highlight">Type of highlight</param>
        private void unhighlight(int row, int col, Highlight highlight)
        {
            if (is_out_of_grid(row, col)) return;
            IEnumerable<Ellipse> ellipses = get_all<Ellipse>(row, col);
            foreach (Ellipse ellipse in ellipses)
                if (ellipse.Style.Equals(highlight_style(highlight)))
                {
                    Grid_Calendar.Children.Remove(ellipse as UIElement);
                    return;
                }
        }
        /// <summary>
        /// Mark highlight on cell.
        /// </summary>
        /// <param name="row">Row of cell</param>
        /// <param name="col">Column of cell</param>
        /// <param name="highlight">Type of highlight</param>
        private void highlight(int row, int col, Highlight highlight)
        {
            if (is_out_of_grid(row, col)) return;
            Ellipse ellipse = new Ellipse { Style = highlight_style(highlight) };
            Grid.SetRow(ellipse, row + 2);
            Grid.SetColumn(ellipse, col);
            Grid_Calendar.Children.Add(ellipse);
        }
        #endregion

        /// <summary>
        /// Refresh the content of calendar.
        /// </summary>
        private void refresh()
        {
            set_date();
            set_days();
            set_schedule_highlight();
        }

        #region Set date and days.
        /// <summary>
        /// Set date on the top as today.
        /// </summary>
        private void set_date()
        {
            DateTime today = DateTime.Today;
            Year.Text = today.ToString("yyyy년");
            Date.Text = today.ToString("MM월 dd일");
        }
        /// <summary>
        /// Set days on calendar.
        /// </summary>
        private void set_days()
        {
            DateTime today = DateTime.Today;
            int days = DateTime.DaysInMonth(today.Year, today.Month);
            int start_day_of_month = (int)today.AddDays(-today.Day + 1).DayOfWeek;
            int days_in_month = DateTime.DaysInMonth(today.Year, today.Month);

            int day = 1;

            Style fontstyle = Resources["ThisMonth"] as Style;

            for (int row = 0; row < 6; row++)
            {
                for (int col = (row == 0 ? start_day_of_month : 0); col < 7; col++)
                {
                    TextBlock text = get<TextBlock>(row, col);
                    text.Style = fontstyle;
                    text.Text = day.ToString();

                    if (day == DateTime.Today.Day 
                        && fontstyle.Equals (Resources["ThisMonth"] as Style) == true)
                        highlight(row, col, Highlight.TODAY);

                    day++;
                    if (day > days_in_month)
                    {
                        day = 1;
                        fontstyle = Resources["NotThisMonth"] as Style;
                    }
                }
            }

            {
                DateTime day_in_last_month = DateTime.Today.AddDays(-DateTime.Today.Day);
                day = DateTime.DaysInMonth(day_in_last_month.Year, day_in_last_month.Month);
            }
            for (int row = 0, col = start_day_of_month - 1; col >= 0; col--)
            {
                TextBlock text = get<TextBlock>(row, col);
                text.Style = fontstyle;
                text.Text = day.ToString();

                day --;
            }
        }
        #endregion

        #region Get and highlight schedules from Google Calendar
        private string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        private string ApplicationName = "Google Calendar API .NET Quickstart";
        /// <summary>
        /// Highlight on calendar if there is any schedule on that day.
        /// </summary>
        private void set_schedule_highlight()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = System.IO.Path.Combine(credPath, ".credentials/calendar-dotnet-quickstart");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            DateTime today = DateTime.Today;
            int days = DateTime.DaysInMonth(today.Year, today.Month);
            int start_day_of_month = (int)today.AddDays(-today.Day + 1).DayOfWeek;
            int days_in_month = DateTime.DaysInMonth(today.Year, today.Month);

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = today.AddDays(-today.Day + 1);
            request.TimeMax = today.AddDays(-today.Day + 1 + days_in_month);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            events = request.Execute();
            Console.WriteLine("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {

                    try
                    {
                        int start_date = (eventItem.Start.DateTime == null ? DateTime.Parse(eventItem.Start.Date).Day : eventItem.Start.DateTime.Value.Day);
                        int end_date = (eventItem.End.DateTime == null ? DateTime.Parse(eventItem.End.Date).Day : eventItem.End.DateTime.Value.Day);

                        do
                        {
                            int row = (start_date + start_day_of_month - 1) / 7,
                                col = (int)DateTime.Today.AddDays(-DateTime.Today.Day + start_date).DayOfWeek;
                            highlight(row, col, Highlight.SCHEDULE);
                            start_date++;
                        }
                        while (start_date < end_date);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
            else
            {
                Console.WriteLine("No upcoming events found.");
            }
        }
        #endregion
    }
}
