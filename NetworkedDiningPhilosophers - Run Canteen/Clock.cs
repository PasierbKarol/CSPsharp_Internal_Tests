using CSPlang;

namespace NetworkedDiningPhilosophers
{
    // copyright 2012-13 Jon Kerridge
    // Let's Do It In Parallel


    class Clock : IamCSProcess
    {
        ChannelOutput toConsole;


        public Clock(ChannelOutput toConsole)
        {
            this.toConsole = toConsole;
        }

        public void run()
        {
            CSTimer tim = new CSTimer();
            int tick = 0;

            while (true)
            {
                toConsole.write("Time: " + tick + " \n");
                tim.sleep(1000);
                tick = tick + 1;
            }
        }
    }
}