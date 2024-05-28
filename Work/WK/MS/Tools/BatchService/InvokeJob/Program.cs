using Autofac;
using BatchService.Job.Base;
using BatchService.Model.Enum;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendServiceNF.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvokeJob
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + "\\";
            ContainerBuilder builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, null);
            builder = DependencyUtilNF.GetJxBackendServiceContainerBuilder(assemblyPath, builder);
            builder = DependencyUtilNF.GetJxBackendServiceContainerBuilder(assemblyPath, builder);
            DependencyUtil.SetContainer(builder.Build());

            string jobName = null;

            if (args.AnyAndNotNull())
            {
                jobName = args.FirstOrDefault();
            }

            List<Type> jobTypes = JobSetting.GetAll().Select(s => s.JobType).ToList();

            Console.WriteLine(" =================== Job Name =================== ");

            for (int i = 0; i < jobTypes.Count; i++)
            {
                Type type = jobTypes[i];
                Console.WriteLine($"{i:D2}：{type.Name}");
            }

            Console.WriteLine(" =================== Job Name =================== ");

            if (jobName.IsNullOrEmpty())
            {
                Console.WriteLine("Please Enter Job Name. (EX：QAVIPAgentCommissionComputingJob)");
                Console.Write("Job Name：");
            }

            jobName = Console.ReadLine();
            Type jobType = jobTypes.Where(w => w.Name.Equals(jobName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

            if (jobType == null)
            {
                Console.Write("Job Not Found");
                Console.ReadLine();
                return;
            }

            BaseQuartzJob job = Activator.CreateInstance(jobType) as BaseQuartzJob;
            job.DoJob();

            Console.Write("Job Finish");
            Console.ReadLine();
        }
    }
}