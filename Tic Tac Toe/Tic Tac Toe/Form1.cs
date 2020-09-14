using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Всякие глобальные переменные
        PictureBox[] GamePole = new PictureBox[9]; //Поле из 9 картинок
        int Player = 0, Computer = 0; // Переменные для выбора кто кем будет играть
        int[] GamePoleMap = { 0,0,0,
                              0,0,0,
                              0,0,0};

        string[] ImgName = //картинки которые будут использоваться
        {
            "empty.png", // Пустое поле
            "krestik.png",
            "circle.png"
        };

        void MainPole() // Функция создания поля игры
        {
            int DX = 0,
                DY = 0;

            //Размер картинки
            int HeightP = 100,
                WidthP = 100,
                IndexPicture = 0; //Индекс подсчета картинок

            string NAME = "P_"; //начало имени в ячейках

            for(int YY = 0; YY < 3; YY++)
            {
                for(int XX = 0; XX < 3; XX++)
                {
                    GamePole[IndexPicture] = new PictureBox()
                    {
                        Name = NAME + IndexPicture, // Имя картинки
                        Height = HeightP,
                        Width = WidthP,
                        Image = Image.FromFile("empty.png"), // Загружаем картинку пустого поля
                        SizeMode = PictureBoxSizeMode.StretchImage, // Подгоняем размеры картинки
                        Location = new Point(DX, DY)
                    };
                    GamePole[IndexPicture].Click += Picture_Click;

                    panel3.Controls.Add(GamePole[IndexPicture]); // Размещаем картинку на панели управления

                    IndexPicture++; // Рассчитываем новое имя

                    DX += WidthP; // координаты по Х для
                }
                DY += HeightP; // По игрик
                DX = 0; // Обнуляем позицию для координаты Х
            }
           
        }

        bool CanStap()
        {
            foreach (int i in GamePoleMap)
                if (i == 0) return true;
            if (TestWin(Player))
            {
                MessageBox.Show("Победа");
                LockPole();
                return false;
            }
            if (TestWin(Computer))
            {
                MessageBox.Show("Проигрыш");
                LockPole();
                return false;
            }

            MessageBox.Show("Ничья");
            LockPole();
            panel1.Visible = true;
            return false;
        }

        bool TestWin(int Who)
        {
            int[,] WinVariant =
            {      {    //1 вариант
                    1,1,1,  //Х Х Х
                    0,0,0,  //_ _ _
                    0,0,0   //_ _ _
                }, {    //2 вариант
                    0,0,0,  //_ _ _
                    1,1,1,  //Х Х Х
                    0,0,0   //_ _ _
                }, {    //3 вариант
                    0,0,0,  //_ _ _
                    0,0,0,  //_ _ _
                    1,1,1   //Х Х Х
                }, {    //4 вариант
                    1,0,0,  //Х _ _
                    1,0,0,  //Х _ _
                    1,0,0   //Х _ _
                }, {    //5 вариант
                    0,1,0,  //_ Х _
                    0,1,0,  //_ Х _
                    0,1,0   //_ Х _
                }, {    //6 вариант
                    0,0,1,  //_ _ Х
                    0,0,1,  //_ _ Х
                    0,0,1   //_ _ Х
                }, {    //7 вариант
                    1,0,0,  //Х _ _
                    0,1,0,  //_ Х _
                    0,0,1   //_ _ Х
                }, {    //8 вариант
                    0,0,1,   //_ _ Х
                    0,1,0,   //_ Х _
                    1,0,0    //Х _ _
                }
            };
            // Получаем поле
            int[] TestMap = new int[GamePoleMap.Length];
            // Просматриваем поле
            for(int i = 0; i < GamePoleMap.Length; i++)
            {
                //если номер в ячейке нам подходит записываем в карту 1
                if (GamePoleMap[i] == Who) TestMap[i] = 1;
            }
            
            //выбираем вариант для сравнения 
            for (int Variant_Index = 0; Variant_Index < WinVariant.GetLength(0); Variant_Index++)
            {
                //счетчик для подсчета соотвествий
                int WinState = 0;
                for (int TestIndex = 0; TestIndex < TestMap.Length; TestIndex++)
                {
                    //если параметр равен 1 то проверяем его иначе 0 тоже = 0
                    if (WinVariant[Variant_Index, TestIndex] == 1)
                    {
                        //если в параметр  в варианте выигрыша совпал с вариантом на карте считаем это в параметре WinState
                        if (WinVariant[Variant_Index, TestIndex] == TestMap[TestIndex]) WinState++;
                    }
                    //если найдены 3 совпадения значит это и есть выигрышная комбинация
                    if (WinState == 3) return true;
                }
            }
            return false;
        }

        private void Picture_Click(object sender, EventArgs e)
        {
            if (CanStap())
            {
                PictureBox ClickImage = sender as PictureBox;
                string[] ParsName = ClickImage.Name.Split('_');

                int IndexSelectImage = Convert.ToInt32(ParsName[1]);

                GamePole[IndexSelectImage].Image = Image.FromFile(ImgName[Player]);
                GamePoleMap[IndexSelectImage] = Player;

                if (!TestWin(Player))
                {
                    LockPole();
                    PC_Step();
                    UnlockPole();
                }
                else
                {
                    MessageBox.Show("Победа");
                    LockPole();
                    panel1.Visible = true;
                }
            }
        }

        void PC_Step()
        {
            Random random = new Random();
         GENER:

            if (CanStap())
            {
                int IndexStep = random.Next(0, 8);

                if (GamePoleMap[IndexStep] == 0)
                {
                    GamePole[IndexStep].Image = Image.FromFile(ImgName[Computer]);
                    GamePoleMap[IndexStep] = Computer;
                }
                else goto GENER;
                if (TestWin(Computer))
                {
                    MessageBox.Show("Проигрыш");
                    panel1.Visible = true;
                }
            }

        }

        void LockPole()// Блокирую поле НАХ
        {
            foreach (PictureBox P in GamePole) P.Enabled = false;
        }

        void UnlockPole()// Разблокирую поле НАХ
        {
            int Indexx = 0;

            foreach(PictureBox P in GamePole)
            {
                // Если поле = 0 - открываем его
                if (GamePoleMap[Indexx++] == 0) P.Enabled = true;
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Player = 2; // Игрок выбрал О
            Computer = 1;
            HidePicture();
            UnlockPole();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Player = 1; // Игрок выбрал X
            Computer = 2;
            HidePicture();
            UnlockPole();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)//MENU
        {
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;

            panel1.Visible = false;
            //обнуляем карту игры
            GamePoleMap = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //обнуляем изображение поля
            foreach (PictureBox P in GamePole) P.Image = Image.FromFile(ImgName[0]);
            //обнуляем выбор игрока
            Player = 0;
            //обнуляем выбор ПК
            Computer = 0;
            //блокируем поле 
            LockPole();
        }

        void HidePicture()
        {
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MainPole();
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            LockPole();
        }

    }
}
