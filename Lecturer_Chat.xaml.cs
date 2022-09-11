using Google.Cloud.Firestore;
using System;
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
using System.Net.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RAD_assignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Lecturer_Chat : Page
    {
        private FirestoreDb db;
        Dictionary<string, string> details = new Dictionary<string, string>();
        private string selected_chatPartner;
        public Lecturer_Chat()
        {
            this.InitializeComponent();
            db = FirestoreDb.Create("booknow-61e27");
        }
        private void link_chat_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Lecturer_Chat), details);
        }
        private async void load_chat_partner()
        {
            progress1.IsActive = true;
            DocumentSnapshot lecturer = await db.Collection("Lecturer").Document(details["lecturerId"]).GetSnapshotAsync();
            Dictionary<string, object> lecturer_data = lecturer.ToDictionary();
            Query query = db.Collection("Student").WhereEqualTo("university", (string)lecturer_data["university"]);
            QuerySnapshot student_chat_partner = await query.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in student_chat_partner.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> data = documentSnapshot.ToDictionary();
                    chatPartner.Items.Add(new TextBlock() { Text = data["username"].ToString(), Tag = documentSnapshot.Id.ToString() });
                }
            }
            Query query_admin = db.Collection("Admin").WhereEqualTo("university", (string)lecturer_data["university"]);
            QuerySnapshot admin_chat_partner = await query_admin.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in admin_chat_partner.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> data = documentSnapshot.ToDictionary();
                    chatPartner.Items.Add(new TextBlock() { Text = (data["username"] + " (admin)").ToString(), Tag = documentSnapshot.Id.ToString() });
                }
            }
            progress1.IsActive = false;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            details = (Dictionary<string, string>)e.Parameter;
            load_chat_partner();
        }

        private void btn_load_data_Click(object sender, RoutedEventArgs e)
        {
            load_chat_message(details["lecturerId"]);

        }

        private async void load_chat_message(string lecturerId)
        {
            if (selected_chatPartner != null)
            {
                ChatList.Items.Clear();
                progress1.IsActive = true;
                Query qref = db.Collection("Chat").OrderBy("createdAt");
                QuerySnapshot capitalQuerySnapshot = await qref.GetSnapshotAsync();
                int count = capitalQuerySnapshot.Count();
                if (count > 0)
                {
                    foreach (DocumentSnapshot documentSnapshot in capitalQuerySnapshot.Documents)
                    {
                        if (documentSnapshot.Exists)
                        {
                            Dictionary<string, object> data = documentSnapshot.ToDictionary();
                            if (data["sender"].Equals(lecturerId) && data["receiver"].Equals(selected_chatPartner))
                            {
                                // message in right
                                load_data_list(data["message"].ToString(), "sender");
                            }
                            else if (data["receiver"].Equals(lecturerId) && data["sender"].Equals(selected_chatPartner))
                            {
                                // message in left
                                load_data_list(data["message"].ToString(), "receiver");
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
                progress1.IsActive = false;
            }

        }
        private void load_data_list(string message, string sender_receiver)
        {
            if (sender_receiver.Equals("sender"))
            {
                ListViewItem chat_message = new ListViewItem
                {
                    Width = 847,
                    Height = 69,
                };
                chat_message.Content = new TextBlock() { Text = message, TextAlignment = TextAlignment.Right, Width = 820 };
                ChatList.Items.Add(chat_message);
            }
            else if (sender_receiver.Equals("receiver"))
            {
                ListViewItem chat_message = new ListViewItem
                {
                    Width = 847,
                    Height = 69,
                };
                chat_message.Content = new TextBlock() { Text = message, TextAlignment = TextAlignment.Left, Width = 820 };
                ChatList.Items.Add(chat_message);
            }
        }

        private void chatPartner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBlock chat_partner = (TextBlock)e.AddedItems[0];
            selected_chatPartner = (string)chat_partner.Tag;
        }
        private static readonly HttpClient client = new HttpClient();
        private async void btn_submit_message_Click(object sender, RoutedEventArgs e)
        {
            progress1.IsActive = true;
            string message = txb_chat_message.Text;
            Dictionary<string, object> chat = new Dictionary<string, object>
            {
                { "sender", details["lecturerId"] },
                { "receiver", selected_chatPartner},
                { "createdAt", Timestamp.GetCurrentTimestamp()},
                { "message", message }
            };
            DocumentReference docRef = await db.Collection("Chat").AddAsync(chat);
            var values = new Dictionary<string, string>
              {
                  { "receiver_email", "chan1992241@1utar.my" },
                  { "message", message },
                  { "sender_name", "lecturer1" }
              };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("http://localhost:62296/api/home", content);
            var responseString = await response.Content.ReadAsStringAsync();
            ListViewItem chat_message = new ListViewItem
            {
                Width = 847,
                Height = 69,
            };
            chat_message.Content = new TextBlock() { Text = message, TextAlignment = TextAlignment.Right, Width = 820 };
            ChatList.Items.Add(chat_message);
            txb_chat_message.Text = "";
            progress1.IsActive = false;
        }
    }
}
