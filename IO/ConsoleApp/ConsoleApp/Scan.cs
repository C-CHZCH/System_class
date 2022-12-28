namespace ConsoleApp;

public class Scan
{
    private readonly LinkedList<Process> _nums;

    private readonly int _start;

    private static int _now;

   

   
    public Scan()
    {
        _nums = new LinkedList<Process>();
        Console.WriteLine($"请输入磁道的开始位置");
        _start = int.Parse(Console.ReadLine());
        _now = _start;
        string[] nums;
        Console.WriteLine("请输入进程数");
        string input = Console.ReadLine();
        Console.WriteLine($"得到的进程数为{input}");
        Console.WriteLine("请输入进程id，进程所需要的磁道位置，并用空格分割开来");
        for (int i = 0; i < int.Parse(input); i++)
        {
            string t = Console.ReadLine();
            nums = t.Split(new string(" "), StringSplitOptions.None);

            Process p = new Process();
            p.Id = nums[0];
            p.Start = int.Parse(nums[1]);
            _nums.AddLast(p);
        }
    }

    public void Work()
    {
        if (_nums.Count() == 0)
        {
            return;
        }
        Process item = null;
        int t = int.MaxValue;
        bool isFind = false;
        foreach (var num in _nums)
        {
            if ( num.Start >=_now )
            {
                isFind = true;
                int res = Math.Abs(_now - num.Start);
                if (res < t)
                {
                    item = num;
                    t = res;
                    item = num;
                }
                
            }
        }//先找磁道比现有的大的
        if (!isFind)
        {
            int t1 = Math.Abs(_now - _nums.First.Value.Start);
            foreach (var num in _nums)
            {
                int res = Math.Abs(_now - num.Start);
                if ( res <t)
                {
                    t = res;
                    item = num;
                }
            }
        }//再找磁道距离小的
        
        _nums.Remove(item);
        int len = Math.Abs(_now - item.Start);
        _now = item.Start;
        Console.WriteLine($"执行{item.Id}进程的磁盘请求,磁道移动了{len},当前磁道位置为{_now}");
        Work();
    }
}