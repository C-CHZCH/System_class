//SDK .NET 7.0
using ConsoleApp;


public class Program
{
    public static void Main(string[] args)
    {
        int len;
        string nums;
        Console.WriteLine("输入长度以及页面串");
        len = int.Parse(Console.ReadLine());
        nums = Console.ReadLine();
        //FIFO fifo = new FIFO(len,nums);
        //fifo.Init();//70120304230321201701
        //LRUCache lruCache = new LRUCache(len);
        //lruCache.Process(nums);
        LFUCache lfuCache = new LFUCache(len);
        lfuCache.Process(nums);
    }
    
} 


