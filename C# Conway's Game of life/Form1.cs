using Conway_s_Game_of_life.Properties;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Conway_s_Game_of_life
{
    //������
    public partial class Form1 : Form
    {
        //�߳���Ӧ�¼���ʼ��
        ManualResetEvent ma = new ManualResetEvent(false);
        //�ü���������Ӧ�߳�
        private void button_continue_Click(object sender, EventArgs e)
        {

            if (design == 1)
            {
                //������Ƶڶ��׶�
                design++;
                button_end.Visible = true;
            }
            //���ð�ť
            else if (ending != -1)
            {
                this.button_continue.Visible = false;
                this.button_end.Visible = false;
                this.button_continue.BackgroundImage = For._continue;
                this.button_end.BackgroundImage = For.end;
                design++;
            }
            ma.Set();
        }
        //���������ӿ�����߳�
        bool end;
        private void button_end_Click(object sender, EventArgs e)
        {
            if (ending != -1)
                //���׹ر��̺߳ʹ��ڣ��˳�����
                System.Environment.Exit(0);
            else if (design == 2)
                end = true;
            ma.Set();
        }
        //�ô�ͼ��ģ��ȫ������¼�
        private void pb_Click(object sender, EventArgs e)
        {
            //������Ƶ�һ�׶�
            if (design == 0)
            {
                design++;
                //���¼��﷽�����ð�ť����ʾ
                button_continue.Visible = true;
            }
            //��Ƶڶ��׶�
            else if (design == 1)
            {
                //���ٹ��λ�����Ԫ������
                x = (MousePosition.X - 30) / 70;
                y = MousePosition.Y / 70 - 1;
                ma.Set();
                //Ԫ���Է���������ת
                maps[0, y, x] = !maps[0, y, x];
            }
        }
        //���ڲ�����ʼ��(������Զ����ɴ���)
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = 
                new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button_continue = new System.Windows.Forms.Button();
            this.button_end = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.pb = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb)).BeginInit();
            this.SuspendLayout();
            //button_continue
            this.button_continue.BackgroundImage = For._continue;
            this.button_continue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_continue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_continue.Location = new System.Drawing.Point(60, 800);
            this.button_continue.Name = "button_continue";
            this.button_continue.Size = new System.Drawing.Size(100, 100);
            this.button_continue.UseVisualStyleBackColor = true;
            this.button_continue.Visible = false;
            this.button_continue.Click += new System.EventHandler(this.button_continue_Click);
            //button_end
            this.button_end.BackgroundImage = For.end;
            this.button_end.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_end.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_end.Location = new System.Drawing.Point(220, 800);
            this.button_end.Name = "button_end";
            this.button_end.Size = new System.Drawing.Size(100, 100);
            this.button_end.UseVisualStyleBackColor = true;
            this.button_end.Visible = false;
            this.button_end.Click += new System.EventHandler(this.button_end_Click);
            //pb(PictureBox)
            this.pb.Location = new System.Drawing.Point(0, 0);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(1924, 1050);//ȫ����
            this.pb.TabStop = false;
            this.pb.Click += new System.EventHandler(this.pb_Click);
            //Form1
            this.ClientSize = new System.Drawing.Size(1924, 1050);
            this.Controls.Add(this.button_end);
            this.Controls.Add(this.button_continue);
            this.Controls.Add(this.pb);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb)).EndInit();
            this.ResumeLayout(false);
        }
        bool[,,] maps = new bool[3, 8, 10];
        int design = 0;//��ʾǰ������Ƶ�ͼ�Ľ׶�
        int a, b, c, d;//����ת�����Ե���ֵ
        int i, j, ending, wave, lives, now = 0, last, past, same;//C#��д�����������
        //���������Ե�ֵ�޶���������ķ�Χ��
        public int h 
        {
            set 
            {
                if (value < 1)
                    a = 1;
                else if (value > 8)
                    a = 8;
                else
                    a = value;
            }
            get { return a; }        
        }
        public int w
        {
            set
            {
                if (value < 1)
                    b = 1;
                else if (value > 10)
                    b = 10;
                else
                    b = value;
            }
            get { return b; }
        }
        //��Ƶڶ��׶�ʹ�õ����꣬ԭ��ͬ��
        public int x
        {
            set
            {
                if (value < 0)
                    c = 0;
                else if (value >= w)
                    c = w - 1;
                else
                    c = value;
            }
            get { return c; }
        }
        public int y
        {
            set
            {
                if (value < 0)
                    d = 0;
                else if (value >= h)
                    d = h - 1;
                else
                    d = value;
            }
            get { return d; }
        }
        int[,] neighbors = new int[8, 10];
        Pen p = new Pen(Color.Black, 3);
        SolidBrush sb = new SolidBrush(Color.Black);
        Font f = new Font("Arial", 20, FontStyle.Bold);
        Rectangle[,,] rs = new Rectangle[2, 8, 10];
        string s;
        //C#��д��ͼ��غ���
        void count_lives()
        {
            lives = 0;
            for (i = 0; i < h; i++)
                for (j = 0; j < w; j++)
                    if (maps[now, i, j] == true)
                        lives++;
        }
        void count_neighbors()
        {
            for (i = 0; i < h; i++)
                for (j = 0; j < w; j++)
                {
                    neighbors[i, j] = 0;
                    if (i > 0 &&
                        maps[now, i - 1, j] == true)
                        neighbors[i, j]++;
                    if (i < h - 1 &&
                        maps[now, i + 1, j] == true)
                        neighbors[i, j]++;
                    if (j > 0 &&
                        maps[now, i, j - 1] == true)
                        neighbors[i, j]++;
                    if (j < w - 1 &&
                        maps[now, i, j + 1] == true)
                        neighbors[i, j]++;
                    if (i > 0 && j > 0 &&
                        maps[now, i - 1, j - 1] == true)
                        neighbors[i, j]++;
                    if (i < h - 1 && j > 0 &&
                        maps[now, i + 1, j - 1] == true)
                        neighbors[i, j]++;
                    if (i > 0 && j < w - 1 &&
                        maps[now, i - 1, j + 1] == true)
                        neighbors[i, j]++;
                    if (i < h - 1 &&
                        j < w - 1 &&
                        maps[now, i + 1, j + 1] == true)
                        neighbors[i, j]++;
                }
        }
        void count_same(int e, int f)
        {
            same = 0;
            for (i = 0; i < h; i++)
                for (j = 0; j < w; j++)
                    if (maps[e, i, j] == maps[f, i, j])
                        same++;
        }
        void change_map()
        {
            for (i = 0; i < h; i++)
                for (j = 0; j < w; j++)
                {
                    if (maps[now, i, j] == false)
                        if (neighbors[i, j] == 3)
                            maps[past, i, j] = true;
                        else
                            maps[past, i, j] = false;
                    else
                        if (neighbors[i, j] != 2
                         && neighbors[i, j] != 3)
                        maps[past, i, j] = false;
                    else
                        maps[past, i, j] = true;
                }
        }
        //��д�������Ϊͼ�θ�ʽ
        void print_parmeter(Graphics g)
        {
            //����˫��ʾ
            s = "�غ���=" + Convert.ToString(wave);
            if (ending == 2)
                s += " , " + Convert.ToString(wave + 1);
            s += "  ��������= " + Convert.ToString(lives);
            if (ending == 2)
            {
                now = last;
                print_map(1, g);
                count_lives();
                s += " , " + Convert.ToString(lives);
                //�������
                now = past;
            }
            s += " / " + Convert.ToString(h * w);
            g.DrawString(s, f, sb, 15, 60 + 70 * h);
        }
        void print_peroration(Graphics g)
        {
            switch (ending)
            {
                case -2: s = "�ܿ��ܽ����غ�ѭ�������������޷�������\n"; break;
                case -1: s = "�����м�������������������"; break;
                case 0: s = "Ԫ����ȫ���������Զ�������"; break;
                case 1: s = "�ѳ�Ϊ����ľ���Զ�������"; break;
                case 2:
                    s = "����������Ϊ���غϵ���ѭ��\n" +
                            "���Զ�����������״̬�������ϡ�"; break; 
            }
            //�����β��ʾ����
            if (ending >= 0)
            {
                g.DrawString("��Ϸ�ѱ������������м�������رճ���"
                             , f, sb, 15, 230 + 70 * h);
            }
            //ǿ����β��ʾ
            if (ending == -2)
                g.DrawString("�����м�������رճ���"
                             , f, sb, 15, 230 + 70 * h);
            g.DrawString(s, f, sb, 15, 120 + 70 * h);
        }
        //�����ƽ׶ε�����
        void print_slogan(Graphics g)
        {
            switch (design)
            {
                case 0: g.DrawString("���϶������Ƶ�ͼ�ĳ���" +
                    "\n��ƺú��������Լ�����", f, sb, 15, 640); break;
                case 1: g.DrawString("����Ԫ���Է�ת�����������" +
                    "\n��ƺú����°�ť������", f, sb, 15, 60 + 70 * h); break;
                case 2: { print_parmeter(g); print_peroration(g); break; }
            }
        }
        void print_map(int z, Graphics g)
        {
            //��Ƴߴ�׶�ȫ����
            if (design == 0)
                for (i = 0; i < 8; i++)
                    for (j = 0; j < 10; j++)
                        if (i < h && j < w)
                            g.DrawRectangle(p, rs[z, i, j]);
                        else
                            g.FillRectangle(sb, rs[z, i, j]);
            else
                for (i = 0; i < h; i++)
                    for (j = 0; j < w; j++)
                        if (maps[now, i, j] == true)
                            g.FillRectangle(sb, rs[z, i, j]);
                        else
                            g.DrawRectangle(p, rs[z, i, j]);
        }
        public Form1()
        {
            //��ȡ����Ȩ��
            Label.CheckForIllegalCrossThreadCalls = false;
            //��ʼ��ͼ�δ��� 
            InitializeComponent();
            //��������ʼ���߳�
            Thread thread = new Thread(Runtime);
            thread.Start();
        }
        //�ڴ��庯�������л��ƣ���֤����ʱ�ػ棬ͼ�β�����ʧ
        protected override void OnPaint(PaintEventArgs e)
        {
            //ʹ��Bitmap��֤ͼ����˸
            Bitmap b = new Bitmap(1924, 1050);
            Graphics g = Graphics.FromImage(b);
            g.Clear(Color.Snow);
            print_slogan(g);
            print_map(0, g);
            //��Bitmap���ص�PictureBox��
            pb.BackgroundImage = b;
            //ÿ�ζ����¼��ز��ͷ�һ���µ�Graphics
            g.Dispose();
        }
        //���߳�ִ�к���
        void Runtime()
        {
            while (true)
            {
                //��ʼ����ƽ׶�
                design = 0;
                //��ʼ����ͼ����
                h = 8; w = 10;
                ending = -1; wave = 0;
                end = false;
                for (i = 0; i < 8; i++)
                    for (j = 0; j < 10; j++)
                    {
                        maps[0, i, j] = false;
                        rs[0, i, j] = new Rectangle(30 + 70 * j, 30 + 70 * i, 60, 60);
                    }
                //�Զ����ͼ����
                while (design < 1)
                {
                    //���ٹ��λ����Ƶ�ͼ�ߴ�
                    h = MousePosition.Y / 70;
                    w = (MousePosition.X + 40) / 70;
                    Thread.Sleep(200);
                    //ǿ�ƴ����ػ�ͼ��
                    this.Invalidate();
                }
                //�ض��尴ťλ��
                button_continue.Location = new Point(60, 70 * h + 200);
                button_end.Location = new Point(220, 70 * h + 200);
                //��ƺÿ�ߺ���������ͼ
                for (i = 0; i < h; i++)
                    for (j = 0; j < w; j++)
                        rs[1, i, j] = new Rectangle(70 * (j + w) + 100, 30 + 70 * i, 60, 60);
                while (design < 2)
                {
                    //�����߳�
                    ma.Reset();
                    ma.WaitOne();
                    this.Invalidate();
                }
                //��дC++ѭ������
                while (design < 3)
                {
                    now = wave % 3;
                    last = (wave + 2) % 3;
                    past = (wave + 1) % 3;
                    if (wave < 100)
                        ending = -1;
                    count_lives();
                    if (lives == 0)
                        ending = 0;
                    else
                    {
                        count_same(now, last);
                        if (same == h * w)
                            ending = 1;
                        else
                        {
                            count_same(now, past);
                            if (same == h * w)
                                ending = 2;
                        }
                    }
                    //������������޷�����ѭ��ʱ�����ӻ�����
                    if (wave > 99)
                        Thread.Sleep(500);
                    if (wave == 100)
                    {
                        ending = -2;
                        button_continue.Location = new Point(60, 300 + 70 * h);
                        button_end.Location = new Point(220, 300 + 70 * h);
                        button_continue.BackgroundImage = For.restart;
                        button_end.BackgroundImage = For.over;
                    }
                    this.Invalidate();
                    //�ܽ��֣���������˾ͽ�������
                    if (ending >= 0)
                    {
                        ma.WaitOne();
                        //������ť
                        button_continue.Location = new Point(60, 300 + 70 * h);
                        button_end.Location = new Point(220, 300 + 70 * h);
                        button_continue.BackgroundImage = For.restart;
                        button_end.BackgroundImage = For.over;
                        this.Invalidate();
                        //��β��ѡ��
                        ma.Reset();
                        ma.WaitOne();
                        break;
                    }
                    //�ȴ���������Ӧ�������ý������������
                    ma.Reset();
                    if (!end)
                        ma.WaitOne();
                    count_neighbors();
                    change_map();
                    wave++;
                }
            }
        }
    }
}