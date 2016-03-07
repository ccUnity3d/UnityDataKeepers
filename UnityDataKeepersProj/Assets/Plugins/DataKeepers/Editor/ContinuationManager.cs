using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace DataKeepers.Manager
{
    internal static class ContinuationManager
    {
        private class Job
        {
            public Job(Func<bool> completed, Action continueWith)
            {
                Completed = completed;
                ContinueWith = continueWith;
            }
            public Func<bool> Completed { get; private set; }
            public Action ContinueWith { get; private set; }
        }

        private static readonly List<Job> Jobs = new List<Job>();

        public static void Add(Func<bool> completed, Action continueWith)
        {
            if (!Jobs.Any()) EditorApplication.update += Update;
            Jobs.Add(new Job(completed, continueWith));
        }

        private static void Update()
        {
            for (int i = 0; i >= 0; --i)
            {
                var jobIt = Jobs[i];
                if (jobIt.Completed())
                {
                    jobIt.ContinueWith();
                    Jobs.RemoveAt(i);
                }
            }
            if (!Jobs.Any()) EditorApplication.update -= Update;
        }
    }
}