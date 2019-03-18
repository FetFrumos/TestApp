using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameList.Core
{
    public class ActionTimer
    {
	    private Timer _timer;
	    private Action _action;

	    public void InitAndStart(Action action)
	    {
		    _action = action;
		    if (_timer == null)
		    {
			    _timer = new Timer(Tick, null, StarTimerSec * 1000, Timeout.Infinite);
		    }
	    }

	    private const int StarTimerSec = 5;

		public void Restart()
	    {
		    _timer.Change(StarTimerSec * 1000, Timeout.Infinite);
	    }

	    private void Tick(object state)
	    {
		    _action();
			Stop();
	    }
		
	    public void Stop()
	    {
		    _timer.Change(Timeout.Infinite, Timeout.Infinite);
	    }
	}
}
