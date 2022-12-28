// See https://aka.ms/new-console-template for more information

using ConsoleApp;

class Program
{
    public static void Main(string[] args)
    {
        bool doItAngin = true;
        do
        {
            Console.WriteLine("1：首次适应算法，2：最佳适应算法，3：, 退出：Exit");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Ffa ffa = new Ffa();
                    ffa.Work();
                    break;
                case "2":
                    Bfa bfa = new Bfa();
                    bfa.Work();
                    break;
                case "3":
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