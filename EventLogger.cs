using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteRuntimeTagViewer
{
	public class EventLogger
	{

		public static void LogEvent(string message)
		{
			//Log to file

			using (StreamWriter w = File.AppendText("log.txt"))
			{
				w.WriteLine(message);
			}
			
		}

	}

	
}
