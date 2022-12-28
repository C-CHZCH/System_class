namespace ConsoleApp;

class Process
{
    public string process_id;

    public PCB processPCB;

    public Process(PCB processPCB)
    {
        this.processPCB = processPCB;
        this.process_id = processPCB.id;
    }
}