using CsvHelper;
using CsvHelper.Configuration;
using EA.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Loggers.CSV
{
    public class CSVLogger<T, Record> : ILogger<T>, IDisposable where T : ISpecimen<T> where Record : IRecord<T>, new()
    {
        public string FilePath { get; set; }

        public Task LoggerThread => this.loggerThread;
        public Queue<Task> LoggerTasks { get; }

        private FileStream CsvStream { get; set; }
        private CsvWriter CsvWriter { get; set; }
        private Task loggerThread;
        public CancellationTokenSource CancellationTokenSource { get; }

        public CSVLogger(string filePath)
        {
            this.FilePath = filePath;
            this.CancellationTokenSource = new CancellationTokenSource();
            this.LoggerTasks = new Queue<Task>();
        }

        public void RunLogger()
        {
            this.CsvStream = new FileStream(this.FilePath, FileMode.Create, FileAccess.Write, FileShare.Read);
            this.CsvWriter = new CsvWriter(new StreamWriter(this.CsvStream)
                , new CsvConfiguration(CultureInfo.CurrentCulture)
                {
                    Delimiter = ";",
                }
            );
            if (!this.CsvStream.CanWrite)
            {
                throw new IOException($"CSVLogger: Cannot create file for logging at {this.FilePath}");
            }
            this.CsvWriter.WriteHeader(typeof(Record));
            this.CsvWriter.NextRecord();
            this.loggerThread = new Task(LoggerLoop);
            this.loggerThread.Start();
        }

        private void LoggerLoop()
        {
            while (!this.CancellationTokenSource.Token.IsCancellationRequested)
            {
                if(this.LoggerTasks.Count == 0)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    var task = this.LoggerTasks.Dequeue();
                    task.Start();
                    task.Wait();
                }
            }
        }

        public Task Log(int currentEpoch, IList<T> currentEpochSpecimens)
        {
            int currentEpochCopy = currentEpoch;
            var specimensCopy = currentEpochSpecimens;
            Task newTask = new Task(() =>
            {
                LogSpecimens(currentEpochCopy, specimensCopy);
            });
            this.LoggerTasks.Enqueue(newTask);
            return newTask;
        }

        private void LogSpecimens(int currentEpoch, IList<T> currentEpochSpecimens)
        {
            int index = 0;
            foreach (var specimen in currentEpochSpecimens)
            {
                var record = new Record();
                record.ApplyData(currentEpoch, index++, specimen);
                this.CsvWriter.WriteRecord<Record>(record);
                this.CsvWriter.NextRecord();
            }
            this.CsvWriter.Flush();
        }

        public void Wait()
        {
            while(this.LoggerTasks.Count > 0)
            {
                Thread.Sleep(100);
            }
            this.CancellationTokenSource.Cancel();
            this.LoggerThread.Wait();
        }

        public void Dispose()
        {
            if (!this.CancellationTokenSource.IsCancellationRequested)
            {
                this.CancellationTokenSource.Cancel();
            }
            this.CsvWriter.Dispose();
        }
    }
}
