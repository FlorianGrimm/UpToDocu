//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace UpToDocu {
//    //public interface IUtdPipelineBuilder<in TValue> {
//    //    IUtdPipelineBuilder<TValue> Source();

//    //    IUtdPipeline<TValue, TResult> Build<TResult>();
//    //}

//    //public interface IUtdPipelineBuilder<in TValue, out TResult> {
//    //    IUtdPipelineSource<in TValue> Source();

//    //    IUtdPipeline<TValue, TResult> Build();
//    //}

//    //public interface IUtdPipelineSource<in TValue> { 
//    //}


//    //public class UtdPipelineBuilder<TValue, TResult> : IUtdPipelineBuilder<TValue, TResult> {
//    //    public UtdPipelineBuilder() {
//    //    }

//    //    public IUtdPipeline<TValue, TResult> Build() {
//    //        throw new NotImplementedException();
//    //    }

//    //    public void Source() {
//    //        throw new NotImplementedException();
//    //    }
//    //}

//    public class UtdPipelineSourceBuilder<TValue> {
//        //Task Sink();
//        //Task OnNext(TValue data);
//        //Task OnCompleted();
//        //Task OnError();
//        public UtdPipelineSourceBuilder<TResult> Then<TResult>(
//            UpToDocu.UtdObject spec,
//            Func<TValue, UpToDocu.UtdValue<TResult>> step
//            ) {
//            throw new NotImplementedException();
//        }
//        public UtdPipelineSourceAsyncBuilder<Task<TResult>> Then<TResult>(
//            UpToDocu.UtdObject spec,
//            Func<TValue, Task<UpToDocu.UtdValue<TResult>>> stepAsync
//            ) {
//            throw new NotImplementedException();
//        }
//        public UtdPipelineSourceAsyncBuilder<TResult> If<TResult>(
//            UpToDocu.UtdObject spec,
//            Func<TValue, UpToDocu.UtdValue<TResult>> ifValue,
//            Func<UtdValue<TResult>>? ifFailed = default,
//            Func<Exception, UtdValue<TResult>>? ifError = default
//            ) {
//            throw new NotImplementedException();
//        }
//        public UtdPipelineSourceAsyncBuilder<TResult> If<TResult>(
//            UpToDocu.UtdObject spec,
//            Func<TValue, Task<UpToDocu.UtdValue<TResult>>> ifValueAsync,
//            Func<UtdValue<TResult>>? ifFailed = default,
//            Func<Exception, UtdValue<TResult>>? ifError = default
//            ) {
//            throw new NotImplementedException();
//        }
//    }
//    public class UtdPipelineSourceAsyncBuilder<TValue> {
//        public UtdPipelineSourceAsyncBuilder<TResult> Then<TResult>(
//           this UtdPipelineSourceAsyncBuilder<TValue> that,
//           UpToDocu.UtdObject spec,
//           Func<TValue, UpToDocu.UtdValue<TResult>> step
//           ) {
//            throw new NotImplementedException();
//        }
//        public UtdPipelineSourceAsyncBuilder<Task<TResult>> Then<TResult>(
//            UpToDocu.UtdObject spec,
//            Func<TValue, Task<UpToDocu.UtdValue<TResult>>> stepAsync
//            ) {
//            throw new NotImplementedException();
//        }
//        public UtdPipelineSourceAsyncBuilder<TResult> If<TResult>(
//            UpToDocu.UtdObject spec,
//            Func<TValue, UpToDocu.UtdValue<TResult>> ifValue,
//            Func<UtdValue<TResult>>? ifFailed = default,
//            Func<Exception, UtdValue<TResult>>? ifError = default
//            ) {
//            throw new NotImplementedException();
//        }
//        public UtdPipelineSourceAsyncBuilder<TResult> If<TResult>(
//            UpToDocu.UtdObject spec,
//            Func<TValue, Task<UpToDocu.UtdValue<TResult>>> ifValueAsync,
//            Func<UtdValue<TResult>>? ifFailed = default,
//            Func<Exception, UtdValue<TResult>>? ifError = default
//            ) {
//            throw new NotImplementedException();
//        }
//    }

//    /*
//    public interface IUtdPipelineSourceBuilder<in TValue> {
//        //Task Sink();
//        //Task OnNext(TValue data);
//        //Task OnCompleted();
//        //Task OnError();
//    }
//    public interface IUtdPipelineSourceAsyncBuilder<in TValue> {
//    }
//    public static class UtdPipelineSourceBuilderExtensions {
//        public static IUtdPipelineSourceBuilder<TResult> Then<TValue, TResult>(
//            this IUtdPipelineSourceBuilder<TValue> that,
//            UpToDocu.UtdObject spec,
//            Func<TValue, UpToDocu.UtdValue<TResult>> step
//            ) {
//            throw new NotImplementedException();
//        }
//        public static IUtdPipelineSourceAsyncBuilder<Task<TResult>> Then<TValue, TResult>(
//            this IUtdPipelineSourceBuilder<TValue> that,
//            UpToDocu.UtdObject spec,
//            Func<TValue, Task<UpToDocu.UtdValue<TResult>>> stepAsync
//            ) {
//            throw new NotImplementedException();
//        }
//        public static IUtdPipelineSourceAsyncBuilder<TResult> If<TValue, TResult>(
//            this IUtdPipelineSourceBuilder<TValue> that,
//            UpToDocu.UtdObject spec,
//            Func<TValue, UpToDocu.UtdValue<TResult>> ifValue,
//            Func<UtdValue<TResult>>? ifFailed = default,
//            Func<Exception, UtdValue<TResult>>? ifError = default
//            ) {
//            throw new NotImplementedException();
//        }
//        public static IUtdPipelineSourceAsyncBuilder<TResult> If<TValue, TResult>(
//            this IUtdPipelineSourceAsyncBuilder<TValue> that,
//            UpToDocu.UtdObject spec,
//            Func<TValue, Task<UpToDocu.UtdValue<TResult>>> ifValueAsync,
//            Func<UtdValue<TResult>>? ifFailed = default,
//            Func<Exception, UtdValue<TResult>>? ifError = default
//            ) {
//            throw new NotImplementedException();
//        }

//    }
//    public static class UtdPipelineSourceBuilderAsyncExtensions {
//        public static IUtdPipelineSourceAsyncBuilder<TResult> Then<TValue, TResult>(
//            this IUtdPipelineSourceAsyncBuilder<TValue> that,
//            UpToDocu.UtdObject spec,
//            Func<TValue, UpToDocu.UtdValue<TResult>> step
//            ) {
//            throw new NotImplementedException();
//        }
//        public static IUtdPipelineSourceAsyncBuilder<Task<TResult>> Then<TValue, TResult>(
//            this IUtdPipelineSourceAsyncBuilder<TValue> that,
//            UpToDocu.UtdObject spec,
//            Func<TValue, Task<UpToDocu.UtdValue<TResult>>> stepAsync
//            ) {
//            throw new NotImplementedException();
//        }
//        public static IUtdPipelineSourceAsyncBuilder<TResult> If<TValue, TResult>(
//            this IUtdPipelineSourceAsyncBuilder<TValue> that,
//            UpToDocu.UtdObject spec,
//            Func<TValue, UpToDocu.UtdValue<TResult>> ifValue,
//            Func<UtdValue<TResult>>? ifFailed = default,
//            Func<Exception, UtdValue<TResult>>? ifError = default
//            ) {
//            throw new NotImplementedException();
//        }
//        public static IUtdPipelineSourceAsyncBuilder<TResult> If<TValue, TResult>(
//            this IUtdPipelineSourceAsyncBuilder<TValue> that,
//            UpToDocu.UtdObject spec,
//            Func<TValue, Task<UpToDocu.UtdValue<TResult>>> ifValueAsync,
//            Func<UtdValue<TResult>>? ifFailed = default,
//            Func<Exception, UtdValue<TResult>>? ifError = default
//            ) {
//            throw new NotImplementedException();
//        }
//    }
//    */
//    public interface IUtdPipelineSource<in TValue> {
//        //Task Sink();
//        //Task OnNext(TValue data);
//        //Task OnCompleted();
//        //Task OnError();
//    }
//    public interface IUtdPipeline<in TValue, out TResult> {
//        //Task Sink();
//        //Task OnNext(TValue data);
//        //Task OnCompleted();
//        //Task OnError();
//    }

//    public class UtdPipelineBuilder<TValue> {
//        public UtdPipelineSourceBuilder<TValue> Source() {
//            throw new NotImplementedException();
//        }

//        public IUtdPipeline<TValue, TResult> Build<TResult>() {
//            throw new NotImplementedException();
//        }
//    }
//}