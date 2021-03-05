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
using System.Windows.Threading;

namespace FFmePlayer_snu.Controls
{
    /// <summary>
    /// live_viewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class live_viewer
    {

        /* 
        * Live Viewer는 여러개의 RTMP 주소를 받으면,
        * Live player를 통해 여러개의 영상을 띄우는 부분입니다.
        * 
        * 주소의 끝부분에는 초기접속 / 재접속 여부를 뜻하는 정보가(0, 1) 함께 들어오는데, 
        * 이를 통하여 처음 접속하면 영상을 viewer에 추가하고,
        * 재접속을 하면 영상이 새로고침되도록 구현하였습니다.
        */


        List<string> uri_list = new List<string>();
        List<live_player> streaming_player_list = new List<live_player>();
        List<Button> Buttons = new List<Button>();

        int num;


        public live_viewer()
        {
            InitializeComponent();

        }

        public void live_viewer_loaded(string m_uri_string)
        {
            string[] uri_list = m_uri_string.Split('^');
            num = uri_list.Length;

            //0이 컴퓨터, 1이 핸드폰
            bool[,] uri_first = new bool[num, 2];
            bool[] first = new bool[num];
            string[,] rtmp_uri = new string[num, 2];

            for (int i = 0; i < num; i++)
            {
                string[] uri = uri_list[i].Split(',');

                first[i] = true;
                uri_first[i, 0] = true;
                uri_first[i, 1] = true;

                rtmp_uri[i, 0] = uri[0];
                rtmp_uri[i, 1] = uri[1];
            }

            for (int i = 0; i < num; i++)
            {
                if (rtmp_uri[i, 0] != "null" && rtmp_uri[i, 1] != "null")
                {
                    if(first[i] == true)
                    {
                        first[i] = false;

                        live_player streaming_player = new live_player();

                        streaming_player.m_uri = rtmp_uri[i, 1];
                        streaming_player.media_play();

                        streaming_player.m_uri2 = rtmp_uri[i, 0];
                        streaming_player.media_play2();

                        myLivePlayers.Items.Add(streaming_player);
                    }
                }
     

                //if (rtmp_uri[i, 0] != "null" && uri_first[i, 0] == true)
                //{
                //    uri_first[i, 0] = false;
                //    // 초기 접속
                //    if (first[i] == true)
                //    {
                //        first[i] = false;

                //        live_player streaming_player = new live_player();

                //        streaming_player.m_uri2 = rtmp_uri[i, 0];
                //        streaming_player.media_play2();

                //        myLivePlayers.Items.Add(streaming_player);
                //        streaming_player_list.Add(streaming_player);
                //    }
                //    //재접속
                //    else
                //    {
                //        live_player streaming_player = new live_player();

                //        if (streaming_player_list[i].m_uri != "null")
                //        {
                //            streaming_player.m_uri = rtmp_uri[i, 1];
                //            streaming_player.media_play();
                //        }

                //        streaming_player.m_uri2 = rtmp_uri[i, 0];
                //        streaming_player.media_play2();

                //        myLivePlayers.Items.Remove(streaming_player_list[i]);
                //        streaming_player_list.Remove(streaming_player_list[i]);

                //        myLivePlayers.Items.Add(streaming_player);
                //        streaming_player_list.Add(streaming_player);
                //    }

                //}
                //else if (rtmp_uri[i, 1] != "null" && uri_first[i, 1] == true)
                //{
                //    uri_first[i, 1] = false;
                //    // 초기 접속
                //    if (first[i] == true)
                //    {
                //        first[i] = false;

                //        live_player streaming_player = new live_player();

                //        streaming_player.m_uri = rtmp_uri[i, 1];
                //        streaming_player.media_play();

                //        myLivePlayers.Items.Add(streaming_player);
                //        streaming_player_list.Add(streaming_player);
                //    }
                //    //재접속
                //    else
                //    {
                //        live_player streaming_player = new live_player();

                //        if (streaming_player_list[i].m_uri2 != "null")
                //        {
                //            streaming_player.m_uri2 = rtmp_uri[i, 0];
                //            streaming_player.media_play2();
                //        }
                //        streaming_player.m_uri = rtmp_uri[i, 1];
                //        streaming_player.media_play();


                //        myLivePlayers.Items.Remove(streaming_player_list[i]);
                //        streaming_player_list.Remove(streaming_player_list[i]);

                //        myLivePlayers.Items.Add(streaming_player);
                //        streaming_player_list.Add(streaming_player);
                //    }
                //}
                //else if (rtmp_uri[i, 0] == "null")
                //{
                //    uri_first[i, 0] = true;
                //}
                //else if (rtmp_uri[i, 1] == "null")
                //{
                //    uri_first[i, 1] = true;
                //}
            }

            //    double_click();
            //    myLivePlayers_btn.ItemsSource = Buttons;
        }

        //// double click을 하면 화면이 확대되도록 button을 구현
        //private void double_click()
        //{
        //    for (int i = 0; i < num; i++)
        //    {
        //        Button btn = new Button();
        //        btn.BorderBrush = Brushes.Transparent;
        //        btn.Background = Brushes.Transparent;
        //        btn.Tag = i;
        //        btn.MouseDoubleClick += btn_click;
        //        Buttons.Add(btn);
        //    }
        //}

        //private void btn_click(object sender, RoutedEventArgs e)
        //{
        //    Button btn = sender as Button;

        //    if (zoom_live_player.Visibility == Visibility.Hidden)
        //    {
        //        zoom_live_player.Visibility = Visibility.Visible;
        //        myLivePlayers.Visibility = Visibility.Hidden;
        //        int k = (int)btn.Tag;
        //        zoom_live_player.m_uri = streaming_player_list[k].m_uri;
        //        zoom_live_player.media_play();
        //    }
        //    else
        //    {
        //        zoom_live_player.Visibility = Visibility.Hidden;
        //        myLivePlayers.Visibility = Visibility.Visible;
        //    }
        //}

    }
}