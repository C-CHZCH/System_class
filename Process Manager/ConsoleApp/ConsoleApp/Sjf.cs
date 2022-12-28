namespace ConsoleApp;

using System;
using System.Collections.Generic;

class Sjf : Singleton<Sjf>//短作业优先算法
{
    readonly Comparison<Process> _sjfReachTimeComparison = SjfReachTimeCompare;
    readonly Comparison<Process> _sjfNeedTimeComparison = SjfNeedTimeCompare;

    private static int SjfNeedTimeCompare(Process x, Process y)
    {
        return x.processPCB.needTime.CompareTo(y.processPCB.needTime);
    }

    readonly List<Process> _sjfPreprocesses = new List<Process>();
    readonly List<Process> _sjfAftprocesses = new List<Process>();

    private static int SjfReachTimeCompare(Process x, Process y)
    {
        return x.processPCB.reachTime.CompareTo(y.processPCB.reachTime);
    }

    public void Init()
    {
        string[] nums;

        Console.WriteLine("进程数:");
        string input = Console.ReadLine();
        Console.WriteLine($"得到进程数:{input}" );
        Console.WriteLine("请输入各进程id、到达时间、运行时间，并用空格分割");
        for (int i = 0; i < int.Parse(input); i++)
        {
            string t = Console.ReadLine();
            nums = t.Split(new string(" "), StringSplitOptions.None);

            PCB pCB = new PCB();
            pCB.id = nums[0];
            pCB.reachTime = float.Parse(nums[1]);
            pCB.needTime = float.Parse(nums[2]);

            ProcessManager.Instance.Processes.Add(new Process(pCB));
        }
    }

    public void Start()
    {
        ProcessManager.Instance.Processes.Sort(_sjfReachTimeComparison);

        List<Process> proc = ProcessManager.Instance.Processes;

        ProcessManager.Instance.nowTime = 0;

        for (int j = 0; j < proc.Count; j++)
        {
            for (int i = 0; i < proc.Count; i++)
            {
                if (proc[i].processPCB.reachTime <= ProcessManager.Instance.nowTime &&
                    proc[i].processPCB.finishTime == 0)
                {
                    _sjfPreprocesses.Add(proc[i]); //  当前时间之前到达的且还未执行的进程放到Pre列表里
                }
                else if (proc[i].processPCB.reachTime > ProcessManager.Instance.nowTime)
                {
                    _sjfAftprocesses.Add(proc[i]); //  在当前时间之后到达的进程放在Aft列表里
                }
            }

            _sjfPreprocesses.Sort(_sjfNeedTimeComparison); //  前面没执行的的按作业时间
            _sjfAftprocesses.Sort(_sjfReachTimeComparison); //  后面的按到达时间

            if (_sjfPreprocesses.Count != 0) //  若有当前时间之前到达的进程
            {
                for (int k = 0; k < proc.Count; k++)
                {
                    if (proc[k].processPCB.id == _sjfPreprocesses[0].processPCB.id) //  在总进程数组中寻找到此进程
                    {
                        proc[k].processPCB.startTime = ProcessManager.Instance.nowTime;
                        proc[k].processPCB.finishTime = proc[k].processPCB.startTime + proc[k].processPCB.needTime;
                        ProcessManager.Instance.nowTime = proc[k].processPCB.finishTime;
                        break;
                    }
                }
            }
            else //  没有当前时间之前到达的进程
            {
                for (int n = 0; n < proc.Count; n++)
                {
                    if (proc[n].processPCB.id == _sjfAftprocesses[0].processPCB.id) //  在总进程数组中寻找到此进程
                    {
                        proc[n].processPCB.startTime = proc[n].processPCB.reachTime;
                        proc[n].processPCB.finishTime = proc[n].processPCB.startTime + proc[n].processPCB.needTime;
                        ProcessManager.Instance.nowTime = proc[n].processPCB.finishTime;
                        break;
                    }
                }
                //  执行后面第一个
            }

            _sjfPreprocesses.Clear();
            _sjfAftprocesses.Clear();
        }
    }
}