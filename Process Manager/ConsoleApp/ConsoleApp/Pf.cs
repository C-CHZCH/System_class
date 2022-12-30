namespace ConsoleApp;

using System;
using System.Collections.Generic;

class Pf : Singleton<Pf> //优先级算法
{
    private readonly Comparison<Process> _pfReachTimeComparison = PfReachTimeCompare;

    private readonly Comparison<Process> _pfPriorityAndReachTimeComparison = PfPriorityAndReachTimeCompare;

    private static int PfPriorityAndReachTimeCompare(Process x, Process y)
    {
        if (x.processPCB.priority != y.processPCB.priority) //若优先级不等，则先比较优先级
        {
            return y.processPCB.priority.CompareTo(x.processPCB.priority);
        }
        else
        {
            return x.processPCB.reachTime.CompareTo(y.processPCB.reachTime); //否则比较到达时间
        }
    }

    private readonly List<Process> _pfPreprocesses = new List<Process>();
    private readonly List<Process> _pfAftprocesses = new List<Process>();

    private List<Process> _proc = ProcessManager.Instance.Processes;
    private Process? _runningProcess;
    private Process? _startProcess;
    private readonly List<Process> _blockList = new List<Process>();

    private static int PfReachTimeCompare(Process x, Process y)
    {
        return x.processPCB.reachTime.CompareTo(y.processPCB.reachTime); //根据到达时间比较
    }

    public void Init()
    {
        string[] nums;
        string t;

        Console.WriteLine("进程数:");
        string input = Console.ReadLine();
        Console.WriteLine($"得到进程数:{input}");
        Console.WriteLine("请输入各进程id、到达时间、运行时间、优先级，并且用空格分割");
        for (int i = 0; i < int.Parse(input); i++)
        {
            t = Console.ReadLine();
            nums = t.Split(new string(" "), StringSplitOptions.None);

            PCB pCB = new PCB();
            pCB.id = nums[0];
            pCB.reachTime = float.Parse(nums[1]);
            pCB.needTime = float.Parse(nums[2]);
            pCB.priority = int.Parse(nums[3]);
            pCB.restTime = pCB.needTime;


            ProcessManager.Instance.Processes.Add(new Process(pCB));
        }

        Console.WriteLine("抢占式？（Y/N）"); //是否为抢占式的优先级算法
        t = Console.ReadLine();
        if (t == "Y")
        {
            ProcessManager.Instance.PfPreemptive = true;
            for (int i = 0; i < _proc.Count; i++)
            {
                _proc[i].processPCB.state = PCB.Status.Ready; //把每一个进程的状态修改为准备态
            }
        }

        ProcessManager.Instance.Processes.Sort(_pfReachTimeComparison); //先根据到达时间进行一次排序

        ProcessManager.Instance.nowTime = 0;
    }

    public void Start()
    {
        if (ProcessManager.Instance.PfPreemptive)
        {
            PfPreemptiveInit();
            return;
        }

        for (int m = 0; m < _proc.Count; m++) //每次在更新当前时间后再重新组织一次Pre和Aft列表
        {
            for (int i = 0; i < _proc.Count; i++)
            {
                if (_proc[i].processPCB.reachTime <= ProcessManager.Instance.nowTime &&
                    _proc[i].processPCB.finishTime == 0)
                {
                    _pfPreprocesses.Add(_proc[i]); //  把当前时间之前到达的且还未执行的进程放到pre列表里
                }
                else if (_proc[i].processPCB.reachTime > ProcessManager.Instance.nowTime)
                {
                    _pfAftprocesses.Add(_proc[i]); //  把在当前时间之后到达的进程放在aft列表里
                }
            }

            _pfPreprocesses.Sort(_pfPriorityAndReachTimeComparison); //  当前需要运行的的按优先级和到达时间排
            _pfAftprocesses.Sort(_pfReachTimeComparison); //  正在排队的按到达时间排

            if (_pfPreprocesses.Count != 0)
            {
                //  运行Pre列表第一个
                for (int k = 0; k < _proc.Count; k++)
                {
                    if (_proc[k].processPCB.id == _pfPreprocesses[0].processPCB.id) // 在总进程数组中寻找到第一个开始的进程的id
                    {
                        _proc[k].processPCB.startTime = ProcessManager.Instance.nowTime;
                        _proc[k].processPCB.finishTime = _proc[k].processPCB.startTime + _proc[k].processPCB.needTime;
                        ProcessManager.Instance.nowTime = _proc[k].processPCB.finishTime; //更新nowtime
                        break;
                    }
                }
            }
            else
            {
                //  运行Aft列表第一个
                for (int n = 0; n < _proc.Count; n++)
                {
                    if (_proc[n].processPCB.id == _pfAftprocesses[0].processPCB.id) //  在总进程数组中寻找到该进程的id
                    {
                        _proc[n].processPCB.startTime = _proc[n].processPCB.reachTime;
                        _proc[n].processPCB.finishTime = _proc[n].processPCB.startTime + _proc[n].processPCB.needTime;
                        ProcessManager.Instance.nowTime = _proc[n].processPCB.finishTime;
                        break;
                    }
                }
            }

            _pfPreprocesses.Clear();
            _pfAftprocesses.Clear();
        }
    }


    private void PfPreemptiveInit()
    {
        ReFresh();

        if (_pfPreprocesses.Count != 0)
        {
            //  运行列表第一个
            for (int k = 0; k < _proc.Count; k++)
            {
                if (_proc[k].processPCB.id == _pfPreprocesses[0].processPCB.id) //  找到这个家伙
                {
                    _startProcess = _proc[k];
                    RunProcess(_proc[k]); //  让这个进程跑
                    break;
                }
            }
        }
    }

    private void RunProcess(Process process)
    {
        Console.WriteLine($"进程{process.processPCB.id}在{ProcessManager.Instance.nowTime}时刻开始运行");
        _runningProcess = process;
        if (_runningProcess.processPCB.startTime == 0 && _runningProcess.processPCB.id != _startProcess.processPCB.id)
        {
            _runningProcess.processPCB.startTime = ProcessManager.Instance.nowTime;
        }

        _runningProcess.processPCB.state = PCB.Status.Runing;

        if (IsBlocked()) //  被阻塞了
        {
            RunProcess(_runningProcess);
        }
        else //  没被阻塞
        {
            process.processPCB.finishTime = ProcessManager.Instance.nowTime + process.processPCB.restTime;
            ProcessManager.Instance.nowTime = process.processPCB.finishTime;
            ReFresh();
            process.processPCB.restTime = 0;

            process.processPCB.state = PCB.Status.Destory;
            _blockList.Remove(process);
            Console.WriteLine($"进程{process.processPCB.id}在{process.processPCB.finishTime}时刻执行完毕");

            if (_blockList.Count != 0) //  如果有被阻塞的进程则优先运行
            {
                _blockList.Sort(_pfPriorityAndReachTimeComparison);

                for (int j = 0; j < _proc.Count; j++)
                {
                    if (_proc[j].processPCB.id == _blockList[0].processPCB.id)
                    {
                        RunProcess(_proc[j]);
                        break;
                    }
                }
            }
            else if (_pfAftprocesses.Count != 0) //  没有被阻塞的进程且后面还有进程，等一段时间后开始
            {
                ProcessManager.Instance.nowTime = _pfAftprocesses[0].processPCB.reachTime;
                ReFresh();
                for (int k = 0; k < _proc.Count; k++)
                {
                    if (_proc[k].processPCB.id == _pfAftprocesses[0].processPCB.id)
                    {
                        RunProcess(_proc[k]);
                    }
                }
            }
            else
            {
                Console.WriteLine("所有进程执行完毕");
            }
        }
    }

    public bool IsBlocked()
    {
        for (int i = 0; i < _pfAftprocesses.Count; i++)
        {
            if ((_pfAftprocesses[i].processPCB.reachTime
                 <= _runningProcess.processPCB.startTime + _runningProcess.processPCB.restTime)
                && _pfAftprocesses[i].processPCB.priority >
                _runningProcess.processPCB.priority) //  不等正在运行进程执行完新进程就会进入且新进程优先级更高
            {
                _runningProcess.processPCB.restTime -=
                    (_pfAftprocesses[i].processPCB.reachTime -
                     _runningProcess.processPCB.startTime); //当前线程被抢占，更新 restTime
                _runningProcess.processPCB.state = PCB.Status.Block;
                _blockList.Add(_runningProcess);
                Console.WriteLine(
                    $"进程{_runningProcess.processPCB.id}在{_pfAftprocesses[i].processPCB.reachTime}时刻被进程{_pfAftprocesses[i].processPCB.id}阻塞，" +
                    $"restTime:{_runningProcess.processPCB.restTime}"
                );


                for (int n = 0; n < _proc.Count; n++)
                {
                    if (_proc[n].processPCB.id == _pfAftprocesses[i].processPCB.id) //查找到此进程的id，更换当前的runningProcess
                    {
                        _runningProcess = _proc[n];
                        break;
                    }
                }

                _runningProcess.processPCB.state = PCB.Status.Runing;
                ProcessManager.Instance.nowTime = _runningProcess.processPCB.reachTime; //更新nowTime为此抢占进程的到达时间
                ReFresh(); //刷新Pre和Aft列表
                return true;
            }
            else if ((_pfAftprocesses[i].processPCB.reachTime
                      <= _runningProcess.processPCB.startTime + _runningProcess.processPCB.restTime)
                     && _pfAftprocesses[i].processPCB.priority <=
                     _runningProcess.processPCB.priority) //  不等正在运行进程执行完新进程就会进入但是优先级不足以抢断
            {
                for (int n = 0; n < _proc.Count; n++)
                {
                    if (_proc[n].processPCB.id == _pfAftprocesses[i].processPCB.id) //总进程中寻找到该进程的id
                    {
                        _proc[n].processPCB.state = PCB.Status.Block; //修改该进程的状态
                        _blockList.Add(_proc[n]);
                        Console.WriteLine($"进程{_proc[n].processPCB.id}在时刻{_proc[n].processPCB.reachTime}尝试抢断失败，进入阻塞状态"
                        );
                        break;
                    }
                }
            }
        }

        return false;
    }


    public void ReFresh()
    {
        _pfPreprocesses.Clear();
        _pfAftprocesses.Clear();

        for (int i = 0; i < _proc.Count; i++)
        {
            if (_proc[i].processPCB.reachTime <= ProcessManager.Instance.nowTime && _proc[i].processPCB.restTime != 0)
            {
                _pfPreprocesses.Add(_proc[i]); //  当前时间之前到达的且还未执行完毕的进程放到Pre列表里
            }
            else if (_proc[i].processPCB.reachTime >= ProcessManager.Instance.nowTime)
            {
                _pfAftprocesses.Add(_proc[i]); //  在当前时间及之后到达的进程放在Aft列表里
            }
        }
    }
}