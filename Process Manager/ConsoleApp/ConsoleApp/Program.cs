

using ConsoleApp;


class Program
{
    bool doItAngin;

    static void Main(string[] args)
    {
        Program program = new Program();
        program.NewMethod();
    }

    private void NewMethod()
    {
        doItAngin = true;

        
        do
        {
            Console.WriteLine("1：先来先服务调度算法，2：短作业优先调度算法，3：优先级调度算法, 退出：Exit");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ProcessManager.Instance.StartFCFS();//A 9.4 20
                    //B 10.1 10
                    //C 10.05 30
                    //D 9.55 15
                    //E 9.45 25

                    break;
                case "2":
                    ProcessManager.Instance.StartSJF();//A 0 4
                    //B 1 3
                    //C 2 5
                    //D 3 2
                    //E 4 4 
                    break;
                case "3":
                    ProcessManager.Instance.StartPF();
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