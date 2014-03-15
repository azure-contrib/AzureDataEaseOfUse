using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataEaseOfUse
{
    public class TaskManager
    {

        private List<Task> Tasks = new List<Task>();


        public void Add(Task task)
        {
            lock (Tasks)
            {
                Tasks.Add(task);
            }

            task.ContinueWith(Remove);
        }

        private void Remove(Task task)
        {
            lock (Tasks)
            {
                Tasks.Remove(task);
            }
        }

        public int Count()
        {
            lock (Tasks)
            {
                return Tasks.Count;
            }
        }

        public void Wait()
        {
            var toWaitFor = WhenAll();

            toWaitFor.Wait();
        }

        public Task WhenAll()
        {
            lock (Tasks)
            {
                return Task.WhenAll(Tasks);
            }
        }


    }
}
