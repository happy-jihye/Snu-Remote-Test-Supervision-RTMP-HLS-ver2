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
using FFmePlayer_snu.Controls;
using System.Net;
using System.Net.Http;

namespace FFmePlayer_snu
{
    /// <summary>
    /// Main_Page.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Main_Page : Page
    {
        /*
        * Main Page
        ** 예정된 시험 list들을 불러옵니다. 시험을 삭제하거나 추가할 수 있습니다.
        ** Review test를 누르면 과거의 시험을 다시 볼 수 있는 페이지로 이동합니다.
        */

        string token;
        
        MainWindow mainWindow { get => Application.Current.MainWindow as MainWindow; }

        public Main_Page(string parm_token)
        {
            InitializeComponent();

            token = parm_token;
            Console.WriteLine("token {0} ", token);

            live_test_list();
            img.Source = new BitmapImage(new Uri(@"\Resources\snu_symbol.PNG", UriKind.RelativeOrAbsolute));
        }


        private void schedule_Click(object sender, RoutedEventArgs e)
        {
            Schedule_test schedule_page = new Schedule_test(token);
            mainWindow.mainFrame.Navigate(schedule_page);
        }

        private void Review_Click(object sender, RoutedEventArgs e)
        {
            search_viewer search_Viewer = new search_viewer(token);
            mainWindow.mainFrame.Navigate(search_Viewer);


        }

        

        private void live_test_list()
        {
            // curl -X POST http://XXXX/superv_endpoint_pre -d token=
            string list; 

            string URI = "http://XXXX/superv_endpoint_pre";
            string myParameters = "token=" + token;


            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                list = webClient.UploadString(URI, myParameters);
                Console.WriteLine(list);
            }

            if (list != "")
            {

                string[] list_split = list.Split('^');
                Console.WriteLine("list : {0}", list);

                List<MyData> items = new List<MyData>();
                for (int i = 0; i < list_split.Length; i++)
                {
                    string[] info = list_split[i].Split('.');
                    string date = "2021" + " / " + info[2][4] + info[2][5] + " / " + info[2][6] + info[2][7];
                    string[] start = info[3].Split('_');
                    string start_t = start[0] + " : " + start[1];
                    string[] end = info[4].Split('_');
                    string end_t = end[0] + " : " + end[1];
                    items.Add(new MyData { Lecture = info[0], Test = info[1], Date = info[2], Date_show = date, Start_time_show = start_t, Start_time = start[0] + start[1], End_time_show = end_t, End_time = end[0] + end[1] });
                }
                
                myListView.ItemsSource = items;

            }
            else
            {

            }

        }

        class MyData
        {
            public string Lecture { get; set; }
            public string Test { get; set; }
            public string Date_show { get; set; }
            public string Date { get; set; }
            public string Start_time_show { get; set; }
            public string Start_time { get; set; }
            public string End_time_show { get; set; }
            public string End_time { get; set; }

        }

        //리스트뷰에서 선택한 항목의 정보를 얻어온다.

        private static MyData ListView_GetItem(RoutedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while (!(dep is System.Windows.Controls.ListViewItem))
            {
                try
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }
                catch
                {
                   return null;
                }
            }

            ListViewItem item = (ListViewItem)dep;

            MyData content = (MyData)item.Content;
            return content;
        }

        private void list_Double_clik(object sender, RoutedEventArgs e)
        {

            MyData data = ListView_GetItem(e);

            live_tab live_Tab = new live_tab(data.Lecture, data.Test, data.Date, data.Start_time, data.End_time, token);
            mainWindow.mainFrame.Navigate(live_Tab);

        }

        private void delete_button_click(object sender, RoutedEventArgs e)
        {
            // delete 
            //curl -X POST http://XXXX/delete_exam_data -d lec_id=sf.midterm_20210112 -d token=

            MyData data = ListView_GetItem(e);

            string URI = "http://XXXX/delete_exam_data";

            string myParameters = "tablename=" + data.Lecture + "_" + data.Test + "_" + data.Date + "_" + data.Start_time + "_" + data.End_time + "&token=" + token;
            Console.WriteLine(myParameters);

            string HtmlResult;

            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                HtmlResult = webClient.UploadString(URI, myParameters);
                Console.WriteLine("======");
                Console.WriteLine(HtmlResult);
                Console.WriteLine("======");
            }

            Main_Page main_Page = new Main_Page(token);
            mainWindow.mainFrame.Navigate(main_Page);

        }

        private void student_button_click(object sender, RoutedEventArgs e)
        {
            MyData data = ListView_GetItem(e);

            Student_list student_List = new Student_list(data.Lecture, data.Test, data.Date, data.Start_time, data.End_time, token);

            // 새로운 창을 가운데에 위치할 수 있도록 위치 지정
            student_List.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            student_List.ShowDialog();
        }

    }
}
