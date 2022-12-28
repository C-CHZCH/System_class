namespace ConsoleApp;

using System;
using System.Collections.Generic;


    class Fcfs : Singleton<Fcfs>//先来先服务
    {
 
        Comparison<Process> _fcfsComparison = FcfsCompare;//用于重载List的Sort
 
        private static int FcfsCompare(Process x, Process y)
        {
            return x.processPCB.reachTime.CompareTo(y.processPCB.reachTime);
        }
 
        public void Init()
        {
            string[] nums;
 
            Console.WriteLine("请输入进程数:");
            string input = Console.ReadLine();
            Console.WriteLine($"得到进程数:{input}");
            Console.WriteLine("请以此输入各进程id、到达时间、运行时间，并用空格分割开来");
            for(int i = 0; i < int.Parse(input); i++)
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
            ProcessManager.Instance.Processes.Sort(_fcfsComparison);//根据到达时间进行排序
 
            List<Process> proc = ProcessManager.Instance.Processes;
 
            ProcessManager.Instance.nowTime = proc[0].processPCB.reachTime;
 
            for (int i =0; i< ProcessManager.Instance.Processes.Count;i++)
            {
                if(proc[i].processPCB.reachTime < ProcessManager.Instance.nowTime)//如果此进程之前就在排队
                {
                    proc[i].processPCB.startTime = ProcessManager.Instance.nowTime;
                }
                else
                {
                    proc[i].processPCB.startTime = proc[i].processPCB.reachTime;
                }
 
                proc[i].processPCB.finishTime = proc[i].processPCB.startTime + proc[i].processPCB.needTime;
                ProcessManager.Instance.nowTime = proc[i].processPCB.finishTime;//更新nowtime
            }
        }
    }
