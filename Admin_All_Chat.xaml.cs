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
using Windows.UI.Xaml.Media;
using Google.Cloud.Firestore;
using System.Collections;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RAD_assignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Admin_All_Chat : Page
    {
        private Dictionary<string, string> details;
        private FirestoreDb db;
        private ArrayList chatParticipants;
        private ArrayList remove_duplicate_item(ArrayList duplicatedArray)
        {
            var uniqueArray = new ArrayList();
            for (int i = 0; i < duplicatedArray.Count; i++)
            {
                if (uniqueArray.Contains(duplicatedArray[i]) == false)
                {
                    uniqueArray.Add(duplicatedArray[i]);
                }
            }
            return uniqueArray;
        }
        async private void load_chat_user(string adminId)
        {
            progress1.IsActive = true;
            var chatWithIDs = new ArrayList();
            Query qref = db.Collection("Chat");
            QuerySnapshot capitalQuerySnapshot = await qref.GetSnapshotAsync();
            int count = capitalQuerySnapshot.Count();
            if (count > 0)
            {
                foreach (DocumentSnapshot documentSnapshot in capitalQuerySnapshot.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        string documentID = documentSnapshot.Id;
                        Dictionary<string, object> data = documentSnapshot.ToDictionary();
                        if (data["sender"].Equals(adminId))
                        {
                            chatWithIDs.Add(data["receiver"]);
                        }
                        else if (data["receiver"].Equals(adminId))
                        {
                            chatWithIDs.Add(data["sender"]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            chatParticipants = remove_duplicate_item(chatWithIDs);
            load_data_list(remove_duplicate_item(chatWithIDs));
        }

        public async void load_data_list(ArrayList chatParticipants_parameter)
        {
            //DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            //Dictionary<string, object> data = snapshot.ToDictionary();
            foreach (string id in chatParticipants_parameter)
            {
                DocumentSnapshot lectRef = await db.Collection("Lecturer").Document(id.ToString()).GetSnapshotAsync();
                Dictionary<string, object> lect_data = lectRef.ToDictionary();
                if (lect_data is null)
                {
                    DocumentSnapshot studRef = await db.Collection("Student").Document(id.ToString()).GetSnapshotAsync();
                    Dictionary<string, object> stud_data = studRef.ToDictionary();
                    ListViewItem student_item = new ListViewItem
                    {
                        Width = 825,
                        Height = 69,
                    };
                    student_item.Content = new TextBlock() { Text = (string)stud_data["name"], Tag = studRef.Id.ToString()};
                    ChatList.Items.Add(student_item);
                }
                else
                {
                    ListViewItem newItem = new ListViewItem
                    {
                        Width = 825,
                        Height = 69,
                    };
                    newItem.Tag = lectRef.Id.ToString();
                    newItem.Content = new TextBlock() { Text = (string)lect_data["name"], Tag= lectRef.Id.ToString()
                    };
                    ChatList.Items.Add(newItem);
                }
            }
            progress1.IsActive = false;

        }
        public Admin_All_Chat()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            details = (Dictionary<string, string>)e.Parameter;
            test.Text = details["adminId"];
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
            TextBlock clickedItem = (TextBlock) e.ClickedItem;
            details.Remove("chat_partner");
            details.Add("chat_partner", (string)clickedItem.Tag);
            Frame.Navigate(typeof(Admin_Chat), details);
        }

        private void btn_load_data_Click(object sender, RoutedEventArgs e)
        {
            ChatList.Items.Clear();
            db = FirestoreDb.Create("booknow-61e27");
            load_chat_user(details["adminId"]);
        }
    }
}
