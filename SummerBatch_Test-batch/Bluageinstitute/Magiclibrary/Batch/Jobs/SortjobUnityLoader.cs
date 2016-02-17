using Microsoft.Practices.Unity;
using Summer.Batch.Core.Step.Tasklet;
using Summer.Batch.Core.Unity;
using Summer.Batch.Extra;
using Summer.Batch.Extra.Sort;
using System;
using System.Collections.Generic;

namespace Bluageinstitute.Magiclibrary.Batch.Jobs
{
    /// <summary>
    /// Implementation of <see cref="ContextManagerUnityLoader" /> for job SORTJOB.
    /// </summary>
    public class SortjobUnityLoader : ContextManagerUnityLoader
    {
        /// <summary>
        /// Registers the artifacts required to execute the steps (tasklets, readers, writers, etc.)
        /// </summary>
        /// <param name="container">the unity container to use for registrations</param>
        public override void LoadArtifacts(IUnityContainer container)
        {
            RegisterStep0Tasklet(container);
            RegisterStep1Tasklet(container);
        }

        // Step step0 - Sort step
        private void RegisterStep0Tasklet(IUnityContainer container)
        {
            IList<OutputFile> list = new List<OutputFile>();
            var outputFile1 = new OutputFile
            {
                Include = "75,2,CH,EQ,C'RD'",
                Outrec = "25,200"
            };
            var outputFile2 = new OutputFile
            {
                Include = "75,2,CH,EQ,C'XD'"
            };
            var outputFile3 = new OutputFile
            {
                Include = "75,2,CH,EQ,C'CD'"
            };
            var outputFile4 = new OutputFile
            {
                Include = "75,2,CH,EQ,C'YD'"
            };
            var outputFile5 = new OutputFile
            {

            };
            list.Add(outputFile1);
            list.Add(outputFile2);
            list.Add(outputFile3);
            list.Add(outputFile4);
            list.Add(outputFile5);
            container.StepScopeRegistration<ITasklet, CustomSortTasklet>("step0Batchlet")
                .Property("Input").Resources("#{settings['SORTJOB.step0.inputFiles']}")
                .Property("Output").Resource("#{settings['SORTJOB.step0.outputFile']}")
                .Property("HeaderSize").Value(0)
                .Property("outputFile").Value(list)
                .Property("Separator").Value(Environment.NewLine)
                .Property("SortCard").Value("FORMAT=CH,FIELDS=(121,14,CH,A)")
                .Register();
        }

        // Step step1 - Sort step
        private void RegisterStep1Tasklet(IUnityContainer container)
        {
            container.StepScopeRegistration<ITasklet, SortTasklet>("step1Batchlet")
                .Property("Input").Resources("#{settings['SORTJOB.step1.inputFiles']}")
                .Property("Output").Resource("#{settings['SORTJOB.step1.outputFile']}")
                .Property("HeaderSize").Value(0)
                .Property("Include").Value("(75,2,BI,NE,X'5244')")
                .Property("Separator").Value(Environment.NewLine)
                .Property("SortCard").Value("FORMAT=CH,FIELDS=(121,14,CH,A)")
                .Register();
        }
    }
}
