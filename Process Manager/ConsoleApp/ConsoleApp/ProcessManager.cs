namespace ConsoleApp;

using System;
using System.Collections.Generic;

class ProcessManager : Singleton<ProcessManager>
{
    public List<Process> Processes = new List<Process>(); //  所有输入的进程

    public bool PfPreemptive = false;

    public float nowTime;

    public void StartFCFS()
    {
        Fcfs.Instance.Init();
        Fcfs.Instance.Start();

        PrintOut();
        Console.WriteLine("Fcfs Finish");
        Reset();
    }

    public void StartSJF()
    {
        Sjf.Instance.Init();
        Sjf.Instance.Start();

        PrintOut();
        Console.WriteLine("Sjf Finish");
        Reset();
    }

    public void StartPF()
    {
        Pf.Instance.Init();
        Pf.Instance.Start();

        PrintOut();
        Console.WriteLine("Pf Finish");
        Reset();
    }


    public void Reset()
    {
        Processes.Clear();
        PfPreemptive = false;
        nowTime = 0;
    }
    
    public void PrintOut()
    {
        Console.WriteLine("\n进程id  到达时间  运行时间  开始时间  完成时间");
        for (int i = 0; i < Processes.Count; i++)
        {
            Console.WriteLine("{0,-6} {1,9:F2} {2,9:F2} {3,9:F2} {4,9:F2} ",
                Processes[i].processPCB.id, Processes[i].processPCB.reachTime, Processes[i].processPCB.needTime,
                Processes[i].processPCB.startTime, Processes[i].processPCB.finishTime);
        }
    }
}