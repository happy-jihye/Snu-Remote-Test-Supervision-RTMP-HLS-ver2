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
using System.Windows.Shapes;
using System.Net;
using System.Net.Http;


namespace FFmePlayer_snu
{
    /// <summary>
    /// Student_list.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Student_list : Window
    {
        /*
        * Student List 에서는 
        * 학생 정보를 불러옵니다. 학생을 추가하거나 삭제할 수 있습니다.
        */

        string lecture;
        string test;
        string date;
        string start_time;
        string end_time;
        string token;
        
        List<MyData> items = new List<MyData>();

        public Student_list(string parm_lecture, string parm_test, string parm_date, string parm_start, string parm_end, string parm_token)
        {
            InitializeComponent();

            lecture = parm_lecture;
            test = parm_test;
            date = parm_date;
            start_time = parm_start;
            end_time = parm_end;
            token = parm_token;

            student_list();
        }



        private void student_list()
        {
            // curl -X POST http://XXXX/student_list -d tablename=DLI_midterm_20210207_2200_2350 -d token=
            string list;

            string URI = "http://XXXX/student_list";
            string myParameters = "tablename=" + lecture + "_" + test + "_" + date + "_" + start_time + "_" + end_time + "&token=" + token;
            Console.WriteLine(myParameters);


            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                list = webClient.UploadString(URI, myParameters);
                Console.WriteLine("Student list : {0}", list);
            }

            if (list != "no student found")
            {

                string[] list_split = list.Split('^');

                for (int i = 0; i < list_split.Length; i++)
                {
                    string[] info = list_split[i].Split(',');

                    items.Add(new MyData { stu_id = info[0], stu_name = info[1], supervNum = info[2] });
                }

                myListView.ItemsSource = items;

            }
            else
            {

            }

        }

        class MyData
        {
            public string stu_id { get; set; }
            public string stu_name { get; set; }
            public string supervNum { get; set; }

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

        private void delete_button_click(object sender, RoutedEventArgs e)
        {
            // delete 
            // curl -X POST http://XXXX/delete_student_data -d tablename=DLI_midterm_20210207_2200_2350 -d num=2020-11111-d token=

            MyData data = ListView_GetItem(e);

            string URI = "http://XXXX/delete_student_data";
            string myParameters = "tablename=" + lecture + "_" + test + "_" + date + "_" + start_time + "_" + end_time + "&num=" + data.stu_id + "&token=" + token;
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

            items.Remove(new MyData { stu_id = data.stu_id, stu_name = data.stu_name, supervNum = data.supervNum });


        }

        private void btn_add_stu_enter(object sender, RoutedEventArgs e)
        {
            //curl -X POST http://XXXX/add_student_data -d num=2020-12345 -d name=원준 -d supervNum=1 -d lec=logicdesign -d test=midterm -d testdate=20210108 -d starttime=1400 -d endtime=1530 -d token=

            string URI = "http://XXXX/add_student_data";
            string myParameters = "num=" + stuNum_txt.Text + "&name=" + stuName_txt.Text + "&supervNum=" + supervNum_txt.Text + "&lec=" + lecture + "&test=" + test + "&testdate=" + date + "&starttime=" + start_time + "&endtime=" + end_time + "&token=" + token;
            //"ID=2020-13579&PW=qwerty1234&lec_id=logic_design";

            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = webClient.UploadString(URI, myParameters);
                Console.WriteLine(HtmlResult);
            }

        }
    }
}
