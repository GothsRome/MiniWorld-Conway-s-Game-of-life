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
    //单窗口
    public partial class Form1 : Form
    {
        //线程响应事件初始化
        ManualResetEvent ma = new ManualResetEvent(false);
        //用继续键来响应线程
        private void button_continue_Click(object sender, EventArgs e)
        {

            if (design == 1)
            {
                //结束设计第二阶段
                design++;
                button_end.Visible = true;
            }
            //重置按钮
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
        //搭配快进键加快结束线程
        bool end;
        private void button_end_Click(object sender, EventArgs e)
        {
            if (ending != -1)
                //彻底关闭线程和窗口，退出程序
                System.Environment.Exit(0);
            else if (design == 2)
                end = true;
            ma.Set();
        }
        //用大图层模拟全局鼠标事件
        private void pb_Click(object sender, EventArgs e)
        {
            //结束设计第一阶段
            if (design == 0)
            {
                design++;
                //在事件里方便设置按钮的显示
                button_continue.Visible = true;
            }
            //设计第二阶段
            else if (design == 1)
            {
                //跟踪光标位置设计元胞坐标
                x = (MousePosition.X - 30) / 70;
                y = MousePosition.Y / 70 - 1;
                ma.Set();
                //元胞自反，生死翻转
                maps[0, y, x] = !maps[0, y, x];
            }
        }
        //窗口部件初始化(设计器自动生成代码)
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
            this.pb.Size = new System.Drawing.Size(1924, 1050);//全屏化
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
        int design = 0;//表示前两步设计地图的阶段
        int a, b, c, d;//用来转递属性的数值
        int i, j, ending, wave, lives, now = 0, last, past, same;//C#改写定义变量代码
        //将长宽属性的值限定在有意义的范围内
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
        //设计第二阶段使用的坐标，原理同上
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
        //C#改写地图相关函数
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
        //改写输出函数为图形格式
        void print_parmeter(Graphics g)
        {
            //参数双显示
            s = "回合数=" + Convert.ToString(wave);
            if (ending == 2)
                s += " , " + Convert.ToString(wave + 1);
            s += "  ，生命数= " + Convert.ToString(lives);
            if (ending == 2)
            {
                now = last;
                print_map(1, g);
                count_lives();
                s += " , " + Convert.ToString(lives);
                //拟真回正
                now = past;
            }
            s += " / " + Convert.ToString(h * w);
            g.DrawString(s, f, sb, 15, 60 + 70 * h);
        }
        void print_peroration(Graphics g)
        {
            switch (ending)
            {
                case -2: s = "很可能进入多回合循环，本程序已无法结束。\n"; break;
                case -1: s = "按下列键继续，或快进到结束："; break;
                case 0: s = "元胞已全部死亡，自动结束。"; break;
                case 1: s = "已成为不变的静物，自动结束。"; break;
                case 2:
                    s = "已陷入周期为二回合的震荡循环\n" +
                            "，自动结束，两种状态陈列如上。"; break; 
            }
            //结局收尾显示工作
            if (ending >= 0)
            {
                g.DrawString("游戏已被结束，按下列键重来或关闭程序："
                             , f, sb, 15, 230 + 70 * h);
            }
            //强制收尾显示
            if (ending == -2)
                g.DrawString("按下列键重来或关闭程序："
                             , f, sb, 15, 230 + 70 * h);
            g.DrawString(s, f, sb, 15, 120 + 70 * h);
        }
        //兼顾设计阶段的文字
        void print_slogan(Graphics g)
        {
            switch (design)
            {
                case 0: g.DrawString("请拖动光标设计地图的长宽，" +
                    "\n设计好后任意点击以继续。", f, sb, 15, 640); break;
                case 1: g.DrawString("请点击元胞以翻转设计其生死，" +
                    "\n设计好后按以下按钮继续：", f, sb, 15, 60 + 70 * h); break;
                case 2: { print_parmeter(g); print_peroration(g); break; }
            }
        }
        void print_map(int z, Graphics g)
        {
            //设计尺寸阶段全陈列
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
            //获取进程权限
            Label.CheckForIllegalCrossThreadCalls = false;
            //初始化图形窗口 
            InitializeComponent();
            //创建并开始单线程
            Thread thread = new Thread(Runtime);
            thread.Start();
        }
        //在窗体函数重载中绘制，保证能随时重绘，图形不会消失
        protected override void OnPaint(PaintEventArgs e)
        {
            //使用Bitmap保证图像不闪烁
            Bitmap b = new Bitmap(1924, 1050);
            Graphics g = Graphics.FromImage(b);
            g.Clear(Color.Snow);
            print_slogan(g);
            print_map(0, g);
            //把Bitmap加载到PictureBox上
            pb.BackgroundImage = b;
            //每次都重新加载并释放一个新的Graphics
            g.Dispose();
        }
        //单线程执行函数
        void Runtime()
        {
            while (true)
            {
                //初始化设计阶段
                design = 0;
                //初始化地图配置
                h = 8; w = 10;
                ending = -1; wave = 0;
                end = false;
                for (i = 0; i < 8; i++)
                    for (j = 0; j < 10; j++)
                    {
                        maps[0, i, j] = false;
                        rs[0, i, j] = new Rectangle(30 + 70 * j, 30 + 70 * i, 60, 60);
                    }
                //自定义地图环节
                while (design < 1)
                {
                    //跟踪光标位置设计地图尺寸
                    h = MousePosition.Y / 70;
                    w = (MousePosition.X + 40) / 70;
                    Thread.Sleep(200);
                    //强制窗体重绘图形
                    this.Invalidate();
                }
                //重定义按钮位置
                button_continue.Location = new Point(60, 70 * h + 200);
                button_end.Location = new Point(220, 70 * h + 200);
                //设计好宽高后再生成右图
                for (i = 0; i < h; i++)
                    for (j = 0; j < w; j++)
                        rs[1, i, j] = new Rectangle(70 * (j + w) + 100, 30 + 70 * i, 60, 60);
                while (design < 2)
                {
                    //阻塞线程
                    ma.Reset();
                    ma.WaitOne();
                    this.Invalidate();
                }
                //改写C++循环进程
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
                    //若快进但还是无法结束循环时用来延缓进程
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
                    //总结结局，如果结束了就结束进程
                    if (ending >= 0)
                    {
                        ma.WaitOne();
                        //更换按钮
                        button_continue.Location = new Point(60, 300 + 70 * h);
                        button_end.Location = new Point(220, 300 + 70 * h);
                        button_continue.BackgroundImage = For.restart;
                        button_end.BackgroundImage = For.over;
                        this.Invalidate();
                        //结尾再选择
                        ma.Reset();
                        ma.WaitOne();
                        break;
                    }
                    //等待继续键响应，除非用结束键跳过快进
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