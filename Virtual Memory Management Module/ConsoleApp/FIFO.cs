

namespace ConsoleApp;

public class FIFO
{
    private readonly int _len;

    private  int _time;
    
    private readonly string _nums;

    private readonly Queue<int> _queue;
    
    public FIFO(int len, string t)
    {
        _queue = new Queue<int>();
        _len = len;
        _nums = t ?? throw new ArgumentNullException(nameof(t));
    }

    public void Init()
    {
        for (int i = 0; i < (_len > _nums.Length ? _nums.Length : _len); i++)
        {
            _queue.Enqueue(_nums[i] - '0'); 
        }
        Process();
    }

    private void Process()
    {
        int index = _len;
        while (index < _nums.Length)
        {
            int num = _nums[index] - '0';
            int i = 0;
            foreach (var t in _queue)
            {
                if (t == num) break;
                i++;
            }

            if (i >= _len)
            {
                _time++;
                Console.WriteLine($"淘汰的页面为{_queue.Dequeue()}");
                _queue.Enqueue(num);
            }
            else
            {
                Console.WriteLine("表中还有此数据");
            }

            index++;
        }
        
        Print();
    }

    
    private void Print()
    {
        Console.WriteLine($"缺页次数为{_time}");
    }
}