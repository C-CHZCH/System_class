namespace ConsoleApp;

public class Ffa//首次适应算法
{
    private List<Partition> _list;

    private int _sumlen;

    public Ffa()
    {
        _list = new List<Partition>();
        Console.WriteLine("请输入各个分区的起始地址以及分区长度，并以空格分割开来，以exit结束");
        bool doItAngin = true;
        do
        {
            string[] num;
            string input = Console.ReadLine();
            if(input == "exit")break;
            num = input.Split(new string(" "), StringSplitOptions.None);
            Partition p = new Partition();
            p.StartAdd = int.Parse(num[0]);
            p.Len = int.Parse(num[1]);
            _list.Add(p);
            _sumlen += p.Len;
            
        } while (doItAngin);
        Console.WriteLine($"空间为{_sumlen}");
    }

    public void Work()
    {
        bool doItAngin = true;
        do
        {
            Console.WriteLine($"请输入需要分配的任务,分别输入任务名以及任务所需的空间，以空格分割开来。输入exit退出");
            string input = Console.ReadLine();
            string[] num;
            if (input == "exit") break;
            num = input.Split(new string(" "), StringSplitOptions.None);
            Homework t = new Homework();
            t.Name = num[0];
            t.Len = int.Parse(num[1]);
            bool isDistribute = false;
            for (int i = 0; i < _list.Count(); i++)
            {
                if (_list[i].Len >= t.Len)
                {
                    _list[i].Len -= t.Len;
                    _list[i].Isused = true;
                    _list[i].Hw.Add(t);
                    isDistribute = true;
                    _sumlen -= t.Len;
                    break;
                }
            }

            if (isDistribute)
            {
                Console.WriteLine($"此任务已被分配，当前整个内存分区可用空间为{_sumlen}");
            }
            
            if (_sumlen == 0)
            {
                doItAngin = false;
            }
        } while (doItAngin);
        Print();
    }

    public void Print()
    {
        
        int id = 1;
        foreach (var t in _list)
        {
            Console.WriteLine("分区号    分区剩余大小    分区起始地址      分区状态");
            Console.WriteLine("{0,-6} {1,14} {2,15} {3,13}", id, t.Len, t.StartAdd, t.Isused);
            Console.WriteLine("此分区下的任务名为任务长度");
            foreach (var j in t.Hw)
            {
                Console.WriteLine($"任务名：{j.Name} 任务长度：{j.Len}");
            }

            id++;
        }
    }
}