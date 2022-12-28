
using ConsoleApp;
class Program
{
    public static void Main(string[] args)
    {
        string example = @""" A 55
        B 58
        C 39
        D 18
        E 90
        F 160
        G 150
        H 38
        I 184
        """;
        
        bool doItAngin = true;
        do
        {
            Console.WriteLine("1：sstf短寻道算法，2：fcfs先来先服务算法，3：scan电梯（向磁道增加方向）算法, 退出：exit");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":Sstf sstf = new Sstf();
                    Console.WriteLine($"sstf开始了");
                    sstf.Work();
                    break;
                case "2":
                    Fcfs fcfs = new Fcfs();
                    Console.WriteLine($"fcfs开始了");
                    fcfs.Work();;
                    break;
                case "3":
                    Scan scan = new Scan();
                    Console.WriteLine($"scan开始了");
                    scan.Work();
                    break;
                case "exit":
                    doItAngin = false;
                    break;
                default:
                    Console.WriteLine("输入错误，请输入三个数字之一或exit");
                    break;
            }
        } while (doItAngin);
        
        
        
    }


}