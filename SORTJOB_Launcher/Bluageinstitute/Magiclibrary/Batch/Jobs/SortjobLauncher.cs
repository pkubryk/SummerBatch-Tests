using Summer.Batch.Core;
using System;
using System.Diagnostics;

namespace Bluageinstitute.Magiclibrary.Batch.Jobs
{
    /// <summary>
    /// Class for launching the SORTJOB job.
    /// </summary>
    public static class SortjobLauncher
    {
        /// <summary>
        /// Launches the job.
        /// </summary>
        /// <param name="args">The arguments for the job execution.</param>
        public static int Main(string[] args)
        {
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            JobExecution jobExecution = JobStarter.Start(@"Bluageinstitute\Magiclibrary\Batch\Jobs\SORTJOB.xml", new SortjobUnityLoader());

#if DEBUG
            stopwatch.Stop();
            Console.WriteLine(Environment.NewLine + "Done in {0}ms.", stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Press a key to end.");
            Console.ReadKey();
#endif

            return (int) (jobExecution.Status == BatchStatus.Completed ? JobStarter.Result.Success : JobStarter.Result.Failed);
        }
    }
}
