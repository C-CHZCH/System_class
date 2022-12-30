namespace ConsoleApp;

class PCB
{
    public string id; //  进程ID

    public float reachTime; //  到达时间

    public float needTime; //  还需时间

    public float startTime; //  开始时间

    public float finishTime; //  完成时间

    public Status state; //  状态

    public float restTime; //  剩余时间

    public int priority; //优先级

    public enum Status
    {
        Ready = 0,
        Runing = 1,
        Block = 2,
        Destory = 3
    }
}