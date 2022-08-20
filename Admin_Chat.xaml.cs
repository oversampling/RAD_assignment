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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RAD_assignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Admin_Chat : Page
    {
        private Dictionary<string, string> details;
        private FirestoreDb db;
        private async void load_lecturer_student_info(string id)
        {
            progress1.IsActive = true;
            DocumentSnapshot lectRef = await db.Collection("Lecturer").Document(id).GetSnapshotAsync();
            Dictionary<string, object> lect_data = lectRef.ToDictionary();
            if (lect_data is null)
            {
                DocumentSnapshot studRef = await db.Collection("Student").Document(id).GetSnapshotAsync();
                Dictionary<string, object> stud_data = studRef.ToDictionary();
                txt_chat_partner.Text += " " + stud_data["name"];
            }
            else
            {
                txt_chat_partner.Text += " " + lect_data["name"];
            }
            progress1.IsActive = false;
        }
        public Admin_Chat()
        {
            this.InitializeComponent();
            db = FirestoreDb.Create("booknow-61e27");
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            details = (Dictionary<string, string>)e.Parameter;
            load_lecturer_student_info(details["chat_partner"]);
        }

        private void link_chat_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Admin_All_Chat), details);
        }

        private void link_add_lecturer_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void link_add_student_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void link_lecturerDetail_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void link_studentDetail_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void ChatList_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btn_load_data_Click(object sender, RoutedEventArgs e)
        {
            load_chat_message(details["adminId"]);
        }
        private async void load_chat_message(string adminId)
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
                        if (data["sender"].Equals(adminId) && data["receiver"].Equals(details["chat_partner"]))
                        {
                            // message in right
                            load_data_list(data["message"].ToString(), "sender");
                        }
                        else if (data["receiver"].Equals(adminId) && data["sender"].Equals(details["chat_partner"]))
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
        private void load_data_list(string message, string sender_receiver)
        {
            if (sender_receiver.Equals("sender"))
            {
                ListViewItem chat_message = new ListViewItem
                {
                    Width = 847,
                    Height = 69,
                };
                chat_message.Content = new TextBlock() { Text = message, TextAlignment = TextAlignment.Right, Width=820 };
                ChatList.Items.Add(chat_message);
            }
            else if (sender_receiver.Equals("receiver")){
                ListViewItem chat_message = new ListViewItem
                {
                    Width = 847,
                    Height = 69,
                };
                chat_message.Content = new TextBlock() { Text = message, TextAlignment = TextAlignment.Left, Width = 820 };
                ChatList.Items.Add(chat_message);
            }
        }

        private async void btn_submit_message_Click(object sender, RoutedEventArgs e)
        {
            progress1.IsActive = true;
            string message = txb_chat_message.Text;
            Dictionary<string, object> chat = new Dictionary<string, object>
            {
                { "sender", details["adminId"] },
                { "receiver", details["chat_partner"]},
                { "createdAt", Timestamp.GetCurrentTimestamp()},
                { "message", message }
            };
            DocumentReference docRef = await db.Collection("Chat").AddAsync(chat);
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
