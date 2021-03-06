// copyright 2012-13 Jon Kerridge
// Let's Do It In Parallel


using System;
using CSPlang;

namespace NetworkedDiningPhilosophers
{
    class QueuingCanteen : IamCSProcess
    {
        ChannelInput service;
        ChannelOutput deliver;
        ChannelInput supply;
        ChannelOutput toConsole;

        public QueuingCanteen(ChannelInput service, ChannelOutput deliver, ChannelInput supply, ChannelOutput toConsole)
        {
            this.service = service;
            this.deliver = deliver;
            this.supply = supply;
            this.toConsole = toConsole;
        }

        public void run()
        {
            Guard[] canteenGuards = {supply as Guard, service as Guard};
            Boolean[] precondition = {true, false};
            var canteenAlt = new Alternative(canteenGuards);

            const int SUPPLY = 0;
            const int SERVICE = 1;

            CSTimer tim = new CSTimer();
            int chickens = 0;

            toConsole.write("Canteen : starting ... \n");

            while (true)
            {
                precondition[SERVICE] = (chickens > 0);
                if (chickens == 0)
                {
                    toConsole.write("Waiting for chickens ...\n");
                }

                switch (canteenAlt.fairSelect(precondition))
                {
                    case SUPPLY:
                        int value = (int) (long) supply.read();
                        toConsole.write("Chickens on the way ...\n");
                        tim.after(tim.read() + 3000);
                        chickens = chickens + value;
                        toConsole.write(chickens + " chickens now available ...\n");
                        supply.read();
                        break;
                    case SERVICE:
                        int id = (int) (long) service.read();
                        chickens = chickens - 1;
                        toConsole.write("chicken ready for Philosoper " + id + " ... " + chickens +
                                        " chickens left \n");
                        deliver.write(1);
                        break;
                }
            }
        }
    }
}