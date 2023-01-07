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
        Console.WriteLine("FIFO");
        FIFO fifo = new FIFO(len,nums);
        fifo.Init();//70120304230321201701
        Console.WriteLine("LRU");
        LRUCache lruCache = new LRUCache(len);
        lruCache.Process(nums);
        Console.WriteLine("LFU");
        LFUCache lfuCache = new LFUCache(len);
        lfuCache.Process(nums);
    } //0213546374733553111723410
}