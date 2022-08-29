using Google.Cloud.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RAD_assignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Student_Search_Schedules : Page
    {
        FirestoreDb db;
        Dictionary<string, string> details = new Dictionary<string, string>();
        private string selected_chatPartner;
        public Student_Search_Schedules()
        {
            this.InitializeComponent();
            db = FirestoreDb.Create("booknow-61e27");
        }
        private void link_chat_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Student_Chat), details);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            details = (Dictionary<string, string>)e.Parameter;
            load_lecturer_name();
        }

        private async void load_lecturer_name()
        {
            progress1.IsActive = true;
            DocumentSnapshot student = await db.Collection("Student").Document(details["studentId"]).GetSnapshotAsync();
            Dictionary<string, object> student_data = student.ToDictionary();
            Query query = db.Collection("Lecturer").WhereEqualTo("university", (string)student_data["university"]);
            QuerySnapshot lecture_chat_partner = await query.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in lecture_chat_partner.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> data = documentSnapshot.ToDictionary();
                    chatPartner.Items.Add(new TextBlock() { Text = data["username"].ToString(), Tag = documentSnapshot.Id.ToString() });
                }
            }
            progress1.IsActive = false;
        }
        private void lecturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBlock chat_partner = (TextBlock)e.AddedItems[0];
            selected_chatPartner = (string)chat_partner.Tag;
        }
        private void btn_load_data_Click(object sender, RoutedEventArgs e)
        {
            load_schedule(details["studentId"]);
        }

        private async void load_schedule(string studentId)
        {
            if (selected_chatPartner != null)
            {
                ScheduleList.Items.Clear();
                progress1.IsActive = true;
                DocumentSnapshot lecturer = await db.Collection("Lecturer").Document(selected_chatPartner).GetSnapshotAsync();
                Dictionary<string, object> lecturer_data = lecturer.ToDictionary();
                if (lecturer != null)
                {
                    List<Object> schedules;
                    try
                    {
                        schedules = (List<Object>)lecturer_data["schedules"];
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                        schedules = null;
                    }
                    if (schedules != null)
                    {
                        foreach (String schedule in schedules)
                        {
                            ScheduleList.Items.Add(new TextBlock() { Text = schedule });
                        }
                    }
                    else
                    {
                        ScheduleList.Items.Add(new TextBlock() { Text = "no schedule found", TextAlignment = TextAlignment.Center, Width = 820 });
                    }
                }
                progress1.IsActive = false;
            }
        }

        private void search_schedule_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Student_Search_Schedules), details);
        }
    }
}
