using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpToDocu.Console {
    public static class ExceptionExtensions {
        public static bool WriteError(Exception error) {
            System.Console.Error.WriteLine(error.ToString());
            if (error is System.AggregateException aggregateException) {
                aggregateException.Handle(innerWriteError);
                //foreach (var innerException in aggregateException.InnerExceptions) {
                //    writeError(innerException);
                //}
            }
            return true;
        }

        private static bool innerWriteError(Exception error) {
            System.Console.Error.WriteLine(error.ToString());
            if (error is System.AggregateException aggregateException) {
                foreach (var innerException in aggregateException.InnerExceptions) {
                    innerWriteError(innerException);
                }
            }
            return true;
        }

    }
}
