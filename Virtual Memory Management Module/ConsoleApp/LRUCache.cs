namespace ConsoleApp;

public class LRUCache {
    private readonly Dictionary<int, int> _dict;
    private readonly LinkedList<int> _nums;//越靠近链表头越不常使用
    private readonly int _capacity;
    private int _time;
    public LRUCache(int capacity) {
        this._capacity = capacity;
        _dict = new Dictionary<int, int>();
        _nums = new LinkedList<int>();
    }

    public void Process(string nums)
    {
        if (nums == null) throw new ArgumentNullException(nameof(nums));
        int index = 0;
        for (; index < nums.Length; index++)
        {
            Put(nums[index] - '0',nums[index] - '0');
        }
        Print();
    }
    
    public int Get(int key) {
        if (_dict.ContainsKey(key)) {
            _nums.Remove(key);
            _nums.AddLast(key);
            return _dict[key];
        }
        return -1;
    }//使用该页面
    
    public void Put(int key, int value) {
        if (_dict.ContainsKey(key)) {
            _nums.Remove(key);
            _nums.AddLast(key);
            _dict[key] = value;//更新哈希表
            Console.WriteLine("页面中已存在此数据，仅作更新使用频率处理");
        }
        else
        {
           
            if (_nums.Count == _capacity) { 
                _time++;
                Console.WriteLine($"发生缺页且当前表容量已满，淘汰{_nums.First.Value}");
                _dict.Remove(_nums.First.Value);
                _nums.RemoveFirst();
                _nums.AddLast(key);
                _dict.Add(key, value);
            }
            else {
                Console.WriteLine($"发生缺页但当前表仍未满，仅做插入处理");
                _nums.AddLast(key);
                _dict.Add(key, value);
            }
        }
    }
 
    public void Print()
    {
        Console.WriteLine($"总缺页次数为{_time}");
    }
}