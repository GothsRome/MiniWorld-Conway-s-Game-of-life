#include <iostream>
//引用头文件以使用其库中的_getch函数
#include <conio.h>
using namespace std;
//方形地图长宽常量
const int height = 5, length = 5;
//定义并初始化本回合的地图，
//用数组附加定义其它两回合的地图
bool maps[3][height][length]
= { { {1,1,0,1,1},
      {1,1,0,1,1},
      {0,0,0,0,0},
      {1,1,0,1,1},
      {1,1,0,1,1} } };
//基本参数
int wave = 0, lives,
//当前回合、上回合和上上回合的序数
now, last, past,
//状态相同的元胞数
same,
//二重循环中使用的两个变量
i, j,
//每个元胞的活邻居数量，用二维数组的形式定义
//，与方形地图一一对应，便于统计
neighbors[height][length];
//清屏并在首行显示基本参数
void print_parmeter()
{
    system("cls");
    cout << "回合数=" << wave << "，生命数="
         << lives << " / " << height * length
         << "\n" << endl;
}
//在控制台中打印地图
void print_map(int h)
{
    for (i = 0; i < height; i++)
    {
        cout << "  ";
        for (j = 0; j < length; j++)
            
            if (maps[h][i][j] == 0)
                cout << "□";
            else
                cout << "■";
        cout << endl;
    }
    cout << endl;
}
//统计地图中活元胞总数
void count_lives()
{
    lives = 0;
    for (i = 0; i < height; i++)
        for (j = 0; j < length; j++)
            if (maps[now][i][j] == 1)
                lives++;
}
//统计每个元胞周围八格的活邻数
//，考虑了边界条件
void count_neighbors()
{
    for (i = 0; i < height; i++)
        for (j = 0; j < length; j++)
        {
            neighbors[i][j] = 0;
            if (i > 0 && 
                maps[now][i - 1][j] == 1)
                neighbors[i][j]++;
            if (i < height - 1 &&
                maps[now][i + 1][j] == 1)
                neighbors[i][j]++;
            if (j > 0 && 
                maps[now][i][j - 1] == 1)
                neighbors[i][j]++;
            if (j < length - 1 &&
                maps[now][i][j + 1] == 1)
                neighbors[i][j]++;
            if (i > 0 && j > 0 && 
                maps[now][i - 1][j - 1] == 1)
                neighbors[i][j]++;
            if (i < height - 1 && j > 0 &&
                maps[now][i + 1][j - 1] == 1)
                neighbors[i][j]++;
            if (i > 0 && j < length - 1 &&
                maps[now][i - 1][j + 1] == 1)
                neighbors[i][j]++;
            if (i < height - 1 &&
                j < length - 1 &&
                maps[now][i + 1][j + 1] == 1)
                neighbors[i][j]++;
        }
}
//统计same值
void count_same(int x,int y)
{
    same = 0;
    for (i = 0; i < height; i++)
        for (j = 0; j < length; j++)
            if (maps[x][i][j] 
                == maps[y][i][j])
                same++;
}
//用本回合活邻数据代入B3/S23规则
//，使地图演化到下回合
 void change_map()
{
    for (i = 0; i < height; i++)
        for (j = 0; j < length; j++)
        {
            if (maps[now][i][j] == 0)
                if(neighbors[i][j] == 3)
//B3规则：若本回合死元胞有三个活邻
//，则下回合变为活元胞
                    maps[past][i][j] = 1;
                else
                    maps[past][i][j] = 0;
            else 
                if (neighbors[i][j] != 2 
                 && neighbors[i][j] != 3)
//S23规则：
// 除非本回合活元胞有两个或三个活邻
//，下回合才能继续存活，否则变为死元胞
                    maps[past][i][j] = 0;
                else
                    maps[past][i][j] = 1;
        }
}
int main()
{
    //任意生命游戏都能无限演化
    while (1)
    {
        //计算三个
        now = wave % 3;
        last = (wave + 2) % 3;
        past = (wave + 1) % 3;
        //序数
        count_lives();
        print_parmeter();
        print_map(now);
        //排除无观赏性的全死局
        if (lives == 0)
        {
            cout << 
            "元胞已全部死亡，自动结束。"
                 << endl;
            break;
        }
        count_neighbors();
        count_same(now, last);
//若same值达到地图总元胞数，
//则已成为不变的静物，同理自动结束
        if (same == height * length)
        {
            cout << 
            "已成为不变的静物，自动结束。"
                 << endl;
            break;
        }
//若same值达到地图总元胞数，
//则已陷入震荡循环，同理自动结束
        count_same(now, past);
        if (same == height * length)
        {
            print_parmeter();
//在此尺度下震荡子一般周期为二，
//尺度最小的三周期震荡子也不足所需
            cout << 
            "已陷入周期为二回合的震荡循环" 
                 << endl; 
            cout << 
            "，自动结束，两种状态陈列如下：\n" 
                 << endl;
            print_map(now);
            print_map(last);
            break;
        }
//若无震荡，
//则用下回合数据覆盖上上回合数据
        change_map();
        //使回合数增加一
        wave++;
        //让用户选择
        cout << 
        "按回车键继续，按其它键结束。" 
             << endl;
        //用_getch函数直接获取键入
        if (_getch() != '\r')
            break;
    }
    cout << "游戏已被结束。" << endl;
    //生命游戏只可能被人为结束
    //，本身并无结束规则
    return 0;
}

