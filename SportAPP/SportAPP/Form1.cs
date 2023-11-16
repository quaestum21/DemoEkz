using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportAPP
{
    public partial class Form1 : Form
    {
        DataBase db = new DataBase();
        private string text = String.Empty;
        public Form1()
        {
            InitializeComponent();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
        }
        
        private int countdown = 180;
        private void timer_Tick(object sender, EventArgs e)
        {
            countdown--;
            TimeForEnter.Text = countdown.ToString();
            if (countdown < 0)
            {
                button2.Visible = true;
                timer.Stop();
                countdown = 180;
                MessageBox.Show("Попытка ввода вновь доступна!");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private int _countFalseEnters = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            if (_countFalseEnters > 0)
            {
                if (_countFalseEnters > 2)
                {
                    MessageBox.Show("Повторите через 3 минуты");
                    button2.Visible = false;
                    timer.Start();
                    return;
                }
                if (textBox3.Text == this.text)
                {

                }
                else
                {
                    MessageBox.Show("Ошибка капчи");
                    pictureBox1.Image = this.CreateImage(pictureBox1.Width, pictureBox1.Height);
                    _countFalseEnters++;
                    return;
                }
            }
            var login = textBox1.Text;
            var password = textBox2.Text;
          
            string queryString = $"select UserRole from [User] where UserLogin = '{login}' and UserPassword = '{password}'";
          
            SqlCommand command = new SqlCommand(queryString, db.GetConnection());

            db.openConnection();
            object i = command.ExecuteScalar();
            int c = Convert.ToInt32(i);
            db.closeConnection();
         
                //проверка на роль
            if (c == 3)
            {
                MessageBox.Show("Вы зашли под учетной записью администратора.");
                this.Hide();
                Goods goodsForm = new Goods(3);
                    goodsForm.ShowDialog();
                    this.Show();

            }
            else if (c == 2)
            {
                 MessageBox.Show("Вы зашли под учетной записью менеджера.");
                    this.Hide();
                    Goods goodsForm = new Goods(2);
                    goodsForm.ShowDialog();
                    this.Show();
                }
            else if (c == 1)
            {
                    MessageBox.Show("Вы зашли под учетной записью клиента.");
                    this.Hide();
                    Goods goodsForm = new Goods(1);
                    goodsForm.ShowDialog();
                    this.Show();
                    
            }
            else
            {
                
                MessageBox.Show("Данные для входа неверны"); MessageBox.Show("такого аккаунта не существует!");
                textBox3.Visible = true;
                ReloadCaptcha.Visible = true;
                pictureBox1.Image = this.CreateImage(pictureBox1.Width, pictureBox1.Height);
                _countFalseEnters++;
                if (_countFalseEnters > 2)
                {
                    MessageBox.Show("Повторите через 3 минуты");
                    button1.Visible = false;
                    timer.Start();
                }
            }
            
        }

        private Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();

            //Создадим изображение
            Bitmap result = new Bitmap(Width, Height);

            //Вычислим позицию текста
            int Xpos = rnd.Next(0, Width - 50);
            int Ypos = rnd.Next(15, Height - 15);

            //Добавим различные цвета
            Brush[] colors = { Brushes.Black,
                     Brushes.Red,
                     Brushes.RoyalBlue,
                     Brushes.Green };

            //Укажем где рисовать
            Graphics g = Graphics.FromImage((Image)result);

            //Пусть фон картинки будет серым
            g.Clear(Color.Gray);

            //Сгенерируем текст
            text = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 5; ++i)
                text += ALF[rnd.Next(ALF.Length)];

            //Нарисуем сгенирируемый текст
            g.DrawString(text,
                         new Font("Arial", 15),
                         colors[rnd.Next(colors.Length)],
                         new PointF(Xpos, Ypos));

            //Добавим немного помех
            /////Линии из углов
            g.DrawLine(Pens.Black,
                       new Point(0, 0),
                       new Point(Width - 1, Height - 1));
            g.DrawLine(Pens.Black,
                       new Point(0, Height - 1),
                       new Point(Width - 1, 0));
            ////Белые точки
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);

            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Goods goodsForm = new Goods(1);
            goodsForm.ShowDialog();
            this.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ReloadCaptcha_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Image = this.CreateImage(pictureBox1.Width, pictureBox1.Height);
        }
    }
}
